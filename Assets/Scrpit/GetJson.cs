using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;
using Photon.Pun;

public class GetJson : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    void Start()
    {
    }

    void Update()
    {

    }

    public void  newPokemon(int random)
    {

        StartCoroutine(GetRequest("https://api-pokemon-fr.vercel.app/api/v1/pokemon/" + random));
    }


    public IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            var N = JSON.Parse(webRequest.downloadHandler.text);
            string name = N["name"][0];
            string imageUrl = N["sprites"][0];
            GameManager.instance.LoadImageForAll(imageUrl);
            GameManager.instance.StartGame(name);
        }

    }

    

    public IEnumerator GetImage(string imageUrl)
    {
        GameManager.instance.SetReady(false);
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request) as Texture2D;
            GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            GameObject panel = GameObject.Find("GamePanel");
            GameManager.instance.SetReady(true);
            while (!GameManager.instance.AllPlayerReady()) yield return new WaitForSeconds(1);
            for (int i = 0; i < 4; i++)
            {
                panel.transform.GetChild(i + 1).GetComponent<Button>().enabled = true;
                panel.transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
            }
            GameManager.instance.SetReady(false);
        }
    }
}

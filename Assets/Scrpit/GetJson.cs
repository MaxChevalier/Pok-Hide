using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

public class GetJson : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
    }

    void Update()
    {

    }

    public void newPokemon(int random)
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
            StartCoroutine(GetImage(imageUrl));
            GameManager.instance.StartGame(name);
        }

    }

    IEnumerator GetImage(string imageUrl)
    {
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
            for (int i = 0; i < 4; i++)
            {
                panel.transform.GetChild(i + 1).GetComponent<Button>().enabled = true;
                panel.transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }
    }
}

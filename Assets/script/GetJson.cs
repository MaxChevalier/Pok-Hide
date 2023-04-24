using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;

public class GetJson : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        int random = Random.Range(1, 1010);
        print(random);
        StartCoroutine(GetRequest("https://api-pokemon-fr.vercel.app/api/v1/pokemon/" + random));
        
    }

    void Update()
    {
       
    }


    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
           yield return webRequest.SendWebRequest();

            print(webRequest.downloadHandler.text);
            var N = JSON.Parse(webRequest.downloadHandler.text);
            string name = N["name"][0];
            string imageUrl = N["sprites"][0];
            print(name);    
            StartCoroutine(GetImage(imageUrl));
            print(imageUrl);
        }
       
    }

    IEnumerator GetImage(string imageUrl){
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request) as Texture2D;
            GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0,0));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageFromUrl : MonoBehaviour
{
    // Start is called before the first frame update


    public string imageUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/83.png";

    void Start()
    {
        StartCoroutine(GetImage(imageUrl));
    }

    // Update is called once per frame
    void Update()
    {
        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleJSON;
using UnityEngine.Networking;

public class PC : MonoBehaviour
{
    [SerializeField] private GameObject card;
    [SerializeField] private List<Sprite> images;
    
    void Start()
    {
        ShowPC();
    }

    public void ShowPC()
    {
        List<PokemonPC> Pokemons = GetComponent<GestionDB>().GetInDB();
        foreach (PokemonPC pokemon in Pokemons)
        {
            GameObject newCard = Instantiate(card, transform);
            StartCoroutine(LoadImg(newCard, pokemon.sprite));
            newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pokemon.name;
            newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "#" + pokemon.pokedexId.ToString();
            newCard.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.generation.ToString();
            newCard.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = pokemon.category;
            for (int i = 0; i < images.Count; i++)
            {
                Debug.Log(images[i].name);
                if (pokemon.type1.ToLower() == images[i].name)
                {
                    newCard.transform.GetChild(5).GetChild(0).GetComponent<Image>().sprite = images[i];
                }else if (pokemon.type2.ToLower() == images[i].name)
                {
                    newCard.transform.GetChild(5).GetChild(1).GetComponent<Image>().sprite = images[i];
                }
            }
            if (pokemon.type1 == null || pokemon.type1 == "")
            {
                GameObject.Destroy(newCard.transform.GetChild(5).GetChild(0).gameObject);
            }
            if (pokemon.type2 == null || pokemon.type2 == "")
            {
                GameObject.Destroy(newCard.transform.GetChild(5).GetChild(1).gameObject);
            }
            newCard.transform.SetParent(transform);
        }
    }

    private IEnumerator LoadImg(GameObject newCard , string imageUrl){
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request) as Texture2D;
            newCard.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        }
    }
}

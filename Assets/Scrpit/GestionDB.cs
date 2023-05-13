using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

public class GestionDB : MonoBehaviour
{

    private string DB = "URI=file:PC.db";

    void Start()
    {
        CreateDB();
    }

    void Update()
    {

    }


    public void CreateDB()
    {
        using (var conn = new SqliteConnection(DB))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS PC (pokedexId int(11) NOT NULL PRIMARY KEY, generation int(11) NOT NULL, name varchar(255) NOT NULL, category varchar(255) NOT NULL, sprite varchar(255) NOT NULL, type1 varchar(255), type2 varchar(255))";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }

    public void addPokemon( int pokedexId, int generation, string name, string category, string sprite, string type1, string type2){

        using (var conn = new SqliteConnection(DB))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO PC (pokedexId, generation, name, category, sprite, type1, type2) VALUES ('" + pokedexId + "', '" + generation + "', '" + name + "', '" + category + "', '" + sprite + "', '" + type1 + "', '" + type2 + "')";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        
    }

    public void newPokemonInDB(string name)
    {

        StartCoroutine(GetRequestPokemonPc("https://api-pokemon-fr.vercel.app/api/v1/pokemon/" + name));
    }


    public IEnumerator GetRequestPokemonPc(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            var N = JSON.Parse(webRequest.downloadHandler.text);
            int pokedexId = N["pokedexId"][0];
            int generation = N["generation"][0];
            string name = N["name"][0];
            string category = N["category"][0];
            string imageUrl = N["sprites"][0];
            string type1 = N["type"][0][1];
            string type2 = N["type"][1][1];

            addPokemon(pokedexId, generation, name, category, imageUrl, type1, type2);
        }

    }
    
}
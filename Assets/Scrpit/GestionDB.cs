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

    private string DB = "URI=file:Assets/DB/PC.db";

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

    public List<PokemonPC> GetInDB()
    {
        List<PokemonPC> listPokemon = new List<PokemonPC>();
        using (var conn = new SqliteConnection(DB))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM PC";
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PokemonPC pokemon = new PokemonPC();
                        pokemon.pokedexId = reader.GetInt32(0);
                        pokemon.generation = reader.GetInt32(1);
                        pokemon.name = reader.GetString(2);
                        pokemon.category = reader.GetString(3);
                        pokemon.sprite = reader.GetString(4);
                        pokemon.type1 = reader.GetString(5);
                        pokemon.type2 = reader.GetString(6);
                        listPokemon.Add(pokemon);
                    }
                }
            }
            conn.Close();
        }
        return listPokemon;
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

    public void newPokemonInDB(int id)
    {
        Debug.Log("newPokemonInDB " + name );
        StartCoroutine(GetRequestPokemonPc(GetJson.URL + id));
    }


    public IEnumerator GetRequestPokemonPc(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            var N = JSON.Parse(webRequest.downloadHandler.text);
            int pokedexId = N["pokedexId"];
            int generation = N["generation"];
            string name = N["name"][0];
            string category = N["category"];
            string imageUrl = N["sprites"][0];
            string type1 = N["types"][0]["name"];
            string type2 = N["types"][1]["name"];

            addPokemon(pokedexId, generation, name, category, imageUrl, type1, type2);
        }

    }
    
}
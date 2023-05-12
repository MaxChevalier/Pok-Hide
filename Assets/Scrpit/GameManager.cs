using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameObject image;
    private int score = 0;

    private int round = 0;

    private float time = 0;

    private int rdm = 1;

    public bool ready = false;

    public Image GamePanel;
    public Image EndGame;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        EndGame.gameObject.SetActive (false);
        ready = true;
        image = GameObject.Find("Poke_Image");
        ReStart();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 10)
        {
            Debug.Log("To late");
            score -= 350;
            time = 0;
            ReStart();
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartGame(string PokemonName)
    {

        rdm = Random.Range(0, 4);
        string name = PokemonName;
        List<string> Names = new List<string>() { name };
        for (int i = 0; i < 4; i++)
        {
            if (i == rdm)
            {
                GameObject.Find("GamePanel").transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            }
            else
            {
                string name_ = ChangeName(name, Names);
                Names.Add(name_);
                GameObject.Find("GamePanel").transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name_;
            }
        }

    }



    public void ReStart()
    {
        if (round == 10)
        {
            EndGame.gameObject.SetActive (true);
            GamePanel.gameObject.SetActive (false);
            // QuitGame();
        }
        else
        {
                GameObject panel = GameObject.Find("GamePanel");
                for (int i = 0; i < 4; i++)
                {
                    panel.transform.GetChild(i + 1).GetComponent<Button>().enabled = false;
                    panel.transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
                }
                int random = Random.Range(1, 1010);
                image.GetComponent<GetJson>().newPokemon(random);
                round++;
                print(round);


        }

    }


    public void IsGoodAnswer(int ID)
    {

        if (rdm == ID)
        {
            Debug.Log("Good Answer");
            int Inttime = (int)(time * 100);
            score += 1000 - (Inttime);

        }
        else
        {
            Debug.Log("Bad Answer");
            score -= 350;
        }
        time = 0;
        print("Score" + score);
        ReStart();
    }

    public string ChangeName(string name, List<string> names)
    {
        name = name.ToLower();
        string tmpName = "";
        int countTag = 0;
        do
        {
            tmpName = "";
            for (int j = 0; j < name.Length; j++)
            {
                if (j > 0 && (name[j] == 'u' && name[j - 1] == 'q')) continue;

                if (name[j] == 'o' || name[j] == 'a')
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        tmpName += "o";
                    }
                    else
                    {
                        tmpName += "a";
                    }
                }
                else if (name[j] == 'k' || name[j] == 'c' || (j < name.Length && name[j] == 'q' && name[j + 1] == 'u'))
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            tmpName += "k";
                            break;

                        case 1:
                            tmpName += "c";
                            break;

                        case 2:
                            tmpName += "qu";
                            j++;
                            break;
                    }
                }
                else if (name[j] == 't' || name[j] == 'd')
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        tmpName += "d";
                    }
                    else
                    {
                        tmpName += "t";
                    }
                }
                else
                {
                    tmpName += name[j];
                }
            }
            tmpName = char.ToUpper(tmpName[0]) + tmpName.Substring(1);
            countTag++;
        } while ((names.Contains(tmpName) && countTag < 10) || tmpName.ToLower() == name);
        return tmpName;
    }

    public void Setimg(Texture2D texture)
    {
        image.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
    }
}

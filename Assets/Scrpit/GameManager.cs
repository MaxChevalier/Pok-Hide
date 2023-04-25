using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
            Debug.Log("Game Manager already exist");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        int rdm = Random.Range(0, 4);
        string name = "Mordudor";
        List<string> Names = new List<string>(){name};
        for (int i = 0; i < 4; i++)
        {
            if (i == rdm)
            {
                GameObject.Find("GamePanel").transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            }
            else
            {
                string name_ = ChangeName(name,Names);
                Names.Add(name_);
                GameObject.Find("GamePanel").transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name_;
            }
        }

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
                if (j>0 && (name[j]=='u' && name[j-1]=='q')) continue;

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
                else if (name[j] == 'k' || name[j] == 'c' || (j < name.Length && name[j] == 'q' && name[j+1] == 'u'))
                {
                    switch (Random.Range(0, 3) )
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
            Debug.Log(tmpName);
            for (int g = 0; g < names.Count; g++)
            {
                Debug.Log(names[g]);
            }
            tmpName = char.ToUpper(tmpName[0]) + tmpName.Substring(1);
            countTag++;
        } while ((names.Contains(tmpName) && countTag < 10) || tmpName.ToLower() == name);
        return tmpName;
    }
}

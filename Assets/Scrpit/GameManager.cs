using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public GameObject image;
    private int score = 0;

    private int round = 0;

    private float time = 0;

    private int rdm = 1;
    private bool ready = false;

    public GameObject gamePanel;
    public GameObject endGame;
    public GameObject watingPanel;
    private bool isWating = true;
    private int timeToWait = 0;
    public TextMeshProUGUI Timer;
    private int minplayer = 2;
    private int waitingTime = 10;
    private ExitGames.Client.Photon.Hashtable hash;
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
        endGame.SetActive(false);
        gamePanel.SetActive(false);
        watingPanel.SetActive(true);
        // ReStart();
        hash = new ExitGames.Client.Photon.Hashtable() { { "score", score }, { "round", round }, { "ready", ready } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    // Update is called once per frame
    void Update()
    {
        if (isWating)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == minplayer)
            {
                isWating = false;
                timeToWait = waitingTime;
                StartCoroutine(TimeToStart());
            }
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < minplayer)
            {
                Timer.text = "";
                isWating = true;
            }
            else if (timeToWait == 0)
            {
                time += Time.deltaTime;
                if (time > 10 && round < 10)
                {
                    score -= 350;
                    SetReady(true);
                    time = 0;
                    ReStart();
                }
            }
        }
    }

    IEnumerator TimeToStart()
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            while (timeToWait > 0)
            {

                photonView.RPC("SetWatingPanel", RpcTarget.All, timeToWait);
                yield return new WaitForSeconds(1);
                timeToWait--;
            }
            endGame.SetActive(false);
            gamePanel.SetActive(true);
            watingPanel.SetActive(false);
            photonView.RPC("ReStart", RpcTarget.All);
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
        gamePanel.SetActive(true);
        watingPanel.SetActive(false);
        endGame.SetActive(false);
        rdm = Random.Range(0, 4);
        string name = PokemonName;
        List<string> Names = new List<string>() { name };
        for (int i = 0; i < 4; i++)
        {
            if (i == rdm)
            {
                photonView.RPC("SetName", RpcTarget.All, i + 1, name);
            }
            else
            {
                string name_ = ChangeName(name, Names);
                Names.Add(name_);
                photonView.RPC("SetName", RpcTarget.All, i + 1, name_);
            }
        }

    }

    public void SetName(int Id, string name)
    {
        GameObject.Find("GamePanel").transform.GetChild(Id).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }

    public void ReStart()
    {
        endGame.SetActive(false);
        gamePanel.SetActive(true);
        watingPanel.SetActive(false);
        round = (int)PhotonNetwork.MasterClient.CustomProperties["round"];
        if (round == 10)
        {
            endGame.SetActive(true);
            gamePanel.SetActive(false);
            watingPanel.SetActive(false);
            List<Scoreboard> scores = new List<Scoreboard>();
            for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            {
                scores.Add(new Scoreboard(PhotonNetwork.PlayerList[i].NickName, (int)PhotonNetwork.PlayerList[i].CustomProperties["score"]));
            }
            scores.Sort(delegate (Scoreboard x, Scoreboard y)
            {
                return y.score.CompareTo(x.score);
            });
            endGame.GetComponent<ScorebordGame>().SetScorebord(scores);
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                gamePanel.transform.GetChild(i + 1).GetComponent<Button>().enabled = false;
                gamePanel.transform.GetChild(i + 1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
            }
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
            {
                int random = Random.Range(1, 1010);
                image.GetComponent<GetJson>().newPokemon(random);
            }
            round++;

        }

    }


    public void IsGoodAnswer(int ID)
    {

        if (rdm == ID)
        {
            int Inttime = (int)(time * 100);
            score += 1000 - (Inttime);


        }
        else
        {
            score -= 350;
        }
        for (int i = 0; i < 4; i++)
        {
            gamePanel.transform.GetChild(i + 1).GetComponent<Button>().enabled = false;
        }
        time = 0;
        SetHash();
        SetReady(true);
        print("Score" + score);
        StartCoroutine(WaitBeforeRestart());
    }

    private IEnumerator WaitBeforeRestart(){
        while (!AllPlayerReady()) yield return new WaitForSeconds(1);
        photonView.RPC("ReStart", RpcTarget.All);
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

    public void WatingForPlayer()
    {
        gamePanel.SetActive(false);
        watingPanel.SetActive(true);
        endGame.SetActive(false);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetWatingPanel(int timeToWait)
    {
        this.timeToWait = timeToWait;
        Timer.text = "Starting in : " + timeToWait.ToString() + "s";
    }

    public void LoadImage(string imageUrl)
    {
        StartCoroutine(image.GetComponent<GetJson>().GetImage(imageUrl));
    }
    public void LoadImageForAll(string imageUrl)
    {
        if (PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            photonView.RPC("LoadImage", RpcTarget.All, imageUrl);
        }
    }

    private void SetHash()
    {
        hash["score"] = score;
        hash["round"] = round;
        hash["ready"] = ready;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void SetReady(bool ready)
    {
        this.ready = ready;
        SetHash();
    }

    public bool AllPlayerReady()
    {
        bool isReady = true;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount && isReady; i++)
        {
            if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["ready"] == false)
            {
                isReady = false;
            }
        }
        return isReady;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Chat : MonoBehaviourPunCallbacks
{

    public GameObject ChatZone;
    public GameObject ChatInput;


    void Start()
    {
        string message = PhotonNetwork.NickName + "Joined the game";
        photonView.RPC("SendMessage", RpcTarget.All, message);
    }

    void Update()
    {
        
    }

    public void ResaveMessage(string message){
        ChatZone.GetComponent<TMPro.TextMeshProUGUI>().text += message + "\n";
    }

    public void SendMessage(){
        string message = PhotonNetwork.NickName + " : " + ChatInput.GetComponent<TMPro.TMP_InputField>().text;
        photonView.RPC("SendMessage", RpcTarget.All, message);
        ChatInput.GetComponent<TMPro.TMP_InputField>().text = "";
    }

}

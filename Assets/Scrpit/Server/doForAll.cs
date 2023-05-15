using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class doForAll : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void SetWatingPanel(int timeToWait)
    {
        GameManager.instance.SetWatingPanel(timeToWait);
    }

    [PunRPC]
    public void LoadImage(string imageUrl)
    {
        GameManager.instance.LoadImage(imageUrl);
    }

    [PunRPC]
    public void SetName(int Id, string name)
    {
        GameManager.instance.SetName(Id, name);
    }

    [PunRPC]
    public void ReStart(){
        GameManager.instance.ReStart();
    }

    [PunRPC]
    public void SetGoodAnswer(int Id)
    {
        GameManager.instance.rdm = Id;
    }

    [PunRPC]
    public void SetTimeToWait(int timeToWait)
    {
        GameManager.instance.timeToWait = timeToWait;
    }

    [PunRPC]
    public void SetRound(int nbt)
    {
        GameManager.instance.round = nbt;
    }

    [PunRPC]
    public void SendMessage(string message)
    {
        GameManager.instance.chat.ResaveMessage(message);
    }
}

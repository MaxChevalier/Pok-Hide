using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;



public class HostAndJoinRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;

    public void HostRoom(bool isPublic = false)
    {
        char[] codeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        string code = "";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        roomOptions.EmptyRoomTtl = 500;
        roomOptions.PlayerTtl = 500;
        roomOptions.IsVisible = isPublic;
        do {
            code = "";
            for (int i = 0; i < 6; i++)
            {
                code += codeChars[Random.Range(0, codeChars.Length)];
            }
            Debug.Log(code);
        } while (!PhotonNetwork.CreateRoom(code, roomOptions, TypedLobby.Default));
    }

    public void JoinRoom()
    {
        if (!PhotonNetwork.JoinRoom(roomName.text)) Debug.Log("Failed to join room");
    }

    public void QuickGame()
    {
        if (!PhotonNetwork.JoinRandomRoom()) HostRoom(true);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Game");
    }
}

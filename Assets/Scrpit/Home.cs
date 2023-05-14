using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class Home : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameInput;
    public void Play()
    {
        if (nameInput.text == "") return;
        PhotonNetwork.NickName = nameInput.text;
        SceneManager.LoadScene("Host&Play");
    }
}

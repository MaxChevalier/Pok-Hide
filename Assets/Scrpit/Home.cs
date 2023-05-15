using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class Home : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameInput;

    void Start()
    {
        nameInput.text = PhotonNetwork.NickName;
    }
    public void Play()
    {
        if (nameInput.text == "") return;
        PhotonNetwork.NickName = nameInput.text;
        SceneManager.LoadScene("Host&Play");
    }
    public void PC()
    {
        SceneManager.LoadScene("PC");
    }
}

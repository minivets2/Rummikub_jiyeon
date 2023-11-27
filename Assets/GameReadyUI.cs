using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerCount;
    
    public void Update()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                           PhotonNetwork.CurrentRoom.MaxPlayers;
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameObject.SetActive(false);
        }
    }

    public void LeaveButtonClick()
    {
        PhotonNetwork.LoadLevel("Lobby");
        PhotonNetwork.ConnectUsingSettings();
    }
}

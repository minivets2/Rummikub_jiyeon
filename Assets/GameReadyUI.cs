using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyUI : MonoBehaviour
{
    public void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameObject.SetActive(false);
        }
    }

    public void LeaveButtonClick()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}

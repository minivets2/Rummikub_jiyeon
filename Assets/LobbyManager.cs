using System;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputField_PlayerId;
    
    private readonly string gameVersion = "1";
    public TMP_Text connectionInfoText;
    public Button joinButton;

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "Connecting To Master Server...";
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...";

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected && inputField_PlayerId.text.Length != 0)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            RoomManager.Instance.SetPlayerId(inputField_PlayerId.text);
            PhotonNetwork.JoinRandomRoom();
        }
        else if (PhotonNetwork.IsConnected && inputField_PlayerId.text.Length == 0)
        {
            connectionInfoText.text = "Please enter your ID.";
            joinButton.interactable = true;
        }
        else
        {
            connectionInfoText.text = $"Offline : Connection Disabled - Try reconnecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room";
        PhotonNetwork.LoadLevel("Main");
    }
}

using System;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField inputField_PlayerId;
    [SerializeField] private RoomInformation roomInformation;
    [SerializeField] public GameObject players;
    private readonly string gameVersion = "1";
    public TMP_Text connectionInfoText;
    public Button joinButton;

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        roomInformation.SetButton(false);
        connectionInfoText.text = "Connecting To Master Server...";
    }

    private void Update()
    {
        if (PhotonNetwork.CountOfRooms == 0)
        {
            players.gameObject.SetActive(true);
        }
        else
        {
            players.gameObject.SetActive(false);
        }
    }

    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        roomInformation.SetButton(true);
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

        if (PhotonNetwork.IsConnected && inputField_PlayerId.text.Length != 0 && inputField_PlayerId.text.Length <= 3)
        {
            connectionInfoText.text = "Connecting to Random Room...";
            RoomManager.Instance.SetPlayerInfomation(inputField_PlayerId.text, roomInformation.GetPlayerImage(), roomInformation.PlayersCount, roomInformation.GetImageIndex());
            PhotonNetwork.JoinRandomRoom();
        }
        else if (PhotonNetwork.IsConnected && inputField_PlayerId.text.Length == 0)
        {
            connectionInfoText.text = "플레이어 아이디를 입력해주세요.";
            joinButton.interactable = true;
        }
        else if (PhotonNetwork.IsConnected && inputField_PlayerId.text.Length > 3)
        {
            connectionInfoText.text = "플레이어 아이디를 3글자 이하로 작성하세요.";
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
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = roomInformation.PlayersCount });
    }

    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected with Room";
        PhotonNetwork.LoadLevel("Main");
    }
}

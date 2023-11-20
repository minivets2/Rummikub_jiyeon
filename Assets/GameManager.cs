using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<GameObject> players;

    private void Start()
    {
        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            //공유플레이스 생성
        }
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].gameObject.SetActive(false);
            
            if (i == localPlayerIndex) players[i].gameObject.SetActive(true);
        }
    }

    public void Init()
    {
        //players[0].StartTurn();
        //PlaceManager.Instance.SaveCardList();

        for (int i = 0; i < 12; i++)
        {
            //CardManager.Instance.NewCardButtonClick();
        }
    }
}

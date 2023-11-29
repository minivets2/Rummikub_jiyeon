using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<Player> players;

    [Header("Init")]
    [SerializeField] private Transform playerPosition;

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cardManager;
    
    private PhotonView _photonView;
    
    private void OnEnable()
    {
        GameReadyUI.onGameStartEvent += SpawnPlayer;
    }

    private void OnDisable()
    {
        GameReadyUI.onGameStartEvent -= SpawnPlayer;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //공유플레이스 생성
            //PhotonNetwork.Instantiate(cardManager.name, Vector3.zero, Quaternion.identity);
        }
    }

    private void SpawnPlayer()
    {
        _photonView = GetComponent<PhotonView>();
        _photonView.RPC("SpawnGamePlayer_RPC", RpcTarget.AllBufferedViaServer, new object[] { });
        HideOtherPlayer();
    }

    private void HideOtherPlayer()
    {
        foreach (var player in players)
        {
            if (player.PlayerIndex != PhotonNetwork.LocalPlayer.ActorNumber -1)
            {
                player.gameObject.SetActive(false);
            }
        }
    }
    
    [PunRPC]
    public void SpawnGamePlayer_RPC()
    {
        var player =PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(), Quaternion.identity);
        player.transform.SetParent(playerPosition);
        player.transform.localPosition = Vector3.zero;
        player.transform.localScale = Vector3.one;
        player.GetComponent<Player>().InitPlayerInfo(PhotonNetwork.LocalPlayer.ActorNumber -1);
        players.Add(player.GetComponent<Player>());
    }
    
}

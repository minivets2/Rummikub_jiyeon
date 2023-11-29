using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player players;

    [Header("Init")]
    [SerializeField] private Transform playerPosition;

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cardManager;
    
    private PhotonView _photonView;
    
    private void OnEnable()
    {
        GameReadyUI.onGameStartEvent += StartGame;
    }

    private void OnDisable()
    {
        GameReadyUI.onGameStartEvent -= StartGame;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //공유플레이스 생성
            //PhotonNetwork.Instantiate(cardManager.name, Vector3.zero, Quaternion.identity);
        }
    }

    private void StartGame()
    {
        _photonView = GetComponent<PhotonView>();
        _photonView.RPC("SpawnGamePlayer_RPC", RpcTarget.AllBufferedViaServer, new object[] { });
        StuffThattMasterClientDoes();
    }

    private void StuffThattMasterClientDoes()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    _photonView.RPC("UpdatedShuffledCards_RPC", RpcTarget.AllBufferedViaServer, CardManager.Instance.NewCard());
                }
            }
        }
    }
    
    [PunRPC]
    public void UpdatedShuffledCards_RPC(string cardStatus)
    {
        CardManager.Instance.NewCardCreate(cardStatus);
    }

    [PunRPC]
    public void SpawnGamePlayer_RPC()
    {
        var player =PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(), Quaternion.identity);
        player.transform.SetParent(playerPosition);
        player.transform.localPosition = Vector3.zero;
        player.transform.localScale = Vector3.one;
        player.GetComponent<Player>().InitPlayerInfo(PhotonNetwork.LocalPlayer.ActorNumber -1);
        players = player.GetComponent<Player>();
    }
    
}

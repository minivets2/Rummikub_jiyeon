using System.Collections.Generic;
using System.Linq.Expressions;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private PhotonView photonView;

    [Header("Init")]
    [SerializeField] private Transform sharePlacePosition;
    [SerializeField] private Transform playerPosition;

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject sharePlace;
    
    private int _startPlayerIndex;
    private int _currentPlayerIndex;

    public int CurrentPlayerIndex => _currentPlayerIndex;
    
    public delegate void DestroyCardEvent(int row, int column);
    public static DestroyCardEvent destroyCardEvent;
    public delegate void DropCardEvent(string cardStatus, int row, int column);
    public static DropCardEvent dropCardEvent;

    private void OnEnable()
    {
        GameReadyUI.onGameStartEvent += StartGame;
        Player.endTurnEvent += NextTurn;
        SharePlaceManager.cardDropEvent += DropCard;
    }

    private void OnDisable()
    {
        GameReadyUI.onGameStartEvent -= StartGame;
        Player.endTurnEvent -= NextTurn;
        SharePlaceManager.cardDropEvent -= DropCard;
    }

    private void StartGame()
    {
        photonView.RPC("SpawnGamePlayer_RPC", RpcTarget.AllBufferedViaServer, new object[] { });
        StuffThattMasterClientDoes();
    }

    private void NextTurn(int index)
    {
        //새로운 카드 1장 가져가기
        //photonView.RPC("UpdatedShuffledCards_RPC", RpcTarget.AllBufferedViaServer, CardManager.Instance.NewCard(), index);
        
        index++;

        if (PhotonNetwork.PlayerList.Length == index) index = 0;

        photonView.RPC("StartTurn", RpcTarget.AllBufferedViaServer, index);
    }

    private void DropCard(int playerIndex, string cardStatus, int row, int column)
    {
        photonView.RPC("DropCard_RPC", RpcTarget.AllBufferedViaServer, playerIndex, cardStatus, row, column);
    }

    private void StuffThattMasterClientDoes()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(sharePlace.name, new Vector3(), Quaternion.identity);
            
            _startPlayerIndex = new Random().Next(0, PhotonNetwork.PlayerList.Length);

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    photonView.RPC("UpdatedShuffledCards_RPC", RpcTarget.AllBufferedViaServer, CardManager.Instance.GetNewCardStatus(), i);
                }
            }

            photonView.RPC("StartTurn", RpcTarget.AllBufferedViaServer, _startPlayerIndex);
            photonView.RPC("EndTurn", RpcTarget.AllBufferedViaServer, _startPlayerIndex);
        }
    }

    [PunRPC]
    public void StartTurn(int index)
    {
        _currentPlayerIndex = index;
        
        if (index == PhotonNetwork.LocalPlayer.ActorNumber - 1) player.StartTurn();
    }
    
    [PunRPC]
    public void EndTurn(int index)
    {
        if (index != PhotonNetwork.LocalPlayer.ActorNumber - 1) player.EndTurn();
    }
    
    [PunRPC]
    public void UpdatedShuffledCards_RPC(string cardStatus, int index)
    {
        if (index == PhotonNetwork.LocalPlayer.ActorNumber - 1) CardManager.Instance.CardCreate(cardStatus, true);
    }

    [PunRPC]
    public void SpawnGamePlayer_RPC()
    {
        var player =PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(), Quaternion.identity);
        player.transform.SetParent(playerPosition);
        player.transform.localPosition = Vector3.zero;
        player.transform.localScale = Vector3.one;
        player.GetComponent<Player>().InitPlayerInfo(PhotonNetwork.LocalPlayer.ActorNumber -1);
        this.player = player.GetComponent<Player>();
    }
    
    [PunRPC]
    public void DropCard_RPC(int playerIndex, string cardStatus, int row, int column)
    {
        if (playerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1) return;

        destroyCardEvent?.Invoke(row, column);
        dropCardEvent?.Invoke(cardStatus, row, column);
    }

}

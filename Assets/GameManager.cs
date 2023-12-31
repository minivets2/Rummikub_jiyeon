using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player player;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private WinnerUI winnerUI;
    [SerializeField] private OtherPlayers otherPlayers;

    [Header("Init")]
    [SerializeField] private Transform playerPosition;

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject sharePlace;
    
    private int _startPlayerIndex;
    private int _currentPlayerIndex;

    public int CurrentPlayerIndex => _currentPlayerIndex;
    public WinnerUI WinnerUI => winnerUI;
    
    public delegate void DestroyCardEvent(int row, int column);
    public static DestroyCardEvent destroyCardEvent;
    public delegate void DropCardEvent(string cardStatus, int row, int column);
    public static DropCardEvent dropCardEvent;

    private void OnEnable()
    {
        GameReadyUI.onGameStartEvent += StartGame;
        SharePlace.endGameEvent += EndGame;
        SharePlace.nextTurnEvent += NextTurn;
        SharePlaceManager.cardDropEvent += DropCard;
        Player.otherPlayerSettingEvent += OtherPlayerSetting;
    }

    private void OnDisable()
    {
        GameReadyUI.onGameStartEvent -= StartGame;
        SharePlace.endGameEvent -= EndGame;
        SharePlace.nextTurnEvent -= NextTurn;
        SharePlaceManager.cardDropEvent -= DropCard;
        Player.otherPlayerSettingEvent -= OtherPlayerSetting;
    }

    private void StartGame()
    {
        photonView.RPC("SpawnGamePlayer_RPC", RpcTarget.AllBufferedViaServer, new object[] { });
        StuffThattMasterClientDoes();
    }

    private void NextTurn(int playerIndex, bool newCard)
    {
        if (newCard)
            photonView.RPC("UpdatedNewCard", RpcTarget.AllBufferedViaServer, playerIndex);

        playerIndex++;

        if (PhotonNetwork.PlayerList.Length == playerIndex) playerIndex = 0;

        photonView.RPC("StartTurn", RpcTarget.AllBufferedViaServer, playerIndex);
        photonView.RPC("UpdatedOtherPlayerTurn_RPC", RpcTarget.AllBufferedViaServer, playerIndex);
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
            photonView.RPC("UpdatedOtherPlayerTurn_RPC", RpcTarget.AllBufferedViaServer, _startPlayerIndex);
        }
    }

    private void OtherPlayerSetting(int playerIndex, int playerImageIndex, string playerId)
    {
        photonView.RPC("OtherPlayerSetting_RPC", RpcTarget.AllBufferedViaServer, playerIndex, playerImageIndex, playerId);
    }

    private void EndGame(int playerIndex, string winnerID)
    {
        photonView.RPC("EndGame_RPC", RpcTarget.AllBufferedViaServer, playerIndex, winnerID);
    }
    
    [PunRPC]
    public void UpdatedNewCard(int playerIndex)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdatedShuffledCards_RPC", RpcTarget.AllBufferedViaServer, CardManager.Instance.GetNewCardStatus(), playerIndex);
        }
    }

    [PunRPC]
    public void OtherPlayerSetting_RPC(int playerIndex, int playerImageIndex, string playerId)
    {
        if (playerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1) return;
        
        otherPlayers.OtherPlayerSetting(playerIndex, playerImageIndex, playerId);
    }
    
    [PunRPC]
    public void UpdatedOtherPlayerTurn_RPC(int playerIndex)
    {
        otherPlayers.UpdatedOtherPlayerTurn(playerIndex);
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

    [PunRPC]
    public void EndGame_RPC(int playerIndex, string winnerID)
    {
        if (playerIndex == PhotonNetwork.LocalPlayer.ActorNumber - 1)
            winnerUI.SetWinnerMessage("");
        else
            winnerUI.SetWinnerMessage(winnerID);
        
        winnerUI.gameObject.SetActive(true);
    }

}

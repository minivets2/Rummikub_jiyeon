using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : Singleton<GameManager>
{
    [FormerlySerializedAs("players")] [SerializeField] private Player player;

    [Header("Init")]
    [SerializeField] private Transform playerPosition;

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cardManager;
    
    private PhotonView _photonView;
    private int _startPlayerIndex;
    
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
            _startPlayerIndex = new Random().Next(0, PhotonNetwork.PlayerList.Length);
            
            for (int j = 0; j < 12; j++)
            {
                _photonView.RPC("UpdatedShuffledCards_RPC", RpcTarget.AllBufferedViaServer, CardManager.Instance.NewCard());
            }

            _photonView.RPC("StartTurn", RpcTarget.AllBufferedViaServer, _startPlayerIndex);
            _photonView.RPC("EndTurn", RpcTarget.AllBufferedViaServer, _startPlayerIndex);
        }
    }

    [PunRPC]
    public void StartTurn(int index)
    {
        if (index == PhotonNetwork.LocalPlayer.ActorNumber - 1) player.StartTurn();
    }
    
    [PunRPC]
    public void EndTurn(int index)
    {
        if (index != PhotonNetwork.LocalPlayer.ActorNumber - 1) player.EndTurn();
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
        this.player = player.GetComponent<Player>();
    }
    
}

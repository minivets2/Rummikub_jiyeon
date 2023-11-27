using Photon.Pun;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cardManager;
    [SerializeField] private GameObject gameReady;

    private void Start()
    {
        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            //공유플레이스 생성
            PhotonNetwork.Instantiate(cardManager.name, Vector3.zero, Quaternion.identity);
            var gameReadyObj = PhotonNetwork.Instantiate(gameReady.name, Vector3.zero, Quaternion.identity);
            gameReadyObj.GetComponent<GameReady>().InitGamePlayersCount(RoomManager.Instance.playersCount);
        }
    }

    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        var player = Instantiate(playerPrefab, playerPosition);
        player.GetComponent<Player>().InitPlayerInfo();
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

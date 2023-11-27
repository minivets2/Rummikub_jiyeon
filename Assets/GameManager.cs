using Photon.Pun;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform cardManagerPosition;
    [SerializeField] private GameObject cardManager;

    private void Start()
    {
        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            //공유플레이스 생성
            PhotonNetwork.Instantiate(cardManager.name, cardManagerPosition.position, cardManagerPosition.rotation);
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

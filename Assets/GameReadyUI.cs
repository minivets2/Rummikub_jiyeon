using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameReadyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerCount;
    public delegate void OnGameStartEvent();
    public static OnGameStartEvent onGameStartEvent;
    
    public void Update()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                           PhotonNetwork.CurrentRoom.MaxPlayers;
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            onGameStartEvent?.Invoke();
            Debug.Log("참석완료");
        }
    }
}

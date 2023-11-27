using Photon.Pun;
using UnityEngine;

public class GameReady : Singleton<GameReady>
{
    [SerializeField] private int count;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void InitGamePlayersCount(int count)
    {
        this.count = count;
    }
    
    public void SetReadyCount()
    {
        count--;

        if (count == 0)
        {
            FindObjectOfType<GameReadyUI>().gameObject.SetActive(false);
        }
    }

}

using Photon.Pun;
using UnityEngine;

public class GameReady : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private int count;

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}

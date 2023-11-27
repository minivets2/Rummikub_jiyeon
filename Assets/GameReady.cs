using System;
using Photon.Pun;
using UnityEngine;

public class GameReady : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private int count;
    [SerializeField] private PhotonView pv;

    public void InitGamePlayersCount(int count)
    {
        this.count = count;
    }
    
    [PunRPC]
    public void SetReadyCount()
    {
        if (!pv.IsMine) return;
        
        count--;

        if (count == 0)
        {
            FindObjectOfType<GameReadyUI>().gameObject.SetActive(false);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(count);
        }
        else
        {
            count = (int)stream.ReceiveNext();
        }
    }
}

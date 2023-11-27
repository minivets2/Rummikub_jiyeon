using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameReady : Singleton<GameReady>
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
            Debug.LogError("ddd");
        }
    }

}

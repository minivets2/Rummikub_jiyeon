using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public string playerId;

    public void SetPlayerId(string playerId)
    {
        this.playerId = playerId;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public string playerId;
    public Sprite playerImage;
    public int playersCount;

    public void SetPlayerInfomation(string playerId, Sprite playerImage, int playersCount)
    {
        this.playerId = playerId;
        this.playerImage = playerImage;
        this.playersCount = playersCount;
    }
}

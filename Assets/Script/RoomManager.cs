using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public string playerId;
    public Sprite playerImage;
    public int playersCount;
    public int playerImageIndex;

    public void SetPlayerInfomation(string playerId, Sprite playerImage, int playersCount, int playerImageIndex)
    {
        this.playerId = playerId;
        this.playerImage = playerImage;
        this.playersCount = playersCount;
        this.playerImageIndex = playerImageIndex;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayers : MonoBehaviour
{
    [SerializeField] private List<OtherPlayer> otherPlayers;

    public void OtherPlayerSetting(int playerIndex, Sprite playerImage, string playerId)
    {
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            if (playerIndex == otherPlayers[i].Index)
            {
                otherPlayers[i].gameObject.SetActive(true);
                otherPlayers[i].SetProfile(playerImage, playerId);
            }
        }
    }
}

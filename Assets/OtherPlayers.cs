using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayers : MonoBehaviour
{
    [SerializeField] private List<OtherPlayer> otherPlayers;
    [SerializeField] private List<Sprite> playerImages;

    public void OtherPlayerSetting(int playerIndex, int playerImageIndex, string playerId)
    {
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            if (playerIndex == otherPlayers[i].Index)
            {
                otherPlayers[i].gameObject.SetActive(true);
                otherPlayers[i].SetProfile(playerImages[playerImageIndex], playerId);
            }
        }
    }

    public void UpdatedOtherPlayerTurn(int playerIndex)
    {
        for (int i = 0; i < otherPlayers.Count; i++)
        {
            if (playerIndex == otherPlayers[i].Index)
                otherPlayers[i].StartTurn();
            else
                otherPlayers[i].EndTurn();
        }
    }
}

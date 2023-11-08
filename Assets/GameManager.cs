using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player[] players;

    public void Init()
    {
        players[0].StartTurn();
        PlaceManager.Instance.SaveCardList();

        for (int i = 0; i < 12; i++)
        {
            CardManager.Instance.NewCardButtonClick();
        }
    }
}

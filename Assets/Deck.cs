using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class Deck : MonoBehaviour
{
    private List<CardInfo> card = new List<CardInfo>();

    public void NumberSortButtonClick()
    {
        SetSlotList();

        List<CardInfo> sorted = card.OrderBy(x => x.color)
            .ThenBy(x => x.number)
            .ToList();

        ResetChildCard(sorted);
    }

    public void ColorSortButtonClick()
    {
        SetSlotList();

        List<CardInfo> sorted = card.OrderBy(x => x.number)
            .ThenBy(x => x.color)
            .ToList();

        ResetChildCard(sorted);
    }

    private void SetSlotList()
    {
        GameManager gameManager = GameManager.Instance;
        
        card.Clear();

        foreach (var slot in gameManager.DeckSlot)
        {
            Vector2 _cardInfo = slot.CardInfo();
            CardInfo cardInfo = new CardInfo((int)_cardInfo.X, (int)_cardInfo.Y);

            if (cardInfo.number != 100)
            {
                card.Add(cardInfo);
            }
        }
    }

    private void ResetChildCard(List<CardInfo> sorted)
    {
        GameManager gameManager = GameManager.Instance;
        
        gameManager.ResetCard(sorted);
    }
}

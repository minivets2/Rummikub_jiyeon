using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Slot[] slot;
    [SerializeField] private Card cardPrefab;

    public void ResetCard(List<CardInfo> sorted)
    {
        for (int i = 0; i < sorted.Count; i++)
        {
            var card = Instantiate(cardPrefab, slot[i].transform);
            int cardNumber = sorted[i].number;
            int cardColor = sorted[i].color;
            card.SetCardStatus(cardNumber, cardColor);
            card.transform.localScale = Vector3.one;
        }
    }
    
    public void CreateCard()
    {
        CardManager cardManager = CardManager.Instance;
        
        int index = 0;
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].transform.childCount == 0)
            {
                index = i;
                break;
            }
        }
        
        var card = Instantiate(cardPrefab, slot[index].transform);
        string cardStatus = cardManager.GetRandomCardStatus();
        
        string number = cardStatus.Substring(1, cardStatus.Length - 1);
        string color = cardStatus.Substring(0, 1);
        
        card.SetCardStatus(int.Parse(number), color);
        
        card.transform.localScale = Vector3.one;
    }
}

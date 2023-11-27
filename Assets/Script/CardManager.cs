using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;
using Vector2 = System.Numerics.Vector2;

public class CardManager : Singleton<CardManager>
{
    [Header("Card Prefab")]
    [SerializeField] private Card cardPrefab;
    
    private Dictionary<string, int> Card = new Dictionary<string, int>()
    {
        {"B1", 2}, {"B2", 2},{"B3", 2},{"B4", 2},{"B5", 2},{"B6", 2},
        {"B7", 2}, {"B8", 2},{"B9", 2},{"B10", 2},{"B11", 2},{"B12", 2},{"B13", 2},
        {"R1", 2}, {"R2", 2},{"R3", 2},{"R4", 2},{"R5", 2},{"R6", 2},
        {"R7", 2}, {"R8", 2},{"R9", 2},{"R10", 2},{"R11", 2},{"R12", 2},{"R13", 2},
        {"O1", 2}, {"O2", 2},{"O3", 2},{"O4", 2},{"O5", 2},{"O6", 2},
        {"O7", 2}, {"O8", 2},{"O9", 2},{"O10", 2},{"O11", 2},{"O12", 2},{"O13", 2},
        {"K1", 2}, {"K2", 2},{"K3", 2},{"K4", 2},{"K5", 2},{"K6", 2},
        {"K7", 2}, {"K8", 2},{"K9", 2},{"K10", 2},{"K11", 2},{"K12", 2},{"K13", 2},
        {"k20", 1}, {"R20", 1}
    };

    private int cardCount = 106;
    
    public void NewCardButtonClick()
    {
        Vector2 vector2;

        while (true)
        {
            vector2 = PlaceManager.Instance.FindEmptyPlayerSlotIndex();
            
            if ((int)vector2.X == 100)
            {
                PlaceManager.Instance.PlayerPlaceExpansion();
            }
            else
            {
                break;
            }
        }
        
        string cardStatus = GetRandomCardStatus();

        if (cardStatus == "")
        {
            //뽑을 카드가 없을때
            return;
        }
        
        var card = Instantiate(cardPrefab, PlaceManager.Instance.PlayerSlots[(int)vector2.X][(int)vector2.Y].transform);
     
        string number = cardStatus.Substring(1, cardStatus.Length - 1);
        string color = cardStatus.Substring(0, 1);
        CardColorType colorType = CardColorType.Black;
        
        switch (color)
        {
            case "B":
                colorType = CardColorType.Blue;
                break;
            case "R":
                colorType = CardColorType.Red;
                break;
            case "O":
                colorType = CardColorType.Orange;
                break;
            case "K":
                colorType = CardColorType.Black;
                break;
        }
        
        card.SetCardStatus(int.Parse(number), colorType);
        card.transform.localScale = Vector3.one;
    }

    private string GetRandomCardStatus()
    {
        string randomKey = "";

        while (cardCount > 0)
        {
            int randomIndex = new Random().Next(0, Card.Count);

            if (Card.Values.ElementAt(randomIndex) > 0)
            {
                randomKey = Card.Keys.ElementAt(randomIndex);
                Card[randomKey]--;
                cardCount--;
                break;
            }
        }

        return randomKey;
    }

}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Vector2 = System.Numerics.Vector2;

public class MyStructComparer : IEqualityComparer<CardInfo>
{
    public bool Equals(CardInfo x, CardInfo y)
    {
        return (x.number == y.number) && (x.color == y.color);
    }

    public int GetHashCode(CardInfo obj)
    {
        return obj.number.GetHashCode();
    }
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Slot[] playGroundSlot;
    [SerializeField] private Slot[] deckSlot;
    [SerializeField] private Card cardPrefab;

    private List<CardInfo> _previousCardList = new List<CardInfo>();
    private List<CardInfo> _newCardList = new List<CardInfo>();

    public Slot[] DeckSlot => deckSlot;
    
    private bool _isResetCard = false;

    public void SaveCardList()
    {
        _previousCardList.Clear();

        foreach (var slot in playGroundSlot)
        {
            Vector2 _cardInfo = slot.CardInfo();
            CardInfo cardInfo = new CardInfo((int)_cardInfo.X, (int)_cardInfo.Y);

            _previousCardList.Add(cardInfo);
        }
    }

    public void ResetCardList()
    {
        _newCardList.Clear();

        foreach (var slot in playGroundSlot)
        {
            Vector2 _cardInfo = slot.CardInfo();
            CardInfo cardInfo = new CardInfo((int)_cardInfo.X, (int)_cardInfo.Y);

            _newCardList.Add(cardInfo);
        }

        for (int i = 0; i < _previousCardList.Count; i++)
        {
            if (_previousCardList[i] == _newCardList[i])
            {
                continue;
            }

            playGroundSlot[i].DestroyChildCard();

            if (_previousCardList[i].number == 100)
            {
                continue;
            }
            
            var card = Instantiate(cardPrefab, playGroundSlot[i].transform);
            int cardNumber = _previousCardList[i].number;
            int cardColor = _previousCardList[i].color;
            string color = "";
            
            switch (cardColor)
            {
                case 0:
                    color = "B";
                    break;
                case 1:
                    color = "R";
                    break;
                case 2:
                    color = "O";
                    break;
                case 3:
                    color = "K";
                    break;
            }
            
            card.SetCardStatus(cardNumber, color);
            card.transform.localScale = Vector3.one;
        }

        int previousCount = 0;
        int newsCount = 0;

        for (int i = 0; i < _previousCardList.Count; i++)
        {
            if (_previousCardList[i].number != 100)
            {
                previousCount++;
            }
            
            if (_newCardList[i].number != 100)
            {
                newsCount++;
            }
        }

        if (previousCount == newsCount)
        {
            return;
        }

        MyStructComparer comparer = new MyStructComparer();

        // 차집합을 계산
        List<CardInfo> difference = _newCardList.Except(_previousCardList, comparer).ToList();

        foreach (var item in difference)
        {
            int index = 0;
            for (int i = 0; i < deckSlot.Length; i++)
            {
                if (deckSlot[i].transform.childCount == 0)
                {
                    index = i;
                    break;
                }
            }
        
            var card = Instantiate(cardPrefab, deckSlot[index].transform);
            int cardNumber = item.number;
            int cardColor = item.color;
            string color = "";
            
            switch (cardColor)
            {
                case 0:
                    color = "B";
                    break;
                case 1:
                    color = "R";
                    break;
                case 2:
                    color = "O";
                    break;
                case 3:
                    color = "K";
                    break;
            }
            
            card.SetCardStatus(cardNumber, color);
            card.transform.localScale = Vector3.one;
        }
    }

    public void CheckOverlap()
    {
        CheckOverlap(playGroundSlot);
        CheckOverlap(deckSlot);
    }

    private void CheckOverlap(Slot[] slot)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].transform.childCount > 1)
            {
                Transform child = slot[i].transform.GetChild(0);
            
                if (child != null)
                {
                    if (i == slot.Length - 1)
                    {
                        child.SetParent(slot[0].transform);
                    }
                    else
                    {
                        child.SetParent(slot[i+1].transform);
                    }
                    
                    child.localPosition = Vector3.zero;
                }
            }
        }
    }

    public void ResetCard(List<CardInfo> sorted)
    {
        foreach (var slot in deckSlot)
        {
            slot.DestroyChildCard();
        }
        
        for (int i = 0; i < sorted.Count; i++)
        {
            var card = Instantiate(cardPrefab, deckSlot[i].transform);
            int cardNumber = sorted[i].number;
            int cardColor = sorted[i].color;
            string color = "";
            
            switch (cardColor)
            {
                case 0:
                    color = "B";
                    break;
                case 1:
                    color = "R";
                    break;
                case 2:
                    color = "O";
                    break;
                case 3:
                    color = "K";
                    break;
            }
            
            card.SetCardStatus(cardNumber, color);
            card.transform.localScale = Vector3.one;
        }
    }
    
    public void NewCardButtonClick()
    {
        CardManager cardManager = CardManager.Instance;
        
        int index = 0;
        for (int i = 0; i < deckSlot.Length; i++)
        {
            if (deckSlot[i].transform.childCount == 0)
            {
                index = i;
                break;
            }
        }
        
        var card = Instantiate(cardPrefab, deckSlot[index].transform);
        string cardStatus = cardManager.GetRandomCardStatus();
        
        string number = cardStatus.Substring(1, cardStatus.Length - 1);
        string color = cardStatus.Substring(0, 1);
        
        card.SetCardStatus(int.Parse(number), color);
        
        card.transform.localScale = Vector3.one;
    }
}


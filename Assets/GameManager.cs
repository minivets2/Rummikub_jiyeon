using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Vector2 = System.Numerics.Vector2;

public class MyClassComparer : IEqualityComparer<Card>
{
    public bool Equals(Card x, Card y)
    {
        return x.CardGuid == y.CardGuid;
    }

    public int GetHashCode(Card obj)
    {
        return obj.CardGuid.GetHashCode();
    }
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Slot[] playGroundSlot;
    [SerializeField] private Slot[] deckSlot;
    [SerializeField] private Card cardPrefab;

    [SerializeField] private List<GameObject> _previousPlayGround = new List<GameObject>();
    
    [SerializeField] private List<Card> _previousCardList = new List<Card>();
    [SerializeField] private List<Card> _newCardList = new List<Card>();
    [SerializeField] private List<Card> difference = new List<Card>();

    public Slot[] PlayGroundSlot => playGroundSlot;
    public Slot[] DeckSlot => deckSlot;
    
    private bool _isResetCard = false;

    public void SaveCardList()
    {
        _previousPlayGround.Clear();
        _previousCardList.Clear();

        foreach (var slot in playGroundSlot)
        {
            if (slot.transform.childCount > 0)
            {
                Transform child = slot.transform.GetChild(0);

                if (child != null)
                {
                    _previousPlayGround.Add(child.gameObject);
                    _previousCardList.Add(child.gameObject.GetComponent<Card>());
                }
            }
            else
            {
                _previousPlayGround.Add(null);
            }
        }
    }

    public void ResetCardList()
    {
        _newCardList.Clear();

        foreach (var slot in playGroundSlot)
        {
            if (slot.transform.childCount > 0)
            {
                Transform child = slot.transform.GetChild(0);

                if (child != null)
                {
                    _newCardList.Add(child.gameObject.GetComponent<Card>());
                }
            }
        }

        MyClassComparer comparer = new MyClassComparer();
        difference = _newCardList.Except(_previousCardList, comparer).ToList();

        for (int i = 0; i < difference.Count; i++)
        {
            int index = 0;
            for (int j = 0; j < deckSlot.Length; j++)
            {
                if (deckSlot[j].transform.childCount == 0)
                {
                    index = j;
                    break;
                }
            }
            
            difference[i].transform.SetParent(deckSlot[index].transform);
            difference[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
        }

        for (int i = 0; i < _previousPlayGround.Count; i++)
        {
            if (_previousPlayGround[i] != null)
            {
                _previousPlayGround[i].transform.SetParent(playGroundSlot[i].transform);
                _previousPlayGround[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
            }
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
    
    public void ResetCard(List<GameObject> sorted)
    {
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].transform.SetParent(deckSlot[i].transform);
            sorted[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
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
}


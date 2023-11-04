using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class CardInfo
{
    public int number;
    public int color;
 
    public CardInfo(int number, int color)
    {
        this.number = number;
        this.color = color;
    }
}

public class Slot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var otherCardTransform = eventData.pointerDrag.transform;
        otherCardTransform.SetParent(transform);
        otherCardTransform.localPosition = Vector3.zero;
    }

    public void DestroyChildCard()
    {
        Transform childTransform = transform.Find("Card(Clone)");

        if (childTransform != null)
        {
            Destroy(childTransform.gameObject);
        }
    }

    public CardInfo CardInfo()
    {
        Transform childTransform = transform.Find("Card(Clone)");

        if (childTransform != null)
        {
            Card card = childTransform.GetComponent<Card>();

            if (card != null)
            {
                CardInfo cardInfo = new CardInfo(card.Number, (int)card.ColorType);
                return cardInfo;
            }
        }
        
        CardInfo nullInfo = new CardInfo(0, 0);
        return nullInfo;
    }
}

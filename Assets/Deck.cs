using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Deck : MonoBehaviour
{
    [SerializeField] private Slot[] slots;

    private List<CardInfo> card = new List<CardInfo>();

    public void NumberSortButtonClick()
    {
        SetSlotList();

        List<CardInfo> sorted = card.OrderBy(x => x.color)
            .ThenBy(x => x.number)
            .ToList();

        DestroyChildCard();
        ResetChildCard(sorted);
    }

    public void ColorSortButtonClick()
    {
        SetSlotList();

        List<CardInfo> sorted = card.OrderBy(x => x.number)
            .ThenBy(x => x.color)
            .ToList();

        DestroyChildCard();
        ResetChildCard(sorted);
    }

    private void SetSlotList()
    {
        card.Clear();

        foreach (var slot in slots)
        {
            CardInfo cardInfo = slot.CardInfo();

            if (cardInfo.number != 0)
            {
                card.Add(cardInfo);
            }
        }
    }

    private void DestroyChildCard()
    {
        foreach (var slot in slots)
        {
            slot.DestroyChildCard();
        }
    }

    private void ResetChildCard(List<CardInfo> sorted)
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetCard(sorted);
    }
}

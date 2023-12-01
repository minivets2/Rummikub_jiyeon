using System;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;

public enum CardColorType
{
    Blue,
    Red,
    Orange,
    Black
}

public class Card : MonoBehaviour
{
    [Header("card info")]
    [SerializeField][ReadOnly] private int number;
    [SerializeField][ReadOnly] private CardColorType colorType;
    [SerializeField] [ReadOnly] private string status;
    [SerializeField] private bool moveComplete;
    [SerializeField][ReadOnly] private string cardGuid;
    
    [Header("card ui")]
    [SerializeField] private TMP_Text textNumber;
    [SerializeField] private Image image;

    [Header("Joker")]
    [SerializeField] private Sprite jokerSprite_black;
    [SerializeField] private Sprite jokerSprite_red;
    
    public int Number => number;
    public CardColorType ColorType => colorType;
    public string Status => status;
    public bool MoveComplete => moveComplete;
    public string CardGuid => cardGuid;

    private void Start()
    {
        cardGuid = string.Empty;
        cardGuid = Guid.NewGuid().ToString();
    }

    public void SetCardStatus(string cardStatus, int cardNumber, CardColorType cardColor)
    {
        textNumber.text = cardNumber.ToString();
        textNumber.color = SetCardColor(cardColor, ref colorType);
        number = cardNumber;
        status = cardStatus;
        moveComplete = false;
        image.gameObject.SetActive(false);

        if (cardNumber == 20 && colorType == CardColorType.Black)
        {
            image.gameObject.SetActive(true);
            image.sprite = jokerSprite_black;
        }
        else if (cardNumber == 20 && colorType == CardColorType.Red)
        {
            image.gameObject.SetActive(true);
            image.sprite = jokerSprite_red;
        }
    }

    private Color SetCardColor(CardColorType cardColor, ref CardColorType colorType)
    {
        Color newColor = Color.clear;
        colorType = cardColor;
        
        switch (colorType)
        {
            case CardColorType.Blue:
                newColor = Color.blue;
                break;
            case CardColorType.Red:
                newColor = Color.red;
                break;
            case CardColorType.Orange:
                newColor = new Color(1.0f, 0.647f, 0.0f);
                break;
            case CardColorType.Black:
                newColor = Color.black;
                break;
        }

        return newColor;
    }

    public void SetMoveComplete(bool value)
    {
        moveComplete = value;
    }

    public Vector2 GetSlotRowColumn()
    {
        int row = 0;
        int column = 0;
        
        if (transform.parent.GetComponent<Slot>())
        {
            var parent = transform.parent;
            row = parent.GetComponent<Slot>().Row;
            column = parent.GetComponent<Slot>().Column;
        }

        return new Vector2(row, column);
    }

    public bool ComparerSameSlot(Card comparerCard)
    {
        if (GetSlotRowColumn() == comparerCard.GetSlotRowColumn())
            return true;

        return false;
    }
}

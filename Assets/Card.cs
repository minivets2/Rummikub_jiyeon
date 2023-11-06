using System;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum CardColorType
{
    Blue,
    Red,
    Orange,
    Black
}

public class Card : MonoBehaviour
{
    [SerializeField][ReadOnly] private int number;
    [SerializeField][ReadOnly] private CardColorType colorType;
    [SerializeField] private TMP_Text textNumber;
    [SerializeField][ReadOnly] private string cardGuid;
    
    private Sprite _resourceImage;
    
    public int Number => number;
    public CardColorType ColorType => colorType;
    public string CardGuid => cardGuid;

    private void Start()
    {
        cardGuid = string.Empty;
        cardGuid = Guid.NewGuid().ToString();
    }

    public void SetCardStatus(int cardNumber, CardColorType cardColor)
    {
        textNumber.text = cardNumber.ToString();
        textNumber.color = SetCardColor(cardColor, ref colorType);
        number = cardNumber;
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
}

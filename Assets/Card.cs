using TMPro;
using Unity.Collections;
using UnityEngine;

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
    
    private Sprite _resourceImage;
    
    public int Number => number;
    public CardColorType ColorType => colorType;

    public void SetCardStatus(int cardNumber, string cardColor)
    {
        textNumber.text = cardNumber.ToString();
        textNumber.color = SetCardColor(cardColor, ref colorType);
        number = cardNumber;
    }

    private Color SetCardColor(string cardColor, ref CardColorType colorType)
    {
        Color newColor = Color.clear;
        colorType = CardColorType.Black;
        
        switch (cardColor)
        {
            case "B":
                newColor = Color.blue;
                colorType = CardColorType.Blue;
                break;
            case "R":
                newColor = Color.red;
                colorType = CardColorType.Red;
                break;
            case "O":
                newColor = new Color(1.0f, 0.647f, 0.0f);
                colorType = CardColorType.Orange;
                break;
            case "K":
                newColor = Color.black;
                colorType = CardColorType.Black;
                break;
        }

        return newColor;
    }
}

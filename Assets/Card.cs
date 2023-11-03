using TMPro;
using Unity.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text textNumber;
    [SerializeField][ReadOnly] private int number;
    [SerializeField][ReadOnly] private Color color;
    private Sprite _resourceImage;

    public void SetCardStatus(int cardNumber, string cardColor)
    {
        textNumber.text = cardNumber.ToString();
        textNumber.color = SetCardColor(cardColor);
        number = cardNumber;
        color = textNumber.color;
    }

    private Color SetCardColor(string cardColor)
    {
        Color color = Color.clear;
        
        switch (cardColor)
        {
            case "B":
                color = Color.blue;
                break;
            case "R":
                color = Color.red;
                break;
            case "O":
                color = new Color(1.0f, 0.647f, 0.0f);
                break;
            case "K":
                color = Color.black;
                break;
        }

        return color;
    }
}

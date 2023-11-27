using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    public bool CheckContainMatchCardList(List<List<GameObject>> matchCards, GameObject card)
    {
        for (int i = 0; i < matchCards.Count; i++)
        {
            if (matchCards[i].Contains(card))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool AllNumbersEqual(List<Card> list)
    {
        int firstValue = 0;
        int index = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Number != 20)
            {
                firstValue = list[i].Number;
                index = i;
                break;
            }
        }

        for (int i = index + 1; i < list.Count; i++)
        {
            if (list[i].Number == 20)
            {
                continue;
            }
            
            if (list[i].Number != firstValue)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool AllColorsEqual(List<Card> list)
    {
        CardColorType firstValue = list[0].ColorType;

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].ColorType != firstValue)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool AllColorsDifferent(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[i].ColorType == list[j].ColorType)
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    public bool AreConsecutive(List<Card> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].Number != list[i - 1].Number + 1)
            {
                return false;
            }
        }

        return true;
    }
}

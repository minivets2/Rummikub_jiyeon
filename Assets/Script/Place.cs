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
            if (list[i].Number == 20) continue;
            
            firstValue = list[i].Number;
            index = i;
            break;
        }

        for (int i = index + 1; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;

            if (list[i].Number != firstValue)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool AllColorsEqual(List<Card> list)
    {
        CardColorType firstColors = list[0].ColorType;
        int index = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;
            
            firstColors = list[i].ColorType;
            index = i;
            break;
        }

        for (int i = index + 1; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;
            
            if (list[i].ColorType != firstColors)
            {
                return false;
            }
        }

        return true;
    }
    
    public bool AllColorsDifferent(List<Card> list)
    {
        List<CardColorType> colors = new List<CardColorType>();
        int index = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;
            
            colors.Add(list[i].ColorType);
            index = i;
            break;
        }

        for (int i = index + 1; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;

            if (colors.Contains(list[i].ColorType))
                return false;
            
            colors.Add(list[i].ColorType);
        }

        return true;
    }
    
    public bool AreConsecutive(List<Card> list)
    {
        int number = 0;
        int index = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;
            
            number = list[i].Number;
            index = i;
            break;
        }
        
        for (int i = index + 1; i < list.Count; i++)
        {
            if (list[i].Number == 20) continue;
            
            if (list[i].Number != number + 1)
                return false;
            
            number = list[i].Number;
        }

        return true;
    }
}

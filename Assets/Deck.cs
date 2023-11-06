using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<GameObject> cards = new List<GameObject>();

    public void NumberSortButtonClick()
    {
        SetSlotList();
        
        List<GameObject> sorted = cards.OrderBy(x => x.GetComponent<Card>().ColorType)
            .ThenBy(x => x.GetComponent<Card>().Number)
            .ToList();
        
        ResetChildCard(sorted);
    }
    
    public void ColorSortButtonClick()
    {
        SetSlotList();
        
        List<GameObject> sorted = cards.OrderBy(x => x.GetComponent<Card>().Number)
            .ThenBy(x => x.GetComponent<Card>().ColorType)
            .ToList();
        
        ResetChildCard(sorted);
    }

    private void SetSlotList()
    {
        GameManager gameManager = GameManager.Instance;

        cards.Clear();

        foreach (var slot in gameManager.DeckSlot)
        {
            if (slot.transform.childCount > 0)
            {
                GameObject child = slot.transform.GetChild(0).gameObject;
                if (child != null) cards.Add(child);
            }
        }
    }

    private void ResetChildCard(List<GameObject> sorted)
    {
        GameManager gameManager = GameManager.Instance;
        
        gameManager.ResetCard(sorted);
    }
}


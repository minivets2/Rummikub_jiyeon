using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPlace : Place
{
    private readonly List<GameObject> _cards = new List<GameObject>();

    public void NumberSortButtonClick()
    {
        GetSlotList();
        
        List<GameObject> sorted = _cards.OrderBy(x => x.GetComponent<Card>().ColorType)
            .ThenBy(x => x.GetComponent<Card>().Number)
            .ToList();
        
        SortDeckSlot(sorted);
    }
    
    public void ColorSortButtonClick()
    {
        GetSlotList();
        
        List<GameObject> sorted = _cards.OrderBy(x => x.GetComponent<Card>().Number)
            .ThenBy(x => x.GetComponent<Card>().ColorType)
            .ToList();
        
        SortDeckSlot(sorted);
    }

    private void GetSlotList()
    {
        _cards.Clear();

        for (int i = 0; i < PlaceManager.Instance.PlayerSlots.Count; i++)
        {
            for (int j = 0; j < PlaceManager.Instance.PlayerSlots[i].Count; j++)
            {
                if (PlaceManager.Instance.PlayerSlots[i][j].transform.childCount > 0)
                {
                    GameObject child = PlaceManager.Instance.PlayerSlots[i][j].transform.GetChild(0).gameObject;
                    if (child != null) _cards.Add(child);
                }
            }
        }
    }
    
    private void SortDeckSlot(List<GameObject> sorted)
    {
        for (int i = 0; i < sorted.Count; i++)
        {
            if (i < PlaceManager.Instance.PlayerSlots[0].Count)
            {
                sorted[i].transform.SetParent(PlaceManager.Instance.PlayerSlots[0][i].transform);
                sorted[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                sorted[i].transform.SetParent(PlaceManager.Instance.PlayerSlots[1][i - PlaceManager.Instance.PlayerSlots[0].Count].transform);
                sorted[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;   
            }
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerPlace : Place
{
    [SerializeField] private List<GameObject> _cards = new List<GameObject>();
    private List<List<GameObject>> _matchCards = new List<List<GameObject>>();

    public void NumberSortButtonClick()
    {
        GetSlotList();
        
        List<GameObject> sorted = _cards.OrderBy(x => x.GetComponent<Card>().ColorType)
            .ThenBy(x => x.GetComponent<Card>().Number)
            .ToList();

        CheckMatchBlocks(sorted, true);
        CheckOtherBlocks(sorted);

        SortPlayerPlace(sorted);
    }
    
    public void ColorSortButtonClick()
    {
        GetSlotList();
        
        List<GameObject> sorted = _cards.OrderBy(x => x.GetComponent<Card>().Number)
            .ThenBy(x => x.GetComponent<Card>().ColorType)
            .ToList();
        
        CheckMatchBlocks(sorted, false);
        CheckOtherBlocks(sorted);
        
        SortPlayerPlace(sorted);
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

    private void CheckMatchBlocks(List<GameObject> sorted, bool isNumberSort)
    {
        _matchCards.Clear();
        
        int index = 0;
        while (true)
        {
            if (index + 1 >= sorted.Count) break;
            
            List<GameObject> element = new List<GameObject>();
            
            if (!CheckContainMatchCardList(_matchCards, sorted[index])) element.Add(sorted[index]);
            else
            {
                index++;
                continue;
            }

            for (int i = index + 1; i < sorted.Count; i++)
            {
                if (CheckContainMatchCardList(_matchCards, sorted[i])) continue;
                    
                if (element[^1].GetComponent<Card>().ColorType == sorted[i].GetComponent<Card>().ColorType &&
                    element[^1].GetComponent<Card>().Number == sorted[i].GetComponent<Card>().Number) continue;
                
                if (isNumberSort)
                {
                    if (CompareNumber(sorted, index, i , element)) break;
                }
                else
                {
                    if (CompareColor(sorted, index, i)) break;
                }

                element.Add(sorted[i]);
            }

            if (element.Count > 2) _matchCards.Add(element);

            index++;
        }
    }
    
    private bool CompareNumber(List<GameObject> sorted, int index, int i, List<GameObject> element)
    {
        if (sorted[index].GetComponent<Card>().ColorType != sorted[i].GetComponent<Card>().ColorType ||
            element[^1].GetComponent<Card>().Number + 1 != sorted[i].GetComponent<Card>().Number) return true; 
        
        return false;
    }

    private bool CompareColor(List<GameObject> sorted, int index, int i)
    {
        if (sorted[index].GetComponent<Card>().Number != sorted[i].GetComponent<Card>().Number) return true; 
        
        return false;
    }

    private void CheckOtherBlocks(List<GameObject> sorted)
    {
        if (_matchCards.Count == 0)
        {
            _matchCards.Add(sorted);
            return;
        }
        
        List<GameObject> element = new List<GameObject>();
        List<GameObject> joker = new List<GameObject>();
        for (int i = 0; i < sorted.Count; i++)
        {
            if (sorted[i].GetComponent<Card>().Number == 20)
            {
                joker.Add(sorted[i]);
                continue;
            }
            
            int count = 0;
            for (int j = 0; j < _matchCards.Count; j++)
            {
                if (_matchCards[j].Contains(sorted[i])) count++;
            }
            
            if (count == 0) element.Add(sorted[i]);
        }

        if (joker.Count > 0)
        {
            for (int i = 0; i < joker.Count; i++) element.Add(joker[i]);
        }

        _matchCards.Add(element);
    }
    
    private void SortPlayerPlace(List<GameObject> sorted)
    {
        int count = 0;
        for (int i = 0; i < _matchCards.Count; i++)
        {
            for (int j = 0; j < _matchCards[i].Count; j++)
            {
                count++;
            }
            
            count++;
        }

        if (count % 2 != 0)
        {
            count++;
        }

        for (int i = 0; i < (count - PlaceManager.Instance.PlayerSlots[0].Count * 2) / 2; i++)
        {
            PlaceManager.Instance.PlayerPlaceExpansion();
        }

        while (true)
        {
            if (count - PlaceManager.Instance.PlayerSlots[0].Count * 2 <= 0)
                break;
            
            PlaceManager.Instance.PlayerPlaceExpansion();
        }
        
        int index = 0;
        for (int i = 0; i < _matchCards.Count; i++)
        {
            for (int j = 0; j < _matchCards[i].Count; j++)
            {
                if (index < PlaceManager.Instance.PlayerSlots[0].Count)
                {
                    _matchCards[i][j].transform.SetParent(PlaceManager.Instance.PlayerSlots[0][index].transform);
                    _matchCards[i][j].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }
                else
                {
                    _matchCards[i][j].transform.SetParent(PlaceManager.Instance.PlayerSlots[1][index - PlaceManager.Instance.PlayerSlots[0].Count].transform);
                    _matchCards[i][j].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;   
                }

                index++;
            }

            index++;
        }
    }
}


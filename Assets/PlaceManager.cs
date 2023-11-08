using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Vector2 = System.Numerics.Vector2;

public class CardComparer : IEqualityComparer<Card>
{
    public bool Equals(Card x, Card y)
    {
        return x.CardGuid == y.CardGuid;
    }

    public int GetHashCode(Card obj)
    {
        return obj.CardGuid.GetHashCode();
    }
}

public class PlaceManager : Singleton<PlaceManager>
{
    [Header("Place")]
    [SerializeField] private PlayerPlace playerPlace;
    [SerializeField] private SharePlace sharePlace;
    
    [Header("Slots")]
    [SerializeField] private Slot[] sharedSlots;
    [SerializeField] private List<Slot> playerSlots_Line0;
    [SerializeField] private List<Slot> playerSlots_Line1;
    private List<List<Slot>> _playerSlots = new List<List<Slot>>();
    
    [Header("Prefab")]
    [SerializeField] private Slot slotPrefab;
    
    private List<GameObject> _previousPlayGround = new List<GameObject>();
    private List<Card> _previousCardList = new List<Card>();
    private List<Card> _newCardList = new List<Card>();
    private List<Card> difference = new List<Card>();

    public SharePlace SharePlace => sharePlace;
    public Slot[] SharedSlots => sharedSlots;
    public List<List<Slot>> PlayerSlots => _playerSlots;

    private void Start()
    {
        _playerSlots.Add(playerSlots_Line0);
        _playerSlots.Add(playerSlots_Line1);
    }

    public void SaveCardList()
    {
        _previousPlayGround.Clear();
        _previousCardList.Clear();

        foreach (var slot in sharedSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Transform child = slot.transform.GetChild(0);

                if (child != null)
                {
                    _previousPlayGround.Add(child.gameObject);
                    _previousCardList.Add(child.gameObject.GetComponent<Card>());
                }
            }
            else
            {
                _previousPlayGround.Add(null);
            }
        }
    }

    public void ResetCardList()
    {
        _newCardList.Clear();

        foreach (var slot in sharedSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Transform child = slot.transform.GetChild(0);

                if (child != null)
                {
                    _newCardList.Add(child.gameObject.GetComponent<Card>());
                }
            }
        }

        CardComparer comparer = new CardComparer();
        difference = _newCardList.Except(_previousCardList, comparer).ToList();

        for (int i = 0; i < difference.Count; i++)
        {
            Vector2 vector2 = FindEmptyPlayerSlotIndex();
            
            difference[i].transform.SetParent(_playerSlots[(int)vector2.X][(int)vector2.Y].transform);
            difference[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
        }

        for (int i = 0; i < _previousPlayGround.Count; i++)
        {
            if (_previousPlayGround[i] != null)
            {
                _previousPlayGround[i].transform.SetParent(sharedSlots[i].transform);
                _previousPlayGround[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
            }
        }
        
    }

    public void CheckOverlap()
    {
        //CheckOverlap(sharedSlots);
        CheckOverlap(_playerSlots);
    }

    private void CheckOverlap(List<List<Slot>> slot)
    {
        for (int i = 0; i < slot.Count; i++)
        {
            for (int j = 0; j < slot[i].Count; j++)
            {
                if (slot[i][j].transform.childCount > 1)
                {
                    Transform child = slot[i][j].transform.GetChild(0);
            
                    if (child != null)
                    {
                        if (j == slot[i].Count - 1)
                        {
                            child.SetParent(slot[0][0].transform);
                        }
                        else
                        {
                            child.SetParent(slot[i][j+1].transform);
                        }
                    
                        child.localPosition = Vector3.zero;
                    }
                }   
            }
        }
    }

    public Vector2 FindEmptyPlayerSlotIndex()
    {
        int index1 = 0;
        int index2 = 0;

        for (int j = 0; j < _playerSlots.Count; j++)
        {
            for (int k = 0; k < _playerSlots[j].Count; k++)
            {
                if (_playerSlots[j][k].transform.childCount == 0)
                {
                    index1 = j;
                    index2 = k;
                    return new Vector2(index1, index2);
                }   
            }
        }
        
        return new Vector2(index1, index2);
    }
}


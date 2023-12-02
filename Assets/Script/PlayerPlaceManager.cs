using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
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

public class PlayerPlaceManager : Singleton<PlayerPlaceManager>
{
    [Header("Player Place")]
    [SerializeField] private PlayerPlace playerPlace;
    private List<List<Slot>> _playerSlots = new List<List<Slot>>();

    [Header("Prefab")]
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Line PlayerPlaceLinePrefab;
    [SerializeField] private Line SharePlaceLinePrefab;

    private List<GameObject> _previousPlayGround = new List<GameObject>();
    private List<Card> _previousCardList = new List<Card>();
    private List<Card> _newCardList = new List<Card>();
    private List<Card> difference = new List<Card>();

    private int _addSlotCount = 0;
    
    public List<List<Slot>> PlayerSlots => _playerSlots;

    public void InitPlayerPlace(PlayerPlace playerPlace)
    {
        this.playerPlace = playerPlace;

        for (int i = 0; i < 2; i++)
        {
            var line = Instantiate(PlayerPlaceLinePrefab, playerPlace.transform);
            _playerSlots.Add(line.Slots);
        }
    }
    
    public void CheckPlaceSize()
    {
        if (playerPlace.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x <= 380) return;

        while (true)
        {
            int count = 0;
            for (int i = 0; i < _playerSlots.Count; i++)
            {
                if (_playerSlots[i][^1].transform.childCount == 0) 
                    count++;
            }

            if (count == 2)
            {
                for (int i = 0; i < _playerSlots.Count(); i++)
                {
                    UnityEngine.Vector2 size = playerPlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta;
                    size.x -= 38;
                    playerPlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = size;
                    
                    Destroy(_playerSlots[i][^1].gameObject);
                    _playerSlots[i].RemoveAt(_playerSlots[i].Count - 1);
                }
            }
            else break;
        }
        
        CheckPlaceImageSize();
    }

    public void CheckOverlap()
    {
        CheckOverlap(_playerSlots);
    }

    private void CheckOverlap(List<List<Slot>> slot)
    {
        while (true)
        {
            int count = 0;
            
            for (int i = 0; i < slot.Count; i++)
            {
                for (int j = 0; j < slot[i].Count; j++)
                {
                    if (slot[i][j].transform.childCount > 1)
                    {
                        Transform child = slot[i][j].transform.GetChild(0);
            
                        if (child != null)
                        {
                            if (i == 0 && j == slot[i].Count - 1) child.SetParent(slot[1][0].transform);
                            else if (i == 1 && j == slot[i].Count - 1)
                            {
                                Vector2 vector2 = FindEmptyPlayerSlotIndex();
                                child.SetParent(slot[(int)vector2.X][(int)vector2.Y].transform);
                            }
                            else child.SetParent(slot[i][j+1].transform);

                            child.localPosition = Vector3.zero;
                            count++;
                        }
                    } 
                }
            }

            if (count == 0) break;
        }
    }

    public void PlayerPlaceExpansion()
    {
        for (int i = 0; i < 2; i++)
        {
            UnityEngine.Vector2 size = playerPlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta;
            size.x += 38;
            playerPlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = size;
            
            Transform pos = _playerSlots[i][^1].transform;
            var slot = Instantiate(slotPrefab, playerPlace.transform.GetChild(i).GetComponent<RectTransform>().transform);
            slot.transform.localPosition = pos.localPosition + new Vector3(38, 0,0);
            
            _playerSlots[i].Add(slot);
        }

        CheckPlaceImageSize();
    }

    public Vector2 FindEmptyPlayerSlotIndex()
    {
        int index1 = 100;
        int index2 = 100;

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

    private void CheckPlaceImageSize()
    {
        if (playerPlace.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x > 570)
        {
            playerPlace.Image.enabled = true;
            
            for (int i = 0; i < 2; i++)
            {
                playerPlace.transform.GetChild(i).GetComponent<Image>().enabled = false;
                float x = playerPlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.x;
                playerPlace.transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.one;
                playerPlace.transform.GetChild(i).GetComponent<RectTransform>().localScale *=
                    570 / x;   
            }
        }
        else
        {
            playerPlace.Image.enabled = false;

            for (int i = 0; i < 2; i++)
            {
                playerPlace.transform.GetChild(i).GetComponent<Image>().enabled = true;
                playerPlace.transform.GetChild(i).GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    public int GetCardCount()
    {
        int cardCount = 0;
        
        for (int i = 0; i < _playerSlots.Count; i++)
        {
            for (int j = 0; j < _playerSlots[i].Count; j++)
            {
                if (_playerSlots[i][j].transform.childCount == 1)
                    cardCount++;
            }
        }

        return cardCount;
    }
}


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

public class PlaceManager : Singleton<PlaceManager>
{
    [Header("Share Place")]
    [SerializeField] private GameObject sharePlace;
    private List<List<Slot>> _shareSlots = new List<List<Slot>>();

    [SerializeField] private Slot[] sharedSlots;

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
    
    public GameObject SharePlace => sharePlace;
    public Slot[] SharedSlots => sharedSlots;
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

    public void SaveCardList()
    {
        /*
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
        */
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

    public void SharePlaceExpansion()
    {
        for (int i = 0; i < _shareSlots.Count; i++)
        {
            //열추가
            UnityEngine.Vector2 size = sharePlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta;
            size.x += 38;
            sharePlace.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = size;
            
            var slot = Instantiate(slotPrefab, _shareSlots[i][0].transform.parent);
            slot.transform.localPosition = _shareSlots[i][^1].transform.localPosition + new Vector3(38, 0,0);
            _shareSlots[i].Add(slot);
        }

        _addSlotCount = _shareSlots[0].Count;
        
        //행추가
        var line = Instantiate(SharePlaceLinePrefab, sharePlace.transform);

        while (true)
        {
            UnityEngine.Vector2 size = line.GetComponent<RectTransform>().sizeDelta;
            size.x += 38;
            line.GetComponent<RectTransform>().sizeDelta = size;
            
            var slot = Instantiate(slotPrefab, line.transform);
            slot.transform.localPosition = line.Slots[^1].transform.localPosition + new Vector3(38, 0,0);
            line.Slots.Add(slot);

            if (line.Slots.Count == _addSlotCount)
                break;
        }
        
        _shareSlots.Add(line.Slots);
        
        //sharePlace 크기 변경
        UnityEngine.Vector2 sharePlaceSize = sharePlace.GetComponent<RectTransform>().sizeDelta;
        sharePlaceSize.x += 38;
        sharePlaceSize.y += 52;
        sharePlace.GetComponent<RectTransform>().sizeDelta = sharePlaceSize;
        
        // 크기 조정
        if (sharePlace.GetComponent<RectTransform>().sizeDelta.x > 608)
        {
            float x = sharePlace.GetComponent<RectTransform>().sizeDelta.x;
            sharePlace.GetComponent<RectTransform>().localScale = Vector3.one;
            sharePlace.GetComponent<RectTransform>().localScale *=
                608 / x;
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
}


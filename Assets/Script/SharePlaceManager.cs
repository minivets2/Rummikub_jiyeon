using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Vector2 = System.Numerics.Vector2;

public class SharePlaceManager : Singleton<SharePlaceManager>
{
    [Header("Share Place")]
    [SerializeField] private GameObject sharePlace;
    private List<List<Slot>> _shareSlots = new List<List<Slot>>();

    [Header("Prefab")]
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Line SharePlaceLinePrefab;

    private List<GameObject> _previousPlayGround = new List<GameObject>();
    private List<Card> _previousCardList = new List<Card>();
    private List<Card> _newCardList = new List<Card>();
    private List<Card> _difference = new List<Card>();

    private int _addSlotCount = 0;
    private int _previousCardCount;

    public int PreviousCardCount => _previousCardCount;

    public delegate void CardDropEvent(int playerIndex, string cardStatus, int row, int column);
    public static CardDropEvent cardDropEvent;

    private void OnEnable()
    {
        Player.resetEvent += ResetCardList;
    }

    private void OnDisable()
    {
        Player.resetEvent -= ResetCardList;
    }

    public void InitSharePlace(GameObject sharePlace)
    {
        this.sharePlace = sharePlace;

        for (int i = 0; i < 4; i++)
        {
            var line = Instantiate(SharePlaceLinePrefab, sharePlace.transform);
            _shareSlots.Add(line.Slots);
        }
    }

    public void SaveCardList()
    {
        _previousPlayGround.Clear();
        _previousCardList.Clear();
        _previousCardCount = 0;

        for (int i = 0; i < _shareSlots.Count; i++)
        {
            for (int j = 0; j < _shareSlots[i].Count; j++)
            {
                if (_shareSlots[i][j].transform.childCount > 0)
                {
                    Transform child = _shareSlots[i][j].transform.GetChild(0);

                    if (child != null)
                    {
                        _previousPlayGround.Add(child.gameObject);
                        _previousCardList.Add(child.gameObject.GetComponent<Card>());
                        _previousCardCount++;
                    }
                }
                else
                {
                    _previousPlayGround.Add(null);
                }
            }
        }
    }

    public void ResetCardList()
    {
        _newCardList.Clear();

        for (int i = 0; i < _shareSlots.Count; i++)
        {
            for (int j = 0; j < _shareSlots[i].Count; j++)
            {
                if (_shareSlots[i][j].transform.childCount > 0)
                {
                    Transform child = _shareSlots[i][j].transform.GetChild(0);

                    if (child != null)
                    {
                        _newCardList.Add(child.gameObject.GetComponent<Card>());
                    }
                }
            }
        }

        CardComparer comparer = new CardComparer();
        _difference = _newCardList.Except(_previousCardList, comparer).ToList(); 

        for (int i = 0; i < _difference.Count; i++)
        {
            Vector2 vector2 = PlayerPlaceManager.Instance.FindEmptyPlayerSlotIndex();
            
            _difference[i].transform.SetParent(PlayerPlaceManager.Instance.PlayerSlots[(int)vector2.X][(int)vector2.Y].transform);
            _difference[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
            _difference[i].GetComponent<RectTransform>().localScale = Vector3.one;
        }

        List<Slot> slots = GetAllSlots();

        for (int i = 0; i < _previousPlayGround.Count; i++)
        {
            if (_previousPlayGround[i] != null)
            {
                _previousPlayGround[i].transform.SetParent(slots[i].transform);
                _previousPlayGround[i].GetComponent<RectTransform>().anchoredPosition = UnityEngine.Vector2.zero;
                _previousPlayGround[i].GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
        
        SetSlotStatus();
    }

    public void CheckOverlap()
    {
        CheckOverlap(_shareSlots);
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
                            if (i == slot.Count - 1 && j == slot[i].Count - 1)
                            {
                                child.SetParent(slot[0][0].transform);
                            }
                            else if (j == slot[i].Count - 1) child.SetParent(slot[i+1][0].transform);
                            else child.SetParent(slot[i][j+1].transform);

                            child.localPosition = Vector3.zero;
                            count++;
                        }
                    } 
                }
            }

            if (count == 0) break;
        }

        SetSlotStatus();
    }

    private void SetSlotStatus()
    {
        for (int i = 0; i < _shareSlots.Count; i++)
        {
            for (int j = 0; j < _shareSlots[i].Count; j++)
            {
                SlotStatus slotStatus = _shareSlots[i][j].GetSlotStatus();
                cardDropEvent?.Invoke(PhotonNetwork.LocalPlayer.ActorNumber - 1, slotStatus.CardStatus, slotStatus.Row, slotStatus.Column);
            }
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
    
    public List<Slot> GetAllSlots()
    {
        List<Slot> slots = new List<Slot>();

        for (int i = 0; i < _shareSlots.Count(); i++)
        {
            for (int j = 0; j < _shareSlots[i].Count; j++)
            {
                slots.Add(_shareSlots[i][j]);
            }
        }

        return slots;
    }
}


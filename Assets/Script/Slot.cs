using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public struct SlotStatus
{
    public int Row;
    public int Column;
    public string CardStatus;
}

public enum SlotType
{
    PlayerPlace,
    SharePlace
}

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private SlotType slotType;
    [SerializeField] private int row;
    [SerializeField] private int column;
    
    private Transform _otherCardTransform;

    public int Row => row;
    public int Column => column;
    public SlotType SlotType => slotType;

    private void Start()
    {
        column = FindChildIndex();
    }
    
    private void OnEnable()
    {
        //추후에 카드 정렬 완료 했을때 이벤트로 수정
        Player.endTurnEvent += SetMoveComplete;
        GameManager.dropCardEvent += DropCard;
    }

    private void OnDisable()
    {
        Player.endTurnEvent -= SetMoveComplete;
        GameManager.dropCardEvent -= DropCard;
    }

    private void SetMoveComplete(int index)
    {
        if (transform.childCount == 1 && slotType == SlotType.SharePlace)
        {
            transform.GetChild(0).GetComponent<Card>().SetMoveComplete(true);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        _otherCardTransform = eventData.pointerDrag.transform;
        
        if ((slotType == SlotType.SharePlace &&
            GameManager.Instance.CurrentPlayerIndex != PhotonNetwork.LocalPlayer.ActorNumber - 1) ||
            (_otherCardTransform.GetComponentInParent<Slot>().slotType == SlotType.SharePlace &&
             _otherCardTransform.GetComponent<Card>().MoveComplete && slotType == SlotType.PlayerPlace)) return;

        _otherCardTransform.SetParent(transform);
        _otherCardTransform.localPosition = Vector3.zero;
        _otherCardTransform.GetComponent<RectTransform>().localScale = Vector3.one;
        
        PlayerPlaceManager.Instance.CheckOverlap();
        PlayerPlaceManager.Instance.CheckPlaceSize();
        SharePlaceManager.Instance.CheckOverlap();
    }

    private int FindChildIndex()
    {
        int index = 0;
        
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetComponent<Line>() && transform.parent.GetChild(i) == transform)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void SetRow(int row)
    {
        this.row = row;
    }

    public SlotStatus GetSlotStatus()
    {
        SlotStatus st;
        st.Row = row;
        st.Column = column;
        st.CardStatus = "";

        if (transform.childCount == 1)
            st.CardStatus = transform.GetChild(0).GetComponent<Card>().Status;

        return st;
    }

    private void DropCard(string cardStatus, int row, int column)
    {
        if (slotType == SlotType.PlayerPlace) return;

        if (transform.childCount == 1)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        
        if (this.row == row && this.column == column && cardStatus != "")
        {
            var card = CardManager.Instance.CardCreate(cardStatus, false);
            
            card.transform.SetParent(transform);
            card.GetComponent<RectTransform>().localPosition = Vector3.zero;
            card.transform.localScale = Vector3.one;
        }
    }
}

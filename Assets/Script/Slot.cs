using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

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
    
    public delegate void CardEvent(int playerIndex, int row, int column, GameObject card);
    public static CardEvent dropCardEvent;

    private void Start()
    {
        column = FindChildIndex();
    }
    
    private void OnEnable()
    {
        //추후에 카드 정렬 완료 했을때 이벤트로 수정
        Player.endTurnEvent += SetMoveComplete;
        Player.completeEvent += CheckCard;
        GameManager.setCardEvent += SetCard;
    }

    private void OnDisable()
    {
        Player.endTurnEvent -= SetMoveComplete;
        Player.completeEvent -= CheckCard;
        GameManager.setCardEvent -= SetCard;
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
        
        PlaceManager.Instance.CheckOverlap();
        PlaceManager.Instance.CheckPlaceSize();
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

    private void CheckCard()
    {
        if (transform.childCount == 0) return;
        
        dropCardEvent?.Invoke( PhotonNetwork.LocalPlayer.ActorNumber -1, row, column, transform.GetChild(0).gameObject);
    }

    private void SetCard(int row, int column, GameObject card)
    {
        Debug.Log(row + "," + column);
    }
}

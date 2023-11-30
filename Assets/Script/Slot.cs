using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = System.Numerics.Vector2;

public enum SlotType
{
    PlayerPlace,
    SharePlace
}

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private SlotType slotType;
    [SerializeField] private int lineIndex;
    public Transform otherCardTransform; 
    public int LineIndex => lineIndex;
    public SlotType SlotType => slotType;

    private void OnEnable()
    {
        //추후에 카드 정렬 완료 했을때 이벤트로 수정
        Player.endTurnEvent += CardStatusChange;
    }

    private void OnDisable()
    {
        Player.endTurnEvent -= CardStatusChange;
    }

    private void CardStatusChange(int index)
    {
        if (transform.childCount == 1 && slotType == SlotType.SharePlace)
        {
            transform.GetChild(0).GetComponent<Card>().SetMoveComplete(true);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        otherCardTransform = eventData.pointerDrag.transform;
        
        if ((slotType == SlotType.SharePlace &&
            GameManager.Instance.CurrentPlayerIndex != PhotonNetwork.LocalPlayer.ActorNumber - 1) ||
            (otherCardTransform.GetComponentInParent<Slot>().slotType == SlotType.SharePlace &&
             otherCardTransform.GetComponent<Card>().MoveComplete && slotType == SlotType.PlayerPlace)) return;

        otherCardTransform.SetParent(transform);
        otherCardTransform.localPosition = Vector3.zero;
        otherCardTransform.GetComponent<RectTransform>().localScale = Vector3.one;
        PlaceManager.Instance.CheckOverlap();
        PlaceManager.Instance.CheckPlaceSize();
    }

    public void DestroyChildCard()
    {
        if (transform.childCount == 0)
            return;

        Transform childTransform = transform.GetChild(0);
        Destroy(childTransform.gameObject);
    }

    public Vector2 CardInfo()
    {
        if (transform.childCount == 0)
        {
            Vector2 nullInfo = new Vector2(100, 100);
            return nullInfo;
        }
        
        Transform childTransform = transform.GetChild(0);

        Card card = childTransform.GetComponent<Card>();

        Vector2 cardInfo = new Vector2(card.Number, (int)card.ColorType);
        return cardInfo;
    }
}

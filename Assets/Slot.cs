using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = System.Numerics.Vector2;

public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private int lineIndex;

    public int LineIndex => lineIndex;
    
    public void OnDrop(PointerEventData eventData)
    {
        var otherCardTransform = eventData.pointerDrag.transform;
        otherCardTransform.SetParent(transform);
        otherCardTransform.localPosition = Vector3.zero;
        
        PlaceManager placeManager = PlaceManager.Instance;
        placeManager.CheckOverlap();
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

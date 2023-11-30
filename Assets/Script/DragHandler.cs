using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector3 _offset;
    [SerializeField] private RectTransform _canvasRectTransform;
    private Transform _beginDragSlot;
    private Transform _beginDragLine;
    private int _beginDragSlotSiblingIndex;
    private int _beginDragLineSiblingIndex;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetComponent<Card>().MoveComplete) return;

        _beginDragSlot = _rectTransform.parent;
        _beginDragLine = _beginDragSlot.parent;
        _beginDragSlotSiblingIndex = _beginDragSlot.GetSiblingIndex();
        _beginDragLineSiblingIndex = _beginDragLine.GetSiblingIndex();
        
        transform.GetComponent<RectTransform>().localScale /= _beginDragLine.GetComponent<RectTransform>().localScale.x;
        transform.GetComponent<RectTransform>().localScale /= _beginDragLine.parent.GetComponent<RectTransform>().localScale.x;
        
        _beginDragSlot.SetAsLastSibling();
        _beginDragLine.SetAsLastSibling();;
        _canvasGroup.blocksRaycasts = false;
        _offset = transform.position - GetMouseWorldPos(eventData);

        if (_beginDragLine.GetComponent<HorizontalLayoutGroup>())
        {
            _beginDragLine.GetComponent<HorizontalLayoutGroup>().enabled = false;   
        }

        if (_beginDragLine.parent.GetComponent<VerticalLayoutGroup>())
        {
            _beginDragLine.parent.GetComponent<VerticalLayoutGroup>().enabled = false;   
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPos(eventData) + _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _beginDragSlot.SetSiblingIndex(_beginDragSlotSiblingIndex);
        _beginDragLine.SetSiblingIndex(_beginDragLineSiblingIndex);
        transform.localPosition = Vector3.zero;
        _canvasGroup.blocksRaycasts = true;
        
        if (_beginDragLine.GetComponent<HorizontalLayoutGroup>())
        {
           _beginDragLine.GetComponent<HorizontalLayoutGroup>().enabled = true;   
        }
        
        if (_beginDragLine.parent.GetComponent<VerticalLayoutGroup>())
        {
            _beginDragLine.parent.GetComponent<VerticalLayoutGroup>().enabled = true;   
        }
    }
    
    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 mousePosition = eventData.position;
        mousePosition.z = -_canvasRectTransform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}

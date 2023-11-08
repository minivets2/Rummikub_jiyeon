using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup _canvasGroup;
    private Canvas _mainCanvas;
    private RectTransform _rectTransform;

    private Transform _beginDragSlot;
    private Transform _beginDragLine;
    private int _beginDragSlotSiblingIndex;
    private int _beginDragLineSiblingIndex;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragSlot = _rectTransform.parent;
        _beginDragLine = _beginDragSlot.parent;
        _beginDragSlotSiblingIndex = _beginDragSlot.GetSiblingIndex();
        _beginDragLineSiblingIndex = _beginDragLine.GetSiblingIndex();
        
        _beginDragSlot.SetAsLastSibling();
        _beginDragLine.SetAsLastSibling();;
        _canvasGroup.blocksRaycasts = false;

        if (_beginDragLine.GetComponent<HorizontalLayoutGroup>())
        {
            _beginDragLine.GetComponent<HorizontalLayoutGroup>().enabled = false;   
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
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
    }
}

using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CardManager deckManager;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(deckManager.transform.root); // move to top of hierarchy while dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Detect if dropped on a valid drop zone
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool placed = false;
        foreach (var result in results)
        {
            DropZone zone = result.gameObject.GetComponent<DropZone>();
            if (zone != null && zone.CanAccept(deckManager))
            {
                transform.SetParent(zone.transform);
                rectTransform.anchoredPosition = Vector2.zero;
                placed = true;
                deckManager.OnCardPlayed(gameObject);
                GameManager.Instance.EndTurn();
                break;
            }
        }

        if (!placed)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}

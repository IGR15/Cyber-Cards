using UnityEngine;
using UnityEngine.EventSystems;


public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
     public CardManager deckManager;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Transform originalParent;

    // Reference to the root Canvas for accurate drag positioning
    private Canvas rootCanvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>(); // find the top-level canvas
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;

        // Temporarily reparent to top canvas so it draws over everything
        transform.SetParent(rootCanvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Use scaleFactor to ensure proper movement under different resolutions
        rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

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

using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
     [Header("Card Prefabs and Slots")]
    public GameObject cardPrefab;

    [Header("Layout References")]
    public Transform handRow;
    public Transform smallSlot;

    [Header("Deck Configuration")]
    public List<GameObject> deckCards = new List<GameObject>(); // 8 total
    private List<GameObject> hand = new List<GameObject>();
    private Queue<GameObject> deckQueue = new Queue<GameObject>();
    private GameObject nextCardInstance;

    public bool IsPlayer; // true = bottom player, false = top player

    public void InitializeDeck()
    {
        List<GameObject> shuffled = new(deckCards);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }

        foreach (var card in shuffled)
            deckQueue.Enqueue(card);

        DrawStartingHand();
        DisplayNextCardPreview();
    }

    void DrawStartingHand()
    {
        for (int i = 0; i < 4 && deckQueue.Count > 0; i++)
            DrawCardToHand();
    }

    void DrawCardToHand()
    {
        if (deckQueue.Count == 0) return;
        GameObject cardPrefabRef = deckQueue.Dequeue();
        GameObject newCard = Instantiate(cardPrefabRef, handRow);
        hand.Add(newCard);

        var draggable = newCard.GetComponent<DraggableCard>();
        if (draggable != null)
            draggable.deckManager = this;
    }

    public void OnCardPlayed(GameObject card)
    {
        hand.Remove(card);
        Destroy(card);
        DrawCardToHand();
        DisplayNextCardPreview();
    }

    void DisplayNextCardPreview()
    {
        if (deckQueue.Count == 0) return;

        GameObject nextCardPrefab = deckQueue.Peek();

        if (nextCardInstance != null)
            Destroy(nextCardInstance);

        nextCardInstance = Instantiate(nextCardPrefab, smallSlot);
        nextCardInstance.transform.localScale = Vector3.one * 0.8f;
    }
}

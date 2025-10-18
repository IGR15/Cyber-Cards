using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("Card Prefabs and Slots")]
    public GameObject cardPrefab;

    [Header("Layout References")]
    public Transform handRow;         // bottom row (or top row for enemy)
    public Transform smallSlot;       // small preview slot

    [Header("Deck Configuration")]
    public List<GameObject> deckCards = new List<GameObject>(); // 8 total prefabs
    private List<GameObject> hand = new List<GameObject>();
    private Queue<GameObject> deckQueue = new Queue<GameObject>();

    private GameObject nextCardInstance;

    void Start()
    {
        InitializeDeck();
        DrawStartingHand();
        DisplayNextCardPreview();
    }

    void InitializeDeck()
    {
        // Randomize the deck order
        List<GameObject> shuffled = new(deckCards);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }

        // Load shuffled cards into a queue
        foreach (GameObject card in shuffled)
            deckQueue.Enqueue(card);
    }

    void DrawStartingHand()
    {
        for (int i = 0; i < 4; i++)
        {
            DrawCardToHand();
        }
    }

    void DrawCardToHand()
    {
        if (deckQueue.Count == 0) return;

        GameObject cardPrefabRef = deckQueue.Dequeue();
        GameObject newCard = Instantiate(cardPrefabRef, handRow);
        hand.Add(newCard);
    }

    void DisplayNextCardPreview()
    {
        if (deckQueue.Count == 0) return;

        GameObject nextCardPrefab = deckQueue.Peek();

        // Destroy old preview if exists
        if (nextCardInstance != null)
            Destroy(nextCardInstance);

        nextCardInstance = Instantiate(nextCardPrefab, smallSlot);
        nextCardInstance.transform.localScale = Vector3.one * 0.8f; // slightly smaller visual
    }

    public void OnCardPlayed(GameObject playedCard)
    {
        // Remove from hand
        hand.Remove(playedCard);
        Destroy(playedCard);

        // Draw next card from deck
        DrawCardToHand();

        // Update preview
        DisplayNextCardPreview();
    }
}

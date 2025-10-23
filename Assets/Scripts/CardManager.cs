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
        // Shuffle deck
        List<GameObject> shuffled = new(deckCards);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }

        // Load into queue
        foreach (var card in shuffled)
            deckQueue.Enqueue(card);

        DrawStartingHand();
        DisplayNextCardPreview();
    }

    private void DrawStartingHand()
    {
        for (int i = 0; i < 4 && deckQueue.Count > 0; i++)
            DrawCardToHand();
    }

    private void DrawCardToHand()
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
        // Called when player drops card into BattleZone
        hand.Remove(card);
        Destroy(card);

        // Notify GameManager that this player has played a card
        GameManager.Instance.OnPlayerPlayedCard(this);
    }

    public void DrawNextCard()
    {
        // Draw next from queue after a round
        if (deckQueue.Count > 0)
        {
            DrawCardToHand();
            DisplayNextCardPreview();
        }
        else
        {
            if (nextCardInstance != null)
                Destroy(nextCardInstance);
        }
    }

    private void DisplayNextCardPreview()
    {
        if (deckQueue.Count == 0)
        {
            if (nextCardInstance != null)
                Destroy(nextCardInstance);
            return;
        }

        GameObject nextCardPrefab = deckQueue.Peek();

        if (nextCardInstance != null)
            Destroy(nextCardInstance);

        nextCardInstance = Instantiate(nextCardPrefab, smallSlot);
        nextCardInstance.transform.localScale = Vector3.one * 0.8f;

        // Disable drag for preview
        var drag = nextCardInstance.GetComponent<DraggableCard>();
        if (drag != null)
            Destroy(drag);
    }
}

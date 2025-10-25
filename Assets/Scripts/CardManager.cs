using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [Header("Data Sources")]
    public CardDataBase cardDatabase;
    public Transform handRow;
    public GameObject cardPrefab;
    [Header("Ownership")]
    public bool IsPlayer; // true = bottom player, false = top enemy


    [Header("Deck Configuration")]
    public int deckSize = 8;
    public bool randomize = true;

    private List<Card> deck = new();
    private List<GameObject> spawnedCards = new();

    public void InitializeDeck()
    {
        deck.Clear();

        // Example: half attack, half defense
        var attacks = cardDatabase.GetCardsByType(Card.CardType.Attack);
        var defenses = cardDatabase.GetCardsByType(Card.CardType.Deffense);

        for (int i = 0; i < deckSize / 2; i++)
        {
            deck.Add(attacks[Random.Range(0, attacks.Count)]);
            deck.Add(defenses[Random.Range(0, defenses.Count)]);
        }

        if (randomize)
            Shuffle(deck);

        SpawnHand();
    }

    void Shuffle(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }

    void SpawnHand()
    {
        foreach (Transform child in handRow)
            Destroy(child.gameObject);

        foreach (var card in deck)
        {
            GameObject cardObj = Instantiate(cardPrefab, handRow);
            var display = cardObj.GetComponent<CardDisplay>();
            display.cardData = card;
            display.UpdateCardDisplay();
            spawnedCards.Add(cardObj);
        }
    }
    public void OnCardPlayed(GameObject card)
{
    Debug.Log($"{card.name} was played from {name}");

    // Example: remove the card from the player’s hand
    if (handRow != null)
    {
        for (int i = 0; i < handRow.childCount; i++)
        {
            if (handRow.GetChild(i).gameObject == card)
            {
                // Remove card from hand (optional logic)
                break;
            }
        }
    }

    // Optional: move card to the BattleZone
    var battleZone = FindFirstObjectByType<BattleZone>();
    if (battleZone != null)
        battleZone.PlaceCard(card, this);
}

}

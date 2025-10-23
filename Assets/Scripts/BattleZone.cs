using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [Header("Battlefield Slots (Assign in Inspector)")]
    public Transform[] topSlots;      // 3 slots for top player
    public Transform[] bottomSlots;   // 3 slots for bottom player

    private GameManager gameManager;

    private List<GameObject> topCards = new List<GameObject>();
    private List<GameObject> bottomCards = new List<GameObject>();

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public bool PlaceCard(GameObject card, bool isPlayerBottom)
    {
        if (isPlayerBottom)
        {
            if (bottomCards.Count >= bottomSlots.Length)
                return false;

            int index = bottomCards.Count;
            bottomCards.Add(card);
            card.transform.SetParent(bottomSlots[index]);
            ResetCardTransform(card);
        }
        else
        {
            if (topCards.Count >= topSlots.Length)
                return false;

            int index = topCards.Count;
            topCards.Add(card);
            card.transform.SetParent(topSlots[index]);
            ResetCardTransform(card);
        }

        return true;
    }

    private void ResetCardTransform(GameObject card)
    {
        RectTransform rect = card.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;
    }

    /// <summary>
    /// Resolves all lane combat with attack/defense logic.
    /// </summary>
    public void ResolveCombat()
    {
        int pairCount = Mathf.Min(topCards.Count, bottomCards.Count);

        for (int i = 0; i < pairCount; i++)
        {
            GameObject topCardObj = topCards[i];
            GameObject bottomCardObj = bottomCards[i];

            Card topCard = topCardObj.GetComponent<CardDisplay>().cardData;
            Card bottomCard = bottomCardObj.GetComponent<CardDisplay>().cardData;

            int topNetDamage = CalculateNetDamage(topCard, bottomCard);
            int bottomNetDamage = CalculateNetDamage(bottomCard, topCard);

            Debug.Log($"Lane {i + 1}: Top → {topNetDamage}, Bottom → {bottomNetDamage}");

            if (topNetDamage > bottomNetDamage)
                Debug.Log($"Top player wins lane {i + 1}");
            else if (bottomNetDamage > topNetDamage)
                Debug.Log($"Bottom player wins lane {i + 1}");
            else
                Debug.Log($"Lane {i + 1} ends in draw");

            // Optional: remove both cards after battle
            Destroy(topCardObj);
            Destroy(bottomCardObj);
        }

        topCards.RemoveAll(card => card == null);
        bottomCards.RemoveAll(card => card == null);

        gameManager.OnRoundResolved();
    }

    /// <summary>
    /// Calculates net attack power considering defense values per attribute type.
    /// </summary>
    private int CalculateNetDamage(Card attacker, Card defender)
    {
        int totalDamage = 0;

        // Get attack attributes
        foreach (var attackVal in attacker.damageType)
        {
            int dmgValue = attackVal.value;
            Card.AttributeType attrType = attackVal.attributeType;

            // Check if defender has defense for this attribute
            int defValue = 0;
            foreach (var defVal in defender.deffenseType)
            {
                if (defVal.attributeType == attrType)
                {
                    defValue = defVal.value;
                    break;
                }
            }

            int net = Mathf.Max(0, dmgValue - defValue);
            totalDamage += net;
        }

        return totalDamage;
    }
    public void ClearAllCards()
    {
        foreach (Transform slot in topSlots)
            foreach (Transform child in slot)
                Destroy(child.gameObject);

        foreach (Transform slot in bottomSlots)
            foreach (Transform child in slot)
                Destroy(child.gameObject);

        topCards.Clear();
        bottomCards.Clear();
    }

}

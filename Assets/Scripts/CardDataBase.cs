using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDataBase", menuName = "Scriptable Objects/CardDataBase")]
public class CardDataBase : ScriptableObject
{
     public List<Card> allCards;

    public List<Card> GetCardsByType(Card.CardType type)
        => allCards.FindAll(c => c.cardType.Contains(type));

    public Card GetCardByName(string name)
        => allCards.Find(c => c.cardName == name);
}

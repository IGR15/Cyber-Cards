using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public string cardName;

    public List<CardType> cardType;
    public Sprite cardImage;
    public int energyCost;

    public int MinDamage;
    public int MaxDamage;

    public int Deffense;
    public string description;

    public List<DamageType> damageType;

    public List<DeffenseType> deffenseType;

    public enum CardType
    {
        Attack,
        Deffense,
        Utility,
        Heal
    }

    public enum DamageType
    {
        Confidentiality,
        Integrity,
        Availability,
        None
    }
    public enum DeffenseType
    {
        Confidentiality,
        Integrity,
        Availability,
        None
    }
}

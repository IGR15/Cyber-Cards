using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public string cardName;

    public List<CardType> cardType;
    public Image cardImage;
    public int energyCost;

 
    public string description;

    public List<AttributeValue> damageType;

    public List<AttributeValue> deffenseType;

    public enum CardType
    {
        Attack,
        Deffense,
        Utility,
        
    }

    public enum AttributeType
    {
        Confidentiality,
        Integrity,
        Availability
    }
     [System.Serializable]
    public struct AttributeValue
    {
        public AttributeType attributeType; // C, I, or A
        public int value;
    }

}

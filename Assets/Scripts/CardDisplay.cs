using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardDisplay : MonoBehaviour
{
    public Card cardData;

    public Image[] cardTypeImage; //attack or defense
    public TMP_Text CardName;

    public Sprite CardImage;
    public TMP_Text EnergyCost;
    public TMP_Text C_Damage;
    public TMP_Text I_Damage;
    public TMP_Text A_Damage;
    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        CardName.text = cardData.cardName;
        CardImage = cardData.cardImage;
        EnergyCost.text = cardData.energyCost.ToString();

        // Reset all type images to inactive
        foreach (var img in cardTypeImage)
        {
            img.gameObject.SetActive(false);
        }

        // Activate the relevant type images based on card types
        foreach (var type in cardData.cardType)
        {
            switch (type)
            {
                case Card.CardType.Attack:
                    cardTypeImage[0].gameObject.SetActive(true);
                    break;
                case Card.CardType.Deffense:
                    cardTypeImage[1].gameObject.SetActive(true);
                    break;
                case Card.CardType.Utility:
                    cardTypeImage[2].gameObject.SetActive(true);
                    break;
            }
        }

        // Reset damage texts
        C_Damage.text = "";
        I_Damage.text = "";
        A_Damage.text = "";

        // Set damage texts based on damage types
        foreach (var Type in cardData.cardType)
        {
            if (Type == Card.CardType.Attack)
            {

                foreach (var dmgType in cardData.damageType)
                {
                    switch (dmgType.attributeType)
                    {
                        case Card.AttributeType.Confidentiality:
                            C_Damage.text = $"{dmgType.value}";
                            break;
                        case Card.AttributeType.Integrity:
                            I_Damage.text = $"{dmgType.value}";
                            break;
                        case Card.AttributeType.Availability:
                            A_Damage.text = $"{dmgType.value}";
                            break;
                    }
                }
            }
            else if (Type == Card.CardType.Deffense)
            {
                foreach (var DefType in cardData.deffenseType)
                {
                    switch (DefType.attributeType)
                    {
                        case Card.AttributeType.Confidentiality:
                            C_Damage.text = $"{DefType.value}";
                            break;
                        case Card.AttributeType.Integrity:
                            I_Damage.text = $"{DefType.value}";
                            break;
                        case Card.AttributeType.Availability:
                            A_Damage.text = $"{DefType.value}";
                            break;
                    }
                }
            }
            else
            {
                C_Damage.text = "";
                I_Damage.text = "";
                A_Damage.text = "";
            }
        }
    }

}

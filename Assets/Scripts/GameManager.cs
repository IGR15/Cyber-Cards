using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CardManager playerDeck;
    public CardManager enemyDeck;

    private bool isPlayerTurn = true;

    void Awake() => Instance = this;

    void Start()
    {
        playerDeck.InitializeDeck();
        enemyDeck.InitializeDeck();
        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        isPlayerTurn = true;
        EnableHand(playerDeck, true);
        EnableHand(enemyDeck, false);
    }

    void StartEnemyTurn()
    {
        isPlayerTurn = false;
        EnableHand(playerDeck, false);
        EnableHand(enemyDeck, true);
    }

    public void EndTurn()
{
    if (isPlayerTurn)
        StartEnemyTurn();
    else
        StartPlayerTurn();
}


    void EnableHand(CardManager deck, bool enable)
    {
        foreach (Transform card in deck.handRow)
        {
            if (card.TryGetComponent(out CanvasGroup cg))
                cg.interactable = enable;
        }
    }
}

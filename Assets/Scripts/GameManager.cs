using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public CardManager playerDeck;
    public CardManager enemyDeck;
    public BattleZone battleZone;

    private bool isPlayerTurn = true;
    private bool playerPlayed = false;
    private bool enemyPlayed = false;
    private int roundCount = 1;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        playerDeck.InitializeDeck();
        enemyDeck.InitializeDeck();
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        Debug.Log($"🔵 Player Turn {roundCount}");
        isPlayerTurn = true;
        playerPlayed = false;
        enemyPlayed = false;

        EnableHand(playerDeck, true);
        EnableHand(enemyDeck, false);
    }

    private void StartEnemyTurn()
    {
        Debug.Log($"🔴 Enemy Turn {roundCount}");
        isPlayerTurn = false;
        EnableHand(playerDeck, false);
        EnableHand(enemyDeck, true);

        // Simulate enemy AI with delay
        StartCoroutine(EnemyPlayDelay());
    }

    private IEnumerator EnemyPlayDelay()
    {
        yield return new WaitForSeconds(2f); // Simulate AI thinking
        enemyPlayed = true;

        // TODO: Replace this with actual AI card placement
        battleZone.PlaceCard(CreateDummyEnemyCard(), false);

        CheckRoundReady();
    }

    private GameObject CreateDummyEnemyCard()
    {
        // Pick first card from enemy hand
        if (enemyDeck.handRow.childCount > 0)
        {
            GameObject card = enemyDeck.handRow.GetChild(0).gameObject;
            enemyDeck.OnCardPlayed(card);
            return card;
        }
        return null;
    }

    public void OnPlayerPlayedCard(CardManager deck)
    {
        if (deck == playerDeck)
            playerPlayed = true;
        else
            enemyPlayed = true;

        CheckRoundReady();
    }

    private void CheckRoundReady()
    {
        if (playerPlayed && enemyPlayed)
        {
            Debug.Log("🧮 Both players played. Resolving round...");
            StartCoroutine(ResolveRoundRoutine());
        }
        else if (playerPlayed && !enemyPlayed)
        {
            StartEnemyTurn();
        }
    }

    private IEnumerator ResolveRoundRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        battleZone.ResolveCombat();
    }

    public void OnRoundResolved()
    {
        Debug.Log($"✅ Round {roundCount} resolved!");
        roundCount++;

        playerDeck.DrawNextCard();
        enemyDeck.DrawNextCard();

        battleZone.ClearAllCards();
        StartPlayerTurn();
    }

    private void EnableHand(CardManager deck, bool enable)
    {
        foreach (Transform card in deck.handRow)
        {
            var cg = card.GetComponent<CanvasGroup>();
            if (cg != null)
                cg.interactable = enable;
        }
    }
}

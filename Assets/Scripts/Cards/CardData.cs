using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardData : NetworkBehaviour
{
    private string path = "ScriptableCards/";

    [SerializeField] private CardDisplay cardDisplay;
    [SyncVar] public CardState state = CardState.Uninitialized;

    [SyncVar(hook = nameof(UpdateManaCost))] public int currentMana;
    [SyncVar(hook = nameof(UpdateHealth))] public int currentHealth;
    [SyncVar(hook = nameof(UpdateAttack))] public int currentAttack;
    [SyncVar] public int AttacksLeft = 0;
    // ^^each turn reset this using the matchdatabase going through all the cards in the players side of the board, reseting the attacks left to 1

    public Card card;

    public void InitializeCard(string cardName)
    {
        card = Resources.Load<Card>(path + cardName);
        state = CardState.Hand;
        currentMana = card.manaCost;
        currentHealth = card.health;
        currentAttack = card.attack;
        cardDisplay.InitializeCard(card);
    }

    public void UpdateHealth(int oldHealth, int newHealth)
    {
        cardDisplay.UpdateHealthText(newHealth);
    }

    public void UpdateAttack(int oldAttack, int newAttack)
    {
        cardDisplay.UpdateAttackText(newAttack);
    }

    public void UpdateManaCost(int oldManaCost, int newManaCost)
    {
        cardDisplay.UpdateManaCostText(newManaCost);
    }
}

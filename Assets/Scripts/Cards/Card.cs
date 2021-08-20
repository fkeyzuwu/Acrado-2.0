using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite sprite;

    public int manaCost;
    public int attack;
    public int health;

    public CardState state = CardState.Deck;

    //TODO: add list of effects of some sort once i work on game logic
}

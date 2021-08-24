using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite sprite;

    private int manaCost;
    private int health;
    private int attack;

    public CardState state = CardState.Uninitialized;

    [SerializeReference] public List<CardEffect> effects;

    public int ManaCost
    {
        get { return ManaCost; }
        set { manaCost = value; }
    }

    public int Health
    {
        get { return health; }
        set 
        {
            health = value;
        }
    }

    public int Attack
    {
        get { return attack; }
        set 
        { 
            attack = value; 
        }
    }
}

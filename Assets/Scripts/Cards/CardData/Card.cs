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

    public int manaCost;
    public int attack;
    public int health;

    public CardState state = CardState.Uninitialized;

    [SerializeReference] public List<CardEffect> effects;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[Serializable]
public class CardEffect
{
    [SerializeReference] public Target target;
    [SerializeReference] public Trigger trigger;
    public virtual void Activate()
    {

    }
}
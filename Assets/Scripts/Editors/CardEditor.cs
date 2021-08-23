using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Card))]
class CardInspector : Editor
{
    private Type[] cardEffectImplementations;
    private int cardEffectImplementationTypeIndex;

    private Type[] triggerImplementations;
    private int triggerImplementationTypeIndex;

    private Type[] targetImplementations;
    private int targetImplementationTypeIndex;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Card card = target as Card;
        //specify type
        if (card == null)
        {
            return;
        }

        if (cardEffectImplementations == null || targetImplementations == null || targetImplementations == null|| GUILayout.Button("Refresh Implementations"))
        {
            //this is probably the most imporant part:
            //find all cardEffectImplementations of INode using System.Reflection.Module
            cardEffectImplementations = GetImplementations<CardEffect>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
            triggerImplementations = GetImplementations<Trigger>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
            targetImplementations = GetImplementations<Target>().Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        //select implementation from editor popup
        cardEffectImplementationTypeIndex = EditorGUILayout.Popup(new GUIContent("CardEffect"),
            cardEffectImplementationTypeIndex, cardEffectImplementations.Select(impl => impl.FullName).ToArray());

        targetImplementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Target"),
            targetImplementationTypeIndex, targetImplementations.Select(impl => impl.FullName).ToArray());

        triggerImplementationTypeIndex = EditorGUILayout.Popup(new GUIContent("Trigger"),
            triggerImplementationTypeIndex, triggerImplementations.Select(impl => impl.FullName).ToArray());

        if (GUILayout.Button("Create instance"))
        {
            //set new value
            card.effects.Add((CardEffect)Activator.CreateInstance(cardEffectImplementations[cardEffectImplementationTypeIndex]));
            CardEffect cardEffect = card.effects[card.effects.Count - 1];
            cardEffect.target = ((Target)Activator.CreateInstance(targetImplementations[targetImplementationTypeIndex]));
            cardEffect.trigger = ((Trigger)Activator.CreateInstance(triggerImplementations[triggerImplementationTypeIndex]));
        }
    }

    private static Type[] GetImplementations<T>()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

        var interfaceType = typeof(T);
        return types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToArray();
    }
}

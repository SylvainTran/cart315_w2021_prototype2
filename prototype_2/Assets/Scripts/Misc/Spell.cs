using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Item
{
    [Header("Gameplay Design Parameters")]
    [SerializeField] private float trapDamage = 10.0f;
    public float TrapDamage { get { return trapDamage; } set { trapDamage = value; } }
    [SerializeField] private float trapEffectRadius = 10.0f;
    public float TrapEffectRadius { get { return trapEffectRadius; } set { trapEffectRadius = value; } }
    [SerializeField] private int sizeInInventory = 1;
    public int SizeInInventory { get { return sizeInInventory; } }

    /**
    * Physics force settings if applicable.
    */ 
    [Header("Physics Parameters")]
    [SerializeField] private Vector3 physicsParameter1;
    public Vector3 PhysicsParameter1 { get { return physicsParameter1;} set {physicsParameter1 = value; } }
}

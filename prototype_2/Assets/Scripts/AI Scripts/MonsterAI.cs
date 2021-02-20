using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : Bot
{
    public void Update()
    {
        Wander();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MenteBacata.ScivoloCharacterController;
using MenteBacata.ScivoloCharacterControllerDemo;

public class MonsterAI : Bot
{
    public void Update()
    {
        Wander();
        if (!coolDown)
        {
            // Wander until in range of player
            if (CanSeeTarget() && !TargetCanSeeMe())
            {
                Pursue();
            } else if (CanSeeTarget() && TargetCanSeeMe()) {
                Evade();
            }
        }
    }
}

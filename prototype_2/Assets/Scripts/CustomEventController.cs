using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventController : EventTrigger
{
    public static string currentPlayerActionTargetEffected;
    public static string eventLookedFor;

    public static void EnableConversationFlow()
    {
        ConversationController.pauseConversations = false;
        ConversationController.dialogueActionIterator++;
    }
    public static void Flush()
    {
        eventLookedFor = null;
    }
}

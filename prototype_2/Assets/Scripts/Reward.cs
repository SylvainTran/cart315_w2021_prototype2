using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    [SerializeField]
    private int moneyReward = 0;
    public int MoneyReward { get { return moneyReward; } set { moneyReward = value; } }
}

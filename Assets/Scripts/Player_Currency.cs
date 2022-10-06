using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Currency : MonoBehaviour
{
    public int Money;
    public int SP;
    public int startingMoney = 60;
    public int specialPoints = 100;

    void Start()
    {
        Money = startingMoney;
        SP = specialPoints;
    }
  
}

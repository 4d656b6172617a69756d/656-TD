using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Player_Currency : MonoBehaviour
{
    public int moneyStart = 100;
    public int manaStart = 100;
    public int startLives = 20;

    public static int money;
    public static int mana;
    public static int lives;

    private void Start()
    {
        money = moneyStart;
        mana = manaStart;
        lives = startLives;
    }
    
}

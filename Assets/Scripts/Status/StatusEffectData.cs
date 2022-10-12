using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public string Name;
    public float DOTAmount;
    public float TickSpeed;
    public float MovementPenalty;
    public float Lifetime;
    public float DPSBoost;
}

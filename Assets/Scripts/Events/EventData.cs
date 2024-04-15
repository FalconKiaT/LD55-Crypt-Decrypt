using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EventData
{
    /// <summary>
    /// Event that will be triggered when the player dies
    /// </summary>
    public static System.Action OnPlayerDeath;
    public static void RaiseOnPlayerDeath() => OnPlayerDeath?.Invoke();

    public static System.Action<int> OnSummonableDeath;

    public static void RaiseOnSummonableDeath(int bones) => OnSummonableDeath?.Invoke(bones);

    //public static System.Action OnPause?????????

}

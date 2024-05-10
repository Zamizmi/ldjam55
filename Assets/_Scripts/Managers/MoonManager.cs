using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonManager : MonoBehaviour
{
    private void Start()
    {
        GameLoopManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
    }

    private void LevelManager_OnStateChanged(object sender, GameLoopManager.OnStateChangedEventArgs e)
    {
        if (e.changedState == GameLoopManager.State.LevelWon)
        {
            // PlayAnimation!
            // Move Camera to player looking the Moon
        }
    }
}

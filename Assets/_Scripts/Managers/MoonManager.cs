using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonManager : MonoBehaviour
{
    private void Start()
    {
        LevelManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
    }

    private void LevelManager_OnStateChanged(object sender, LevelManager.OnStateChangedEventArgs e)
    {
        if (e.changedState == LevelManager.State.LevelWon)
        {

        }
    }
}

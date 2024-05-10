using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainMenuCamera;
    [SerializeField] private CinemachineVirtualCamera pinkCamera;

    private void Start()
    {
        GameLoopManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
    }

    private void LevelManager_OnStateChanged(object sender, GameLoopManager.OnStateChangedEventArgs e)
    {
        if (e.changedState == GameLoopManager.State.GamePlaying)
        {
            mainMenuCamera.gameObject.SetActive(false);
            pinkCamera.gameObject.SetActive(true);
        }
        // if (e.changedState == LevelManager.State.LevelWon)
        // {
        //     mainMenuCamera.gameObject.SetActive(true);
        //     pinkCamera.gameObject.SetActive(false);
        // }
    }
}

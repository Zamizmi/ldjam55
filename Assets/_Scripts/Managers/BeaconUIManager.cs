using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeaconUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beaconCount;

    private void Start()
    {
        GameLoopManager.Instance.OnBeaconCountChanged += LevelManager_BeaconCountChanged;
        GameLoopManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
    }

    private void LevelManager_BeaconCountChanged(object sender, System.EventArgs e)
    {
        beaconCount.text = GameLoopManager.Instance.GetActiveBeaconCount().ToString();
    }

    private void LevelManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameLoopManager.Instance.IsGameActive() || GameLoopManager.Instance.IsRespawn())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

}

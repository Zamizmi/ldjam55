using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeaconUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI beaconCount;

    private void Start()
    {
        LevelManager.Instance.OnBeaconCountChanged += LevelManager_BeaconCountChanged;
        LevelManager.Instance.OnStateChanged += LevelManager_OnStateChanged;
        Hide();
    }

    private void LevelManager_BeaconCountChanged(object sender, System.EventArgs e)
    {
        beaconCount.text = LevelManager.Instance.GetActiveBeaconCount().ToString();
    }

    private void LevelManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (LevelManager.Instance.IsGameActive())
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Beacon : BaseInteractable
{
    [SerializeField] private float progress;
    [SerializeField] private Light2D fireLight;

    public override void Interact(Player player)
    {
        fireLight.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (progress > 0) progress -= Time.deltaTime;
    }
}

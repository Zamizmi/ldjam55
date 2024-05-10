using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Beacon : BaseInteractable
{
    [SerializeField] private float progress;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private SpriteRenderer visualRenderer;
    [SerializeField] private Sprite litImage;
    [SerializeField] private Light2D fireLight;
    [SerializeField] private SelectedInteractableVisual selectedVisual;
    private bool isLit = false;

    private void Start()
    {
        selectedVisual = GetComponent<SelectedInteractableVisual>();
    }

    public bool IsLit()
    {
        return isLit;
    }
    public override void Interact(Player player)
    {
        if (!isLit)
        {
            visualRenderer.sprite = litImage;
            isLit = true;
            fireLight.gameObject.SetActive(true);
            GameLoopManager.Instance.IncreaseActiveBeaconCount();
            selectedVisual.SetIsActive(false);
        }
    }

    private void Update()
    {
        if (progress > 0) progress -= Time.deltaTime;
    }
}

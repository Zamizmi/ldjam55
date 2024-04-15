using UnityEngine.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : BaseInteractable
{
    [SerializeField] private Teleporter targetTeleport;
    [SerializeField] private Transform transformToTeleport;
    [SerializeField] private SplineContainer currentLevel;
    [SerializeField] private int layerTarget;

    public override void Interact(Player player)
    {
        // player.SetSpriteLevel(targetTeleport.GetLayerTarget());
        player.transform.position = targetTeleport.GetTeleportTransform().position;
        player.SetSplineContainer(targetTeleport.GetTeleportSpline());
    }

    public void SetTargetTeleport(Teleporter teleport)
    {
        targetTeleport = teleport;
    }

    public Transform GetTeleportTransform()
    {
        return transformToTeleport;
    }

    public SplineContainer GetTeleportSpline()
    {
        return currentLevel;
    }

    public int GetLayerTarget()
    {
        return layerTarget;
    }

}

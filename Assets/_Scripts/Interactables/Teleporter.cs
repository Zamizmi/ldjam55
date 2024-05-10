using UnityEngine.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : BaseInteractable
{
    [SerializeField] private Teleporter targetTeleport;
    [SerializeField] private Transform transformToTeleport;
    [SerializeField] private LevelEntity currentLevel;
    [SerializeField] private GameObject visualChild;
    [SerializeField] private int layerTarget;

    public override void Interact(Player player)
    {
        player.transform.position = targetTeleport.GetTeleportTransform().position;
        player.SetLevelContainer(targetTeleport.GetCurrentLevel());
    }

    public void PairWithTeleport(Teleporter newTeleport)
    {
        targetTeleport = newTeleport;
        if (newTeleport.GetPairedTeleport() != this)
        {
            newTeleport.PairWithTeleport(this);
        }
    }

    public void SetRenderColor(Color color)
    {
        visualChild.GetComponent<SpriteRenderer>().color = color;
    }

    public Teleporter GetPairedTeleport()
    {
        return targetTeleport;
    }

    public void SetCurrentLevel(LevelEntity newCurrentLevel)
    {
        currentLevel = newCurrentLevel;
    }

    public LevelEntity GetCurrentLevel()
    {
        return currentLevel;
    }

    public Transform GetTeleportTransform()
    {
        return transformToTeleport;
    }
    public int GetLayerTarget()
    {
        return layerTarget;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineStartingPole : BaseInteractable
{
    [SerializeField] private ZipLine parentZipLine;
    public override void Interact(Player player)
    {
        parentZipLine.Interact(player);
    }
}

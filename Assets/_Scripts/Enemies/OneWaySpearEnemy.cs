using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OneWaySpearEnemy : BaseEnemy
{
    // false = left
    // true = right
    private void Start()
    {
        // Set the value of the enemy. Stays same for ever
        SetDirection(1 == Random.Range(0, 2));
        animator = GetComponentInChildren<Animator>();
        visualRender = GetComponentInChildren<SpriteRenderer>();
    }

    private void SetDirection(bool directionToGo)
    {
        this.directionToGo = directionToGo;
    }

}

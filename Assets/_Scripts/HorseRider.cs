using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseRider : MonoBehaviour
{
    [SerializeField] private LevelEntity levelToClear;
    [SerializeField] private float speed;

    public void SetupHorse(LevelEntity levelToClear)
    {
        this.levelToClear = levelToClear;
    }
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (levelToClear == null) return;
        Vector3 positionToMove = BaseSplineMovement.GetNextPositionOnSpline(levelToClear.GetNativeSpline(), transform.position, 1 * speed);
        if (positionToMove.x < transform.position.x)
        {
            // Warped ouside of map. Job Done.
            Destroy(this.gameObject);
            return;
        }
        transform.position = positionToMove;
    }
}

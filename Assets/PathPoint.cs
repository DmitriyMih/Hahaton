using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    [SerializeField] private PathPoint lastPoint;
    [SerializeField] private PathPoint nextPoint;

    public void InitializationPoint(PathPoint lastLPathPoint, PathPoint nextLPathPoint)
    {
        lastPoint = lastLPathPoint;
        nextPoint = nextLPathPoint;
    }
}
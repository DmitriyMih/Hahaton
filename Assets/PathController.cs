using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] private List<PathPoint> pathPoints = new List<PathPoint>();
    [SerializeField] private Transform pathGroup;

    [ContextMenu("Get Path Points")]
    private void GetPathPoints()
    {
        if (pathGroup == null)
            return;

        pathPoints.Clear();
        pathPoints.AddRange(pathGroup.GetComponentsInChildren<PathPoint>());
        SetNewName();
        InitializationPath();
    }

    private void SetNewName()
    {
        for (int i = 0; i < pathPoints.Count; i++)
        {
            pathPoints[i].name = "Path Point - " + i;
        }
    }

    private void InitializationPath()
    {
        if (pathPoints.Count < 2)
            return;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            PathPoint tempLast = i > 0 ? pathPoints[i - 1] : null;
            PathPoint tempNext = i < pathPoints.Count - 1 ? pathPoints[i + 1] : null;

            pathPoints[i].InitializationPoint(tempLast, tempNext);
        }
    }
}
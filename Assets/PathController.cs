using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] private List<PathPoint> pathPoints = new List<PathPoint>();
    [SerializeField] private Transform pathGroup;

    [SerializeField] private LineRenderer lineRenderer;
    private void GetPathPoints()
    {
        if (pathGroup == null)
            return;

        pathPoints.AddRange(pathGroup.GetComponentsInChildren<PathPoint>());
        CreatePathLine();
    }

    private void CreatePathLine()
    {
        if (lineRenderer == null)
            return;

        lineRenderer.positionCount = pathPoints.Count;

        for (int i = 0; i < pathPoints.Count; i++)
            lineRenderer.SetPosition(i, new Vector3(pathPoints[i].transform.position.x, pathPoints[i].transform.position.z, pathPoints[i].transform.position.y));
    }
}
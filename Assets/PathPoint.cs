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
    [Header("Place Settings")]
    public bool isPlace = true;

    [Header("Debug Settings")]
    [SerializeField] private Color tempPointColor = Color.green;
    [SerializeField] protected float debugRadius = 0.5f;

    [SerializeField] protected Color currentColor;
    [SerializeField] protected Color currentLineColor = Color.white;

    [Header("Connect Settings")]
    public BaseUnit unitController;

    private void OnEnable()
    {
        ChangeColor(false, Color.white);
    }

    public void SetColor(Color newColor)
    {
        tempPointColor = newColor;
        ChangeColor(false, Color.white);
    }

    public void SetLineColor(Color newColor)
    {
        currentLineColor = newColor;
    }

    public void ChangeColor(bool isNew, Color newColor)
    {
        currentColor = isNew ? newColor : tempPointColor;
    }

    public void ChangeCurrentUnit(BaseUnit bus)
    {
        unitController = bus;
    }

    public virtual PathPoint CheckNextPoint(BaseUnit bus)
    {
        if (nextPoint != null)
        {
            return nextPoint;
        }
        else
            return null;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = nextPoint != null ? currentColor : Color.red;
        Gizmos.DrawSphere(transform.position, debugRadius);

        if (nextPoint != null)
        {
            Gizmos.color = currentLineColor;
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
        }
    }
}
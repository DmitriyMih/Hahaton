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
    [SerializeField] private Color currentColor;
    [SerializeField] private Color tempPointColor = Color.green;
    [SerializeField] protected float debugRadius = 0.5f;

    [Header("Connect Settings")]
    public BaseUnit unitController;

    private void OnEnable()
    {
        ChangeColor(false, Color.white);
    }

    public void ChangeColor(bool isNew, Color newColor)
    {
        currentColor = isNew ? newColor : tempPointColor;
    }

    public void ChangeCurrentUnit(BaseUnit bus)
    {
        unitController = bus;
    }

    public virtual PathPoint CheckNextPoint(BaseUnit bus, BaseUnit.Direction direction)
    {
        switch (direction)
        {
            case BaseUnit.Direction.Forward:
                if (nextPoint != null)
                    return nextPoint;
                break;

            case BaseUnit.Direction.Back:
                if (lastPoint != null)
                    return lastPoint;
                break;
        }

        return null;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = nextPoint != null ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, debugRadius);

        if (nextPoint != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
        }
    }
}
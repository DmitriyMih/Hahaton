using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseUnit : MonoBehaviour
{
    private bool isEnemy = false;

    private Direction currentDirection = Direction.Forward;
    public enum Direction
    {
        Forward,
        Back
    }

    [SerializeField] private Transform carBody;

    [Space]
    [Header("Move Settings")]
    private PlayerInput playerInput;

    [SerializeField] private float maxSpeed = 10;
    [SerializeField] private float currentSpeed;

    [SerializeField] private PathPoint currentPoint;
    [SerializeField] private PathPoint nextPoint;

    [SerializeField] private float maxDistanceToNextPoint = 0.1f;
    [SerializeField] private float lookRotationTime = 0.2f;

    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private Color currentSelectColor;
    [SerializeField] private bool connectToPoint = false;

    [Header("Speed Curve Settings")]
    [SerializeField] private Vector2 firstPoint = new Vector2(3f, 2.5f);
    private Vector2 tempFirstPoint;
    private float tempSpeed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void InitializationUnit(Direction direction, PathPoint startPoint)
    {
        currentPoint = startPoint;
        transform.position = currentPoint.transform.position;

        currentDirection = direction;
        isEnemy = currentDirection == Direction.Back;

        if (nextPoint != null)
            nextPoint.ChangeColor(false, Color.white);

        nextPoint = null;
    }

    private void FixedUpdate()
    {
        //  speed   upgrade
        float speed = maxSpeed;

        if (tempSpeed != speed || tempFirstPoint.x != firstPoint.x || tempFirstPoint.y != firstPoint.y)
        {
            Debug.Log("Update Speed Point");
            tempFirstPoint = firstPoint;

            tempSpeed = speed;
            Keyframe maxKey = new Keyframe(playerInput.maxInputValue, speed);
            speedCurve.MoveKey(speedCurve.keys.Length - 1, maxKey);

            if (speedCurve.length < 3)
            {
                speedCurve.AddKey(playerInput.maxInputValue / firstPoint.x, speed / firstPoint.y);
                Debug.Log("Add Key " + speedCurve.keys[1]);
            }
            else
            {
                Keyframe firstKey = new Keyframe(playerInput.maxInputValue / firstPoint.x, speed / firstPoint.y);
                speedCurve.MoveKey(1, firstKey);
            }
        }

        float valueInCurve = Mathf.Clamp(playerInput.isMove, 0, playerInput.maxInputValue);
        currentSpeed = speedCurve.Evaluate(valueInCurve);

        if (currentPoint == null)
        {
            //Debug.Log("Start Point Is Null");
            return;
        }
        else
        {
            if (nextPoint == null)
                FindNewPoint(currentPoint);
            else
            {
                if (currentSpeed == 0)
                    return;

                transform.position = Vector3.MoveTowards(transform.position, nextPoint.transform.position, Time.deltaTime * currentSpeed);
                float distance = (transform.position - nextPoint.transform.position).sqrMagnitude;
                if (distance < maxDistanceToNextPoint * maxDistanceToNextPoint)
                {

                    FindNewPoint(nextPoint);
                }
            }
        }
    }

    private void StartRotation()
    {
        Quaternion lookRotation = Quaternion.LookRotation(nextPoint.transform.position - transform.position);
        transform.DORotateQuaternion(lookRotation, lookRotationTime);
    }

    private void FindNewPoint(PathPoint point)
    {
        PathPoint tempPoint = point.CheckNextPoint(this, currentDirection);

        if (tempPoint == null)
            return;
        else
        {
            if (tempPoint.unitController != null)
                if (tempPoint.unitController != this)
                    return;

            if (nextPoint != null)
            {
                currentPoint.ChangeCurrentUnit(null);
                currentPoint.ChangeColor(false, Color.white);

                currentPoint = nextPoint;
                currentPoint.ChangeColor(true, currentSelectColor);
            }

            nextPoint = tempPoint;
            nextPoint.ChangeCurrentUnit(this);
            nextPoint.ChangeColor(true, currentSelectColor);
        }

        if (connectToPoint == true)
            transform.parent = nextPoint.transform;
        else
            transform.parent = null;

        StartRotation();
    }
}

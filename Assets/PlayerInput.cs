using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float brakingSpeed = 4;
    [SerializeField] private float accelerationSpeed = 2;

    [SerializeField] public bool isAutomatic = false;

    public float isMove;
    //[HideInInspector] 
    public float maxInputValue = 1f;

    private void Update()
    {
        if ((Input.touchCount > 0 || Input.GetMouseButton(0)) || isAutomatic)
            isMove = Mathf.Clamp(isMove + Time.deltaTime * accelerationSpeed, 0f, maxInputValue);
        else
            isMove = Mathf.Clamp(isMove - Time.deltaTime * brakingSpeed, 0f, maxInputValue);
    }
}

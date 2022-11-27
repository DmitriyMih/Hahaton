using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    [SerializeField] private Transform castle;

    private int clickIndex = 0;
    private void OnMouseDown()
    {
        clickIndex += 1;
        
        if (clickIndex==2)
        {

        }
    }
}
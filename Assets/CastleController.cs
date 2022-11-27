using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    [Header("Connect Settings")]
    [SerializeField] private bool isPlayerInteractable = false;
    public bool IsPlayerInteractable => isPlayerInteractable;

    [SerializeField] private Transform castle;

    [Header("View Settings")]
    [SerializeField] private CastleUI castleUI;
    [SerializeField] private float cooldownTime = 0.25f;

    private int clickIndex = 0;
    private bool isInteractable = true;

    private void OnMouseDown()
    {
        if (!isPlayerInteractable || castleUI == null)
            return;

        if (!isInteractable)
            return;

        clickIndex += 1;
        
        if (clickIndex==2)
        {
            castleUI.ChangeState();
            clickIndex = 0;

            StartCoroutine(Cooldown(cooldownTime));
        }
    }

    private IEnumerator Cooldown(float time)
    {
        isInteractable = false;
        yield return new WaitForSeconds(time);
        isInteractable = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    [Header("Connect Settings")]
    [SerializeField] private bool isPlayerInteractable = false;
    public bool IsPlayerInteractable => isPlayerInteractable;

    [Header("Interact Settings")]
    [SerializeField] private float tapClearTime = 0.5f;
    private Coroutine clearCoroutine;

    [Header("View Settings")]
    [SerializeField] private CastleUI castleUI;
    [SerializeField] private float cooldownTime = 0.25f;

    [SerializeField] private int clickIndex = 0;
    private bool isInteractable = true;

    private void Awake()
    {
        if (castleUI != null)
            castleUI.Initialization(transform);
    }

    private void CreateUnit()
    {

    }

    private void OnMouseDown()
    {
        if (!isPlayerInteractable || castleUI == null)
            return;

        if (!isInteractable)
            return;

        clickIndex += 1;

        if (clearCoroutine != null)
            StopCoroutine(clearCoroutine);
        StartCoroutine(ClearInput(tapClearTime));

        if (clickIndex == 2)
        {
            castleUI.ChangeState();
            clickIndex = 0;

            if (clearCoroutine != null)
                StopCoroutine(clearCoroutine);

            StartCoroutine(Cooldown(cooldownTime));
        }
    }

    private IEnumerator Cooldown(float time)
    {
        isInteractable = false;
        yield return new WaitForSeconds(time);
        isInteractable = true;
    }

    private IEnumerator ClearInput(float time)
    {
        yield return new WaitForSeconds(time);
        clickIndex = 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CastleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform leftUpGroup;

    [SerializeField] private float openTime = 0.15f;
    [SerializeField] private float closedTime = 0.1f;

    [SerializeField] private bool openState = false;
    private bool processing = false;

    public void ChangeState()
    {

    }

    private IEnumerator SetGroupState(bool isState)
    {
        processing = true;

        float time = isState ? openTime : closedTime;
        float newPosition = isState ? -70f : -285f;

        leftUpGroup.DOLocalMoveY(newPosition, time);

        yield return new WaitForSeconds(time);
        processing = false;
    }
}

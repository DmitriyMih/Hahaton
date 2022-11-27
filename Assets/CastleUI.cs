using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform leftUpGroup;

    [SerializeField] private float openTime = 0.15f;
    [SerializeField] private float closedTime = 0.1f;

    [SerializeField] private bool tempLeftGroupOpen = false;
    private bool processing = false;

    private IEnumerator SetGroupState(bool isState)
    {
        processing = true;

        float time = isState ? openTime : closedTime;
        float newPosition = isState ? -70f : -285f;

        //leftUpGroup.DOLocalMoveY(newPosition, time);

        yield return new WaitForSeconds(time);
        processing = false;
    }
}

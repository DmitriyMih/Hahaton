using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class CastleUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float openTime = 0.15f;
    [SerializeField] private float closedTime = 0.1f;

    [SerializeField] private bool openState = false;
    [SerializeField] private bool processing = false;

    [Header("Main Settings")]
    [SerializeField] private Transform objectOnMap;
    [SerializeField] private Image debugImage;

    [SerializeField] private List<RectTransform> buttons = new List<RectTransform>();

    [SerializeField] private Vector2 offcet = new Vector2(25f, 50f);
    [SerializeField] private float offcetYCoefficient = 0.5f;
    [SerializeField] private float dampingValueDifference = 0.2f;

    public void Initialization(Transform castlePosition)
    {
        objectOnMap = castlePosition;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        openState = false;
    }

    public void ChangeState()
    {
        if (processing)
            return;

        bool tempState = !openState;
        //Debug.Log("Change " + tempState);
        StartCoroutine(SetGroupState(tempState));
    }

    [ContextMenu("Debug On Screen")]
    private void DebugOnScreen()
    {
        if (objectOnMap == null || debugImage == null)
            return;

        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectOnMap.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        debugImage.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

    private void Update()
    {
        if (openState)
            DebugOnScreen();
    }

    [ContextMenu("Place In Group")]
    private void PlaceImageInGroup()
    {
        if (buttons.Count == 0)
            return;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].localPosition = Vector2.zero;
        }

        for (int i = -1; i < buttons.Count - 1; i++)
        {
            int index = i + 1;
            float offcetY = (i == -1 || i == buttons.Count - 2) ? offcetYCoefficient : 0f;

            Vector3 newPosition = new Vector3(i * offcet.x, offcet.y - offcet.y * offcetY, 0);
            buttons[index].DOLocalMove(newPosition, openTime);
        }
    }

    private IEnumerator SetGroupState(bool isState)
    {
        processing = true;
        openState = isState;

        float time = isState ? openTime : closedTime;
        float newValue = isState ? 1f : 0f;

        if (canvasGroup != null)
            canvasGroup.DOFade(newValue, time - 0.25f);

        if (buttons.Count != 0)
        {
            for (int i = -1; i < buttons.Count - 1; i++)
            {
                int index = i + 1;
                float offcetY = (i == -1 || i == buttons.Count - 2) ? offcetYCoefficient : 0f;

                Vector3 newPosition = isState ? new Vector3(i * offcet.x, offcet.y - offcet.y * offcetY, 0) : Vector3.zero;
                buttons[index].DOLocalMove(newPosition, time);
                Debug.Log($"Is State - {isState} | Time - {time}");
            }
        }

        yield return new WaitForSeconds(time);
        processing = false;
    }
}

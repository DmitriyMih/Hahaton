using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CastleUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float openTime = 0.15f;
    [SerializeField] private float closedTime = 0.1f;

    [SerializeField] private bool openState = false;
    [SerializeField] private bool processing = false;

    [Header("Main Settings")]
    [SerializeField] private CastleController castleController;
    [SerializeField] private Image debugImage;

    [SerializeField] private List<CastleUiItem> uiItems = new List<CastleUiItem>();

    [SerializeField] private Vector2 offcet = new Vector2(25f, 50f);
    [SerializeField] private float offcetYCoefficient = 0.5f;
    [SerializeField] private float dampingValueDifference = 0.2f;

    [Header("Money View Settings")]
    [SerializeField] private CanvasGroup moneyGroup;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int tempHp = -1;

    [Header("HP Bar Settings")]
    [SerializeField] private CanvasGroup hpBarGroup;
    [SerializeField] private Image hpFill;
    private int tempMoney = -1;

    public void Initialization(CastleController currentCastle)
    {
        castleController = currentCastle;
        openState = false;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = openState;
        }

        for (int i = 0; i < uiItems.Count; i++)
            uiItems[i].InititializationButton(currentCastle);

        if (castleController != null)
        {
            if (hpFill != null)
                hpFill.color = castleController.IsEnemy ? castleController.EnemyColor : castleController.PlayerColor;

            if (moneyGroup != null)
            {
                moneyGroup.alpha = 0f;

                if (!castleController.IsEnemy)
                    moneyGroup.DOFade(1f, openTime);
            }
        }
    }

    public void ChangeState()
    {
        if (processing || castleController == null)
            return;

        bool tempState = !openState;
        StartCoroutine(SetGroupState(tempState));
    }

    [ContextMenu("Debug On Screen")]
    private void DebugOnScreen()
    {
        if (castleController == null || debugImage == null)
            return;

        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(castleController.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        debugImage.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

    private void Update()
    {
        if (openState)
            DebugOnScreen();

        if (castleController != null)
        {
            if (hpBarGroup != null)
                if (castleController.CurrentHp != tempHp)
                    SetHpState(openState);

            if (castleController.IsEnemy)
                return;

            if (castleController.CurrentMoney != tempMoney)
                UpdateMoney();
        }
    }

    private void UpdateMoney()
    {
        if (moneyText == null)
            return;

        tempMoney = castleController.CurrentMoney;
        moneyText.text = castleController.CurrentMoney.ToString();
    }

    private void SetHpState(bool isState)
    {
        float hpBarAlphaValue = isState ? 0f : 1f;
        float time = isState ? openTime : closedTime;

        float fillCoef = 1f / castleController.MaxHp;

        if (hpBarGroup != null && hpFill != null)
            if (castleController.CurrentHp != castleController.MaxHp)
            {
                hpBarGroup.DOFade(hpBarAlphaValue, time - 0.25f);
                hpFill.DOFillAmount(castleController.CurrentHp * fillCoef, time - 0.25f);
                Debug.Log("HP - " + hpFill.fillAmount);
            }
            else
            {
                hpBarGroup.alpha = 0f;
                hpFill.fillAmount = castleController.CurrentHp;
            }

        tempHp = castleController.CurrentHp;
    }

    private IEnumerator SetGroupState(bool isState)
    {
        processing = true;
        openState = isState;

        float time = isState ? openTime : closedTime;
        float buttonsAlphaValue = isState ? 1f : 0f;

        if (canvasGroup != null)
            canvasGroup.DOFade(buttonsAlphaValue, time - 0.25f);

        SetHpState(isState);

        if (uiItems.Count != 0)
        {
            for (int i = -1; i < uiItems.Count - 1; i++)
            {
                int index = i + 1;
                float offcetY = (i == -1 || i == uiItems.Count - 2) ? offcetYCoefficient : 0f;

                Vector3 newPosition = isState ? new Vector3(i * offcet.x, offcet.y - offcet.y * offcetY, 0) : Vector3.zero;
                uiItems[index].transform.DOLocalMove(newPosition, time).OnComplete(() => canvasGroup.interactable = isState); ;
            }
        }

        yield return new WaitForSeconds(time);
        processing = false;
    }
}

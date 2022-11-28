using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private bool isEnemy = true;
    public bool IsEnemy => isEnemy;

    [SerializeField] private int maxHp = 100;
    [SerializeField] private int currentHp;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    [SerializeField] private int currentMoney = 1000;
    public int CurrentMoney => currentMoney;

    [Header("Connect Settings")]
    [SerializeField] private bool isPlayerInteractable = false;
    public bool IsPlayerInteractable => isPlayerInteractable;

    [Header("Interact Settings")]
    [SerializeField] private float tapClearTime = 0.5f;
    private Coroutine clearCoroutine;

    [Header("View Settings")]
    [SerializeField] private Color playerColor = new Color(76f / 255f, 129f / 255f, 237f / 255f, 255f / 255f);
    [SerializeField] private Color enemyColor = new Color(237f / 255f, 76f / 255f, 76f / 255f, 255f / 255f);
    public Color PlayerColor=> playerColor;
    public Color EnemyColor=> enemyColor;

    [SerializeField] private CastleUI castleUI;
    [SerializeField] private float cooldownTime = 0.25f;

    [SerializeField] private int clickIndex = 0;
    private bool isInteractable = true;

    [SerializeField] private PathController path;

    private void Awake()
    {
        currentHp = maxHp;

        if (castleUI != null)
            castleUI.Initialization(this);
    }

    public bool CheckMoneyWithdrawal(int value)
    {
        return currentMoney - value > 0;
    }

    public void WriteOffMoney(int value)
    {
        currentMoney -= value;
    }

    [Header("Waiting Settings")]
    [SerializeField] private int maxWaitingCount = 3;
    [SerializeField] private List<UnitData> creationWaitingList = new List<UnitData>();
    private void Update()
    {
        if (creationWaitingList.Count == 0)
            return;

        if (!createProgress || creationWaitingList.Count == 0)
            CheckBeforeCreating(creationWaitingList[0]);
    }
    #region Create
    public bool CheckCreateList(UnitData unit)
    {
        if (unit == null)
            return false;

        if (creationWaitingList.Count >= maxWaitingCount)
            return false;

        creationWaitingList.Add(unit);
        return true;
    }

    private bool createProgress = false;
    private bool CheckBeforeCreating(UnitData unitData)
    {
        if (path == null)
            return false;

        BaseUnit.Direction tempDirection = IsEnemy ? BaseUnit.Direction.Back : BaseUnit.Direction.Forward;
        PathPoint tempPath = path.GetStartPathPoint(tempDirection);

        if (!tempPath)
            return false;

        StartCoroutine(CreatingUnit(unitData, tempDirection, tempPath));
        return true;
    }

    private IEnumerator CreatingUnit(UnitData unitData, BaseUnit.Direction tempDirection, PathPoint tempPath)
    {
        createProgress = true;
        yield return new WaitForSeconds(unitData.createTime);
        createProgress = false;

        CreateUnit(unitData.unitPrefab, tempDirection, tempPath);
        creationWaitingList.RemoveAt(0);
    }

    private void CreateUnit(BaseUnit unit, BaseUnit.Direction tempDirection, PathPoint tempPath)
    {
        BaseUnit tempUnit = Instantiate(unit);
        tempUnit.InitializationUnit(tempDirection, tempPath);
    }
    #endregion

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
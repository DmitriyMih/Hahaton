using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleUiItem : MonoBehaviour
{
    [SerializeField] private UnitData unitData;
    [SerializeField] private CastleController castleController;

    [Header("View Settings")]
    [SerializeField] private Button createButton;

    public void InititializationButton(CastleController currentCastle)
    {
        if (createButton == null)
            return;

        castleController = currentCastle;
        createButton.onClick.AddListener(() => CreateUnit(unitData));
    }

    public void CreateUnit(UnitData unit)
    {
        if (castleController == null || unit == null)
            return;

        if (!castleController.CheckMoneyWithdrawal(unit.cost))
            return;

        if (castleController.CheckCreateList(unit))
            castleController.WriteOffMoney(unit.cost);
    }
}
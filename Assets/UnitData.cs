using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Units", fileName = "Create New Unit Data")]
public class UnitData
{
    public BaseUnit unitPrefab;
    public int cost;
    public float createTime;
}

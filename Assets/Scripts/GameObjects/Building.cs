using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public class Building : MonoBehaviour
{
    [SerializeField] string builidingName;
    [SerializeField] BuildingMaterial buildingMaterial;

    [SerializeField] BuildingSize buildingSize;
    [SerializeField] int buildingCurHealth;
    [SerializeField] int buildingFullHealth;
    [SerializeField] float buildingRegenTime;
    BuildingBaseStat buildingBaseStat;
    private string buildingHealthExpression;
    private bool isAttackable = true;

    [Button]
    public void Init()
    {
        foreach (Dictionary<string, object> dataDic in GameManager.Instance.dataListDictionary["Building"])
        {
            if (dataDic["Name"].ToString() == builidingName)
            {
                buildingBaseStat = new BuildingBaseStat((BuildingMaterial)Enum.Parse(typeof(BuildingMaterial), dataDic["Material"].ToString()), (BuildingSize)Enum.Parse(typeof(BuildingSize), dataDic["Size"].ToString()), (int)dataDic["Health"], float.Parse(dataDic["RegenTime"].ToString()));
                break;
            }
        }
        buildingMaterial = buildingBaseStat.BaseBuildingMaterial;
        buildingSize = buildingBaseStat.BaseBuildingSize;
        buildingHealthExpression = GameManager.Instance.dataListDictionary["Expressions"][0]["BuildingHealthExpression"].ToString();
        InitGameLevel();
        buildingRegenTime = buildingBaseStat.BaseBuildingRegenTime;
        buildingCurHealth = buildingFullHealth;
    }

    public void OnDamaged(int playerAttack)
    {
        if (isAttackable)
        {
            if (playerAttack > 0)
                Debug.Log("데미지");
            buildingCurHealth -= playerAttack;
            if (buildingCurHealth <= 0)
            {
                DestroyBuilding();
            }
        }
    }

    private void DestroyBuilding()
    {
        isAttackable = false;
        RegenBuilding().Forget();
        gameObject.SetActive(false);
    }

    private async UniTaskVoid RegenBuilding()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(buildingRegenTime));
        gameObject.SetActive(true);
        buildingCurHealth = buildingFullHealth;
        isAttackable = true;
    }

    public void InitGameLevel()
    {
        int healthEval;
        ExpressionEvaluator.Evaluate(string.Format(buildingHealthExpression, buildingBaseStat.BaseBuildingHealth, GameManager.Instance.gameData.GameLevel), out healthEval);
        buildingFullHealth = healthEval;
    }
}

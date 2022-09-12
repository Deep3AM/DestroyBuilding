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
    private Color originalColor;
    private bool isAttackable = true;
    IEnumerator damageCo;

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
        originalColor = GetComponent<Renderer>().material.color;
        damageCo = null;
    }

    public void OnDamaged(int playerAttack)
    {
        if (isAttackable)
        {
            if (playerAttack > 0)
            {
                GameManager.Instance.audios["SFX"].PlayOneShot(GameManager.Instance.audioClips["DM-CGS-48"]);
                if (damageCo != null)
                {
                    StopCoroutine(damageCo);

                    damageCo = DamageColor();
                    StartCoroutine(damageCo);
                }
                else
                {
                    damageCo = DamageColor();
                    StartCoroutine(damageCo);
                }
            }
            buildingCurHealth -= playerAttack;
            if (buildingCurHealth <= 0)
            {
                StopCoroutine(damageCo);
                DestroyBuilding();
            }
        }
    }

    private void DestroyBuilding()
    {
        var render = GetComponent<Renderer>();
        render.material.SetColor("_Color", originalColor);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", originalColor);
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.SetColor("_Color", originalColor);
            }
        }
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

    private IEnumerator DamageColor()
    {
        var render = GetComponent<Renderer>();
        render.material.SetColor("_Color", Color.red);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }
        yield return new WaitForSeconds(0.33f);
        render.material.SetColor("_Color", originalColor);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", originalColor);
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                transform.GetChild(i).GetChild(j).GetComponent<Renderer>().material.SetColor("_Color", originalColor);
            }
        }
    }

    public void InitGameLevel()
    {
        //int healthEval;
        //ExpressionEvaluator.Evaluate(string.Format(buildingHealthExpression, buildingBaseStat.BaseBuildingHealth, GameManager.Instance.gameData.GameLevel), out healthEval);
        //buildingFullHealth = healthEval;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private int playerAttack = 10;
    [SerializeField] private int fullAmmo = 10;
    [SerializeField] private int curAmmo;
    [SerializeField] private float reloadTime = 3f;
    private bool isReloading = false;
    public float Speed { get { return playerSpeed; } set { playerSpeed = value; } }
    public int Attack { get { return playerAttack; } set { playerAttack = value; } }
    public int FullAmmo { get { return fullAmmo; } set { fullAmmo = value; } }
    public int CurAmmo { get { return curAmmo; } set { curAmmo = value; } }
    public float ReloadTime { get { return reloadTime; } set { reloadTime = value; } }
    private PlayerBaseStat playerBaseStat;
    private string playerAttackExpression;
    private string playerAmmoExpression;
    private string playerReloadExpression;
    private void Start()
    {
        Init();
    }
    [Button]
    public void Init()
    {
        List<Dictionary<string, object>> dataList = GameManager.Instance.dataListDictionary["Player"];
        playerBaseStat = new PlayerBaseStat((int)dataList[0]["BaseAttack"], (int)dataList[0]["BaseAmmo"], float.Parse(dataList[0]["BaseReloadTime"].ToString()));
        playerAttackExpression = GameManager.Instance.dataListDictionary["Expressions"][0]["PlayerAttackExpression"].ToString();
        playerAmmoExpression = GameManager.Instance.dataListDictionary["Expressions"][0]["PlayerAmmoExpression"].ToString();
        playerReloadExpression = GameManager.Instance.dataListDictionary["Expressions"][0]["PlayerReloadExpression"].ToString();
        InitPlayerStat(false, false, false);
        curAmmo = fullAmmo;
    }

    public void Move(string arrow)
    {
        if (GameManager.Instance.canRaycastGameObject)
        {
            GameManager.Instance.canRaycastGameObject = false;
        }
        if (arrow.Equals("Up"))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed, Space.World);
        }
        else if (arrow.Equals("Down"))
        {
            transform.Translate(Vector3.back * Time.deltaTime * playerSpeed, Space.World);
        }
        else if (arrow.Equals("Right"))
        {
            transform.Translate(Vector3.right * Time.deltaTime * playerSpeed, Space.World);
        }
        else if (arrow.Equals("Left"))
        {
            transform.Translate(Vector3.left * Time.deltaTime * playerSpeed, Space.World);
        }
    }

    public int Shoot()
    {
        if (curAmmo <= 0)
        {
            if (!isReloading)
                StartCoroutine(Reload());
            return 0;
        }
        curAmmo--;
        return playerAttack;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("재장전 시작");
        yield return new WaitForSeconds(reloadTime);
        curAmmo = fullAmmo;
        isReloading = false;
    }

    public void InitPlayerStat(bool isAttackUp, bool isAmmoUp, bool isReloadUp)
    {
        GameManager.Instance.gameData.PlayerAttackLevel += isAttackUp ? 1 : 0;
        GameManager.Instance.gameData.PlayerAmmoLevel += isAttackUp ? 1 : 0;
        GameManager.Instance.gameData.PlayerReloadLevel += isAttackUp ? 1 : 0;
        Debug.Log(GameManager.Instance.gameData.PlayerAmmoLevel);
        if (isAttackUp || isAmmoUp || isReloadUp)
            GameManager.Instance.Save();



        System.Data.DataTable dt = new System.Data.DataTable();
        int attackEval = (int)dt.Compute(string.Format(playerAttackExpression, playerBaseStat.BaseAttack, GameManager.Instance.gameData.PlayerAttackLevel), "");
        int ammoEval = (int)dt.Compute(string.Format(playerAmmoExpression, playerBaseStat.BaseAmmo, GameManager.Instance.gameData.PlayerAmmoLevel), "");
        Debug.Log(string.Format(playerReloadExpression, playerBaseStat.BaseReloadTime, GameManager.Instance.gameData.PlayerReloadLevel));
        float reloadEval = (float)dt.Compute(string.Format(playerReloadExpression, playerBaseStat.BaseReloadTime, GameManager.Instance.gameData.PlayerReloadLevel), "");

        playerAttack = attackEval;
        fullAmmo = ammoEval;
        reloadTime = reloadEval;
    }
}
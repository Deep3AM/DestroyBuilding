using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DataDownloader
{
    public async UniTask<int> GoogleSheetsDownloader(string url, string dataName)
    {
        try
        {
            bool result;
            UnityWebRequest chk = UnityWebRequest.Head("https://google.com");
            chk.timeout = 5;
            await chk.SendWebRequest();
            result = !(chk.result == UnityWebRequest.Result.ConnectionError || chk.result == UnityWebRequest.Result.DataProcessingError || chk.result == UnityWebRequest.Result.ProtocolError);
            if (result)
            {
                Debug.Log("시트데이터 받아오는 중");
                UnityWebRequest www = UnityWebRequest.Get(url);
                await www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (File.Exists(Application.persistentDataPath + "/" + dataName))
                    {
                        StreamReader originalData = new StreamReader(Application.persistentDataPath + "/" + dataName);
                        if (originalData.ReadToEnd() != www.downloadHandler.text)
                        {
                            Debug.Log("데이터가 다르므로 재다운로드함");
                            originalData.Close();
                            await File.WriteAllBytesAsync(Application.persistentDataPath + "/" + dataName, www.downloadHandler.data);
                        }
                        else
                        {
                            Debug.Log("데이터가 같음");
                        }
                    }
                    else
                    {
                        Debug.Log("파일이 존재하지 않으므로 다운로드 합니다.");
                        await File.WriteAllBytesAsync(Application.persistentDataPath + "/" + dataName, www.downloadHandler.data);
                    }

                }
                if (GameManager.Instance.dataListDictionary.TryAdd(dataName, DataReaders.Read(dataName)))
                {
                    Debug.Log("데이터 불러오기 성공");
                }
            }
            return 1;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return -1;
        }

    }
}

[System.Serializable]
public class GameData
{
    private int playerAttackLevel;
    private int playerAmmoLevel;
    private int playerReloadLevel;
    private int gameLevel;
    public int PlayerAttackLevel { get { return playerAttackLevel; } set { playerAttackLevel = value; } }
    public int PlayerAmmoLevel { get { return playerAmmoLevel; } set { playerAmmoLevel = value; } }
    public int PlayerReloadLevel { get { return playerReloadLevel; } set { playerAmmoLevel = value; } }
    public int GameLevel { get { return gameLevel; } set { gameLevel = value; } }

    public GameData() { }

    public GameData(int _playerAttackLevel, int _playerAmmoLevel, int _playerReloadLevel, int _gameLevel)
    {
        playerAttackLevel = _playerAttackLevel;
        playerAmmoLevel = _playerAmmoLevel;
        playerReloadLevel = _playerReloadLevel;
        gameLevel = _gameLevel;
    }
}

[System.Serializable]
public class PlayerBaseStat
{
    private int baseAttack;
    private int baseAmmo;
    private float baseReloadTime;
    public int BaseAttack { get { return baseAttack; } }
    public int BaseAmmo { get { return baseAmmo; } }
    public float BaseReloadTime { get { return baseReloadTime; } }
    public PlayerBaseStat(int _baseAttack, int _baseAmmo, float _baseReloadTime)
    {
        baseAttack = _baseAttack;
        baseAmmo = _baseAmmo;
        baseReloadTime = _baseReloadTime;
    }
}

[System.Serializable]
public class BuildingBaseStat
{
    private BuildingMaterial baseBuildingMaterial;
    private BuildingSize baseBuildingSize;
    private int baseBuildingHealth;
    private float baseBuildingRegenTime;
    public BuildingMaterial BaseBuildingMaterial { get { return baseBuildingMaterial; } }
    public BuildingSize BaseBuildingSize { get { return baseBuildingSize; } }
    public int BaseBuildingHealth { get { return baseBuildingHealth; } }
    public float BaseBuildingRegenTime { get { return baseBuildingRegenTime; } }
    public BuildingBaseStat(BuildingMaterial _baseBuildingMaterial, BuildingSize _baseBuildingSize, int _baseBuildingHealth, float _baseBuildingRegenTime)
    {
        baseBuildingMaterial = _baseBuildingMaterial;
        baseBuildingSize = _baseBuildingSize;
        baseBuildingHealth = _baseBuildingHealth;
        baseBuildingRegenTime = _baseBuildingRegenTime;
    }
}
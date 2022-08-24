using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Dictionary<string, List<GameObject>> buildingDictionary = new Dictionary<string, List<GameObject>>();
    [SerializeField] private List<GameObject> maps;
    private string curMap = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void OnEnable()
    {
        Debug.Log("sdfdf");
        foreach (GameObject map in maps)
        {
            List<GameObject> tempList = new List<GameObject>();
            foreach (Transform child in map.transform)
            {
                tempList.Add(child.gameObject);
            }
            buildingDictionary.Add(map.name, tempList);
        }
        foreach (string key in buildingDictionary.Keys)
        {
            Debug.Log(key);
        }
        InitMap("TestMap");
    }

    public void InitMap(string mapName)
    {
        if (!string.IsNullOrWhiteSpace(curMap) && buildingDictionary.ContainsKey(curMap))
        {
            foreach (GameObject building in buildingDictionary[curMap])
            {
                Debug.Log("실행");
                building.SetActive(false);
            }
        }
        curMap = mapName;
        //아마도 로딩창?
        foreach (GameObject building in buildingDictionary[mapName])
        {
            building.gameObject.SetActive(true);
            building.GetComponent<Building>()?.Init();
        }
    }

    public void UpgradeMap(int n)
    {
        GameManager.Instance.gameData.GameLevel += n;
        foreach (GameObject building in buildingDictionary[curMap])
        {
            building.GetComponent<Building>()?.InitGameLevel();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance = null;
    public bool canRaycastGameObject = true;
    public ConcurrentDictionary<string, List<Dictionary<string, object>>> dataListDictionary = new ConcurrentDictionary<string, List<Dictionary<string, object>>>();
    public ConcurrentDictionary<string, AudioSource> audios = new ConcurrentDictionary<string, AudioSource>();
    public ConcurrentDictionary<string, AudioClip> audioClips = new ConcurrentDictionary<string, AudioClip>();
    private DataDownloader dataDownloader = new DataDownloader();
    private GameObject loadingPrefab = null;
    private GameObject loadingObject = null;
    public GameData gameData = new GameData();

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    public void Save()
    {
        var settings = new ES3Settings(ES3.EncryptionType.AES, "1DuseopShin");
        ES3.Save("GameData", gameData, settings);
    }

    public void Load()
    {
        var settings = new ES3Settings(ES3.EncryptionType.AES, "1DuseopShin");
        gameData = ES3.Load("GameData", new GameData(1, 1, 1, 1), settings);
    }

    public async UniTask LoadingSceneAsync(List<UniTask> uniTasks)
    {
        if (loadingPrefab == null)
        {
            loadingPrefab = await Resources.LoadAsync<GameObject>("Loading") as GameObject;
        }
        if (loadingObject == null)
        {
            loadingObject = GameObject.Instantiate(loadingPrefab) as GameObject;
            GameObject.DontDestroyOnLoad(loadingObject);
        }

        loadingObject.SetActive(true);
        await loadingObject.GetComponentInChildren<Loading>().LoadingAsync(uniTasks);
        loadingObject.SetActive(false);
    }

    public async UniTask LoadingSceneAsync(List<AsyncOperation> uniTasks)
    {
        if (loadingPrefab == null)
        {
            loadingPrefab = await Resources.LoadAsync<GameObject>("Loading") as GameObject;
        }
        if (loadingObject == null)
        {
            loadingObject = GameObject.Instantiate(loadingPrefab) as GameObject;
            GameObject.DontDestroyOnLoad(loadingObject);
        }
        loadingObject.SetActive(true);
        await loadingObject.GetComponentInChildren<Loading>().LoadingAsync(uniTasks);
        loadingObject.SetActive(false);
    }

    public void OnSceneAsync(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void InitSound()
    {
        if (!audios.ContainsKey("BGM"))
        {
            var bgm = new GameObject();
            bgm.name = "BGM";
            bgm.AddComponent<AudioSource>();
            audios.TryAdd("BGM", bgm.GetComponent<AudioSource>());
            GameObject.DontDestroyOnLoad(bgm);
        }
        if (!audios.ContainsKey("SFX"))
        {
            var sfx = new GameObject();
            sfx.name = "SFX";
            sfx.AddComponent<AudioSource>();
            audios.TryAdd("SFX", sfx.GetComponent<AudioSource>());
            GameObject.DontDestroyOnLoad(sfx);
        }
        var musics = Resources.LoadAll("Musics/", typeof(AudioClip));
        foreach (var music in musics)
        {
            audioClips.TryAdd(music.name, music as AudioClip);
            Debug.Log(music.name);
        }

    }
}

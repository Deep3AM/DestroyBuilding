using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
public class StartScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI touchToStartText;
    [SerializeField] UnityEngine.UI.Button startButton;
    private async void Start()
    {
        DataDownloader dataDownloader = new DataDownloader();
        List<UniTask> uniTasks = new List<UniTask>();
        GameManager.Instance.Load();
        GameManager.Instance.InitSound();
        uniTasks.Add(dataDownloader.GoogleSheetsDownloader("https://docs.google.com/spreadsheets/d/e/2PACX-1vQ-557cepnRoUQUyu0R3tdC46pg8hxjtwPPnBmVrDH05E2YL-iM1zGYSjbVuXQYiXfGoooFLd14mIUV/pub?gid=0&single=true&output=csv", "Building"));
        uniTasks.Add(dataDownloader.GoogleSheetsDownloader("https://docs.google.com/spreadsheets/d/e/2PACX-1vQ-557cepnRoUQUyu0R3tdC46pg8hxjtwPPnBmVrDH05E2YL-iM1zGYSjbVuXQYiXfGoooFLd14mIUV/pub?gid=1603614838&single=true&output=csv", "Player"));
        uniTasks.Add(dataDownloader.GoogleSheetsDownloader("https://docs.google.com/spreadsheets/d/e/2PACX-1vQ-557cepnRoUQUyu0R3tdC46pg8hxjtwPPnBmVrDH05E2YL-iM1zGYSjbVuXQYiXfGoooFLd14mIUV/pub?gid=56949865&single=true&output=csv", "Expressions"));
        await GameManager.Instance.LoadingSceneAsync(uniTasks);
        startButton.onClick.AddListener(() =>
        {
            GameManager.Instance.OnSceneAsync("MainScene");
        });
        touchToStartText.text = "";
        await touchToStartText.DOText("Press Anywhere To Start Game", 2.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        Sequence fade = DOTween.Sequence();
        fade.Append(touchToStartText.DOFade(0, 1.5f));
        fade.Append(touchToStartText.DOFade(1, 2f));
        fade.SetLoops(-1);
        fade.Play();
    }
}

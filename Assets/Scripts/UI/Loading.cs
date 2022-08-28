using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;

public class Loading : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    public async UniTask LoadingAsync(List<UniTask> uniTasks)
    {
        particle.Play();
        foreach (UniTask uniTask in uniTasks)
        {
            await uniTask;
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.25f));
        }
        particle.Stop();
    }
    public async UniTask LoadingAsync(List<AsyncOperation> asyncOperations)
    {
        particle.Play();
        foreach (AsyncOperation async in asyncOperations)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.25f));
        }
        particle.Stop();
    }

}

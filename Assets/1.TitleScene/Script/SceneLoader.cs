using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string nextSceneName;
    public string loadingSceneName;

    void Start()
    {
        SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);
        Debug.Log($"[장시진] 로딩씬 로드중...");
        
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        SceneManager.LoadSceneAsync(nextSceneName);
        Debug.Log($"[장시진] 다음씬 로드중...");

        yield return null;
    }
}

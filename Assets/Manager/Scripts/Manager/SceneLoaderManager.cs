using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ESceneState
{
    _1TitleScene,
    _2IngameScene,
    _3LoadingUI
}

public class SceneLoaderManager : MonoBehaviour
{
    [SerializeField]
    private ESceneState titleSceneName;
    [SerializeField]
    private ESceneState gameSceneName;
    [SerializeField]
    private ESceneState loadingSceneName;
    
    #region Singleton
    public static SceneLoaderManager instance; // SceneLoader Manager을 싱글톤으로 관리
    private void Awake()
    {
        // SceneLoader Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } 
        else
        { 
            Destroy(gameObject);
        }
    }
    #endregion Singleton

    public void LoadTitleScene()
    {
        Debug.Log($"[장시진] 타이틀씬 로드중...");
        SceneManager.LoadSceneAsync(loadingSceneName.ToString(), LoadSceneMode.Single); // 로딩씬 불러오기
        
        StartCoroutine(AsyncLoadTitleScene());
    }
    
    public void LoadGameScene() 
    {
        print($"{loadingSceneName.ToString()}");
        Debug.Log($"[장시진] 게임씬 로드중...");
        SceneManager.LoadSceneAsync(loadingSceneName.ToString(), LoadSceneMode.Single); // 로딩씬 불러오기
        
        StartCoroutine(AsyncLoadGameScene());
    }
    
    private IEnumerator AsyncLoadTitleScene()
    {
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(titleSceneName.ToString());
        Debug.Log($"[장시진] 다음씬 로드중...");

        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator AsyncLoadGameScene()
    {
        print($"{gameSceneName.ToString()}");
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName.ToString());
        Debug.Log($"[장시진] 다음씬 로드중...");

        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

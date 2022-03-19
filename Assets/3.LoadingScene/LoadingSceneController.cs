// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
//
// public class LoadingSceneController : MonoBehaviour
// {
//     private static LoadingSceneController instance;
//
//     public static LoadingSceneController Instance
//     {
//         get
//         {
//             if (instance == null)
//             {
//                 var obj = FindObjectOfType<LoadingSceneController>();
//
//                 if (obj != null)
//                 {
//                     instance = obj;
//                 }
//                 else
//                 {
//                     instance = Create();
//                 }
//             }
//             return instance;
//         }
//     }
//
//     private static LoadingSceneController Create()
//     {
//         return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
//     }
//
//     private void Awake()
//     {
//         if (Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         DontDestroyOnLoad(gameObject);
//     }
//
//     [SerializeField] 
//     private CanvasGroup canvasGroup;
//     
//     [SerializeField]
//     private Image progressBar;
//
//     private string loadSceneName;
//
//     public void LoadScene(string sceneName)
//     {
//         gameObject.SetActive(true);
//         SceneManager.sceneLoaded += OnSceneLoaded;
//         loadSceneName = sceneName;
//         StartCoroutine(LoadSceneProcess());
//     }
//
//     private IEnumerator LoadSceneProcess()
//     {
//         progressBar.fillAmount = 0f;
//         yield return StartCoroutine(Fade(true));
//
//         AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
//         op.allowSceneActivation = false;
//
//         float timer = 0f;
//         while (!op.isDone)
//         {
//             yield return null;
//             
//             if(op.progress < 1.0f)
//                 canvasGroup.transform.Rotate(new Vector3(0,1,0), 1f);
//             else
//                 yield break;
//         }
//     }
//
//     private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
//     {
//         if (arg0.Name == loadSceneName)
//         {
//             StartCoroutine(Fade(false));
//             SceneManager.sceneLoaded -= OnSceneLoaded;
//         }
//     }
//
//     private IEnumerator Fade(bool isFadeIn)
//     {
//         float timer = 0f;
//         while (timer <= 1f)
//         {
//             yield return null;
//             timer += Time.unscaledDeltaTime * 3f;
//             // canvasGroup.transform.Rotate(0f,1f,0f);
//         }
//
//         if (!isFadeIn)
//         {
//             gameObject.SetActive(false);
//         }
//     }
// }

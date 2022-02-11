using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerGameObject;

    // [SerializeField]
    private Vector3 startPlayerPosition;
    
    #region Game Manager 싱글톤 설정
    public static GameManager instance; // Game Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Game Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } 
        else
        {
            // 이미 Game Manager가 존재할 때 오브젝트 파괴 
            Destroy(gameObject);  
        }
    }
    #endregion

    void Start()
    {
    }
    
    void Update()
    {
        
    }
    
    public void InGameTimeStop()
    {
        Time.timeScale = 0;
    }
    
    public void InGameTimeStart()
    {
        Time.timeScale = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static ItemList ItemDB;
    public static ExcelDB ExcelDB;
    
    #region DataManager 싱글톤 설정
    public static DataManager instance; // Game Manager을 싱글톤으로 관리
    private void Awake()
    {
        // DataManager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            // 이미 DataManager가 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);  
        }
    }
    #endregion
}

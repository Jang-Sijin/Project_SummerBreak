using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveDataDictionary
{
    public static Dictionary<string, SaveInfo> saveDataDictionary;
    public static SaveInfo selectSaveData;
}

public class JsonManager : MonoBehaviour
{
    [Header("↓ 현재 설정된 세이브 파일 위치(dataPath)")]
    [SerializeField] private string dataPath = "/SaveFile/savefile.json";
    
    private int saveDataSlotCount = 10;
    private string currentSelectBtnName;
    private string clearSlotName = "빈 슬롯";

    [SerializeField] private SaveInfo loadData;

    #region Singleton
    public static JsonManager instance; // Json Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Json Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
        } 
        else
        { 
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance);
        
        LoadInit();
    }
    #endregion Singleton

    private void Start()
    {
        foreach (var data in SaveDataDictionary.saveDataDictionary)
        {
            print($"{data.Key}, {data.Value}");
            print($"name:{data.Value.name}, position:{data.Value.position}, rotation:{data.Value.rotation}, hp:{data.Value.hp}, stamina:{data.Value.stamina},");
        }
    }

    private void Update()
    {
        print($"{currentSelectBtnName}");
    }

    public void LoadInit()
    {
        string filePath = $"{Application.dataPath + dataPath}";
        FileInfo fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.dataPath + dataPath);
            SaveDataDictionary.saveDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, SaveInfo>>(jdata);
        }
        else
        {
            CreateJsonFile();
        }
    }

    public void Load()
    {
        // 가장 마지막으로 누른 세이브 슬롯의 데이터를 불러온다. // 플레이어 정보 등... 셋팃이 필요
        if (SaveDataDictionary.saveDataDictionary[currentSelectBtnName] != null)
        {
            SaveDataDictionary.selectSaveData = SaveDataDictionary.saveDataDictionary[currentSelectBtnName];
            print($"[장시진] 불러올 세이브 슬롯 이름: {currentSelectBtnName}");
        }
        else
        {
            print("[장시진] 세이브 파일 로드 실패. (선택된 세이브 슬롯의 데이터가 없습니다.)");
        }
    }

    public void Save(string slotName)
    {
        foreach (var data in SaveDataDictionary.saveDataDictionary)
        {
            print($"{data.Key}, {data.Value}");
            print($"name:{data.Value.name}, position:{data.Value.position}, rotation:{data.Value.rotation}, hp:{data.Value.hp}, stamina:{data.Value.stamina},");
        }
        
        print($"{slotName}");
        print($"{SaveDataDictionary.saveDataDictionary[slotName]}");
        if (slotName != null)
        {
            SaveDataDictionary.saveDataDictionary[slotName] = new SaveInfo(
                $"{slotName}",
                GameManager.instance.playerGameObject.transform.position,
                GameManager.instance.playerGameObject.transform.rotation.eulerAngles,
                0,
                0,
                DateTime.Now.ToString("yyyy.MM.dd ") + DateTime.Now.ToString("HH:mm:ss tt ") +
                DateTime.Now.DayOfWeek.ToString().ToUpper().Substring(0, 3));

            // Formatting.Indented -> Json 자동으로 라인/들여쓰기 적용 [참고: https://www.csharpstudy.com/Data/Json-beautifier.aspx]
            string jdata = JsonConvert.SerializeObject(SaveDataDictionary.saveDataDictionary, Formatting.Indented);
            File.WriteAllText(Application.dataPath + dataPath, jdata);
            print($"[장시진] {slotName} Save Success!!!");
            
            SaveSlot currentSaveSlot = GameObject.Find(slotName).GetComponent<SaveSlot>();
            currentSaveSlot.UpdateSlotText();
        }
        else
        {
            print("선택된 세이브 슬롯이 없습니다. 세이브 슬롯을 다시 선택해주세요.");
        }
    }

    // ClearJsonFile 메서드는 Json SaveFile 최초 생성 시 수행해주면 됩니다. 
    // 인게임 UI 세이브 슬롯은 10개로 고정하였으며 변동이 필요할시 for문의 i Count를 바꾼 뒤 해당 메서드를 실행시키면 됩니다.  
    public void CreateJsonFile()
    {
        Dictionary<string, SaveInfo> createData = new Dictionary<string, SaveInfo>();
        
        for (int i = 0; i < saveDataSlotCount; i++)
        {
            // transform은 JsonManager 오브젝트의 transform으로 설정합니다. (필수 확인: JsonManager 오브젝트의 위치(0,0,0), 회전(0,0,0)으로 설정해줘야 합니다.)
            createData[$"SaveSlot ({i})"] = new SaveInfo(clearSlotName, 
                new Vector3(0,0,0), 
                new Vector3(0,0,0), 
                0, 
                0, 
                "");
        }

        string jdata = JsonConvert.SerializeObject(createData, Formatting.Indented);
        File.WriteAllText(Application.dataPath + dataPath, jdata);
        
        print($"[장시진] Save Slot All Clear!!!");
        // print(jdata); // Success Check Print
    }

    public void ClearSlot(string slotName)
    {
        SaveDataDictionary.saveDataDictionary[slotName] = 
            new SaveInfo(clearSlotName, new Vector3(0,0,0), new Vector3(0,0,0), 0, 0, "");
        
        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.saveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.dataPath + dataPath, jdata);
        
        print($"[장시진] {slotName} 슬롯의 데이터를 초기화!!!");
    }
    
    public void SelectSlot(string slotName)
    {
        print($"[장시진] {slotName} 선택되어 JsonManager 전달");
        currentSelectBtnName = slotName;
    }

    public void UpdateSlot(string slotName)
    {
        // dataDictionary에 있는 slotName의 위치의 세이브 데이터를 가져온다.
        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.saveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.dataPath + dataPath, jdata);
        
        print($"[장시진] {slotName} 슬롯의 데이터를 초기화!!!");
    }

    public void CheckLoadJson()
    {
        foreach (var data in SaveDataDictionary.saveDataDictionary)
        {
            print($"{data.Key}, {data.Value}");
            print($"name:{data.Value.name}, position:{data.Value.position}, rotation:{data.Value.rotation}, hp:{data.Value.hp}, stamina:{data.Value.stamina},");
        }
    }
    
    // 세이브 파일을 불러올 때 선택한 세이브 슬롯의 저장 데이터를 반환한다. 선택된 세이브 파일이 없다면 NULL 
    public SaveInfo LoadSaveFile()
    {
        if (SaveDataDictionary.selectSaveData.name.ToString() != clearSlotName && SaveDataDictionary.selectSaveData != null)
        {
            loadData = SaveDataDictionary.selectSaveData;
            
            return SaveDataDictionary.selectSaveData;
        }
        else // selectSaveData == null
        {
            return null;
        }
    }

    // 선택된 세이브 파일이 있는지 확인하는 함수 // 세이브 파일 있음: true, 세이브 파일 없음: false
    public bool CheckSaveFile()
    {
        // 선택된 세이브 파일이 없을 때
        if (SaveDataDictionary.selectSaveData.name.ToString() == clearSlotName || SaveDataDictionary.selectSaveData.name.ToString() == "")
        {
            return false;
        }
        else // 선택된 세이브 파일이 있을 때
        {
            return true;
        }
    }

    public string GetCurrentSelectBtnName()
    {
        return currentSelectBtnName;
    }
}

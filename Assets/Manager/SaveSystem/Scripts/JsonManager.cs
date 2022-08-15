using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveDataDictionary
{
    public static Dictionary<string, SaveInfo> saveDataDictionary = new Dictionary<string, SaveInfo>();
    public static SaveInfo selectSaveData = new SaveInfo();
}

public class JsonManager : MonoBehaviour
{
    [Header("↓ 현재 설정된 세이브 파일 위치(dataPath)")]
    [SerializeField] private string dataPath = "/savefile.json";
    
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
            // DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadInit();
    }
    #endregion Singleton

    public void LoadInit()
    {
        string filePath = $"{Application.persistentDataPath + dataPath}";
        FileInfo fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + dataPath);
            SaveDataDictionary.saveDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, SaveInfo>>(jdata);
        }
        else
        {
            // 새로운 세이브 파일을 생성한다.
            CreateJsonFile();
            
            string jdata = File.ReadAllText(Application.persistentDataPath + dataPath);
            SaveDataDictionary.saveDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, SaveInfo>>(jdata);
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

    public void ResetLoadData()
    {
        // 인게임에서 빠져나올 때 타이틀 씬에서 선택한 슬롯의 내용을 초기화한다.
        SaveDataDictionary.selectSaveData = null;
    }

    public void Save(string slotName, string saveName)
    {
        if (string.IsNullOrEmpty(slotName))
        {
            print("[장시진] 선택된 세이브 슬롯이 없습니다. 세이브 슬롯을 다시 선택해주세요.");
            return;
        }

        print($"[장시진] slotName:{slotName}, saveName:{saveName}");
        print($"[장시진] {SaveDataDictionary.saveDataDictionary[slotName]}");
        
        SaveDataDictionary.saveDataDictionary[slotName] = new SaveInfo(
                saveName,
                DateTime.Now.ToString("yyyy.MM.dd ") + DateTime.Now.ToString("HH:mm:ss tt ") + DateTime.Now.DayOfWeek.ToString().ToUpper().Substring(0, 3),
                GameManager.instance.playerGameObject.transform.position,
                GameManager.instance.playerGameObject.transform.rotation.eulerAngles,
                (int)GameManager.instance.playerGameObject.GetComponent<PlayerStatus>().currentHealth,
                (int)GameManager.instance.playerGameObject.GetComponent<PlayerStatus>().currentMaxstamina,
                (int)GameManager.instance.playerGameObject.GetComponent<PlayerStatus>().currentStamina,
                InventorySystem.instance.playerCoinCount,
                InventorySystem.instance.SaveEquipmentSlot(),
                InventorySystem.instance.SaveInventoryItems(),
                QuestSystem.instance.PlayerProgressQuestID,
                QuestSystem.instance.IsProgressQuest,
                MapPiecesController.instance.landMarkEnable,
                GameManager.instance.saveItemList.SaveMapItemList(),
                GameManager.instance.saveChestBoxList.SaveMapChestBoxList());

        // Formatting.Indented -> Json 자동으로 라인/들여쓰기 적용 [참고: https://www.csharpstudy.com/Data/Json-beautifier.aspx]
        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.saveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + dataPath, jdata);
        print($"[장시진] {slotName}, {saveName} Save Success!!!");
        
        SaveSlot currentSaveSlot = GameObject.Find(slotName).GetComponent<SaveSlot>();
        currentSaveSlot.UpdateSlotText();
    }

    // ClearJsonFile 메서드는 Json SaveFile 최초 생성 시 수행해주면 됩니다. 
    // 인게임 UI 세이브 슬롯은 10개로 고정하였으며 변동이 필요할시 for문의 i Count를 바꾼 뒤 해당 메서드를 실행시키면 됩니다.  
    public void CreateJsonFile()
    {
        Dictionary<string, SaveInfo> createData = new Dictionary<string, SaveInfo>();
        
        for (int i = 0; i < saveDataSlotCount; i++)
        {
            // transform은 JsonManager 오브젝트의 transform으로 설정합니다. (필수 확인: JsonManager 오브젝트의 위치(0,0,0), 회전(0,0,0)으로 설정해줘야 합니다.)
            createData[$"SaveSlot ({i})"] = new SaveInfo(
                clearSlotName,
                "",
                new Vector3(0,0,0), 
                new Vector3(0,0,0), 
                0, 
                0,
                0,
                0,
                null,
                null,
                1,
                false,
                null,
                null,
                null
            );
        }

        string jdata = JsonConvert.SerializeObject(createData, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + dataPath, jdata);
        
        print($"[장시진] Save Slot All Clear!!!");
        // print(jdata); // Success Check Print
    }

    public void ClearSlot(string slotName)
    {
        SaveDataDictionary.saveDataDictionary[slotName] = 
            new SaveInfo(
                clearSlotName, 
                "",
                new Vector3(0,0,0), 
                new Vector3(0,0,0), 
                0, 
                0, 
                0,
                0,
                null,
                null,
                1,
                false,
                null,
                null,
                null
            );
        
        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.saveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + dataPath, jdata);
        
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
        File.WriteAllText(Application.persistentDataPath + dataPath, jdata);
        
        print($"[장시진] {slotName} 슬롯의 데이터를 초기화!!!");
    }

    public void CheckLoadJson()
    {
        foreach (var data in SaveDataDictionary.saveDataDictionary)
        {
            print($"{data.Key}, {data.Value}");
            print($"name:{data.Value.name}, position:{data.Value.position}, rotation:{data.Value.rotation}, hp:{data.Value.hp}, max stamina:{data.Value.maxStamina}, stamina:{data.Value.currentStamina}," +
                  $"{data.Value.saveTime}");
        }
    }
    
    // 세이브 파일을 불러올 때 선택한 세이브 슬롯의 저장 데이터를 반환한다. 선택된 세이브 파일이 없다면 NULL 
    public SaveInfo LoadSaveFile()
    {
        if (SaveDataDictionary.selectSaveData == null || SaveDataDictionary.selectSaveData.name == clearSlotName || string.IsNullOrEmpty(SaveDataDictionary.selectSaveData.name))
        {
            return null;
        } 
        
        // selectSaveData != null
        loadData = SaveDataDictionary.selectSaveData;
        return SaveDataDictionary.selectSaveData;
    }

    // 선택된 세이브 파일이 있는지 확인하는 함수 // 세이브 파일 있음: true, 세이브 파일 없음: false
    public bool CheckSaveFile()
    {
        // print($"[장시진] CheckSaveFile:{SaveDataDictionary.selectSaveData.name}");
        // 선택된 세이브 파일이 없을 때 // null 값 또는 "" 값이있는 문자열을 확인하려면 C#에서 string.IsNullOrEmpty() 메서드를 사용할 수 있습니다. 문자열이 비어 있거나 널이면 true 를 리턴.
        if (SaveDataDictionary.selectSaveData == null || SaveDataDictionary.selectSaveData.name == clearSlotName || string.IsNullOrEmpty(SaveDataDictionary.selectSaveData.name))
        {
            return false;
        }
        
        // 선택된 세이브 파일이 있을 때
        return true;
    }

    public string GetCurrentSelectBtnName()
    {
        return currentSelectBtnName;
    }
}

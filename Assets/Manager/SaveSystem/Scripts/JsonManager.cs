using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// 저장된 게임 데이터를 담는 딕셔너리를 관리
public static class SaveDataDictionary
{
    // 세이브 데이터를 저장하는 딕셔너리 변수
    public static Dictionary<string, SaveInfo> s_SaveDataDictionary = new Dictionary<string, SaveInfo>();
    // 선택된 세이브 데이터를 저장하는 변수
    public static SaveInfo s_SelectSaveData = new SaveInfo();
}

// 클래스는 싱글톤으로 구현되며 게임 저장과 불러오기를 처리
public class JsonManager : MonoBehaviour
{
    // 저장된 JSON 파일의 경로를 저장하는 변수.
    [Header("↓ 현재 설정된 세이브 파일 위치(dataPath)")][SerializeField] private string _dataPath = "/savefile.json";
    [SerializeField] private SaveInfo _loadData;

    // 세이브 슬롯의 최대 개수
    private int _saveDataSlotSize = 10;
    // 현재 선택된 세이브 슬롯의 이름을 저장하는 변수
    private string _currentSelectBtnName;
    // 빈 슬롯의 기본 이름
    private string _clearSlotName = "빈 슬롯";

    #region Singleton
    public static JsonManager Instance; // Json Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Json Manager 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadInit();
    }
    #endregion Singleton

    // 게임을 시작할 때 초기화 작업을 수행
    public void LoadInit()
    {
        string filePath = $"{Application.persistentDataPath + _dataPath}";
        FileInfo fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
        {
            string jdata = File.ReadAllText(Application.persistentDataPath + _dataPath);
            SaveDataDictionary.s_SaveDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, SaveInfo>>(jdata);
        }
        else
        {
            // 새로운 세이브 파일을 생성한다. [슬롯 초기화]
            CreateJsonFile();

            // 세이브 파일 읽어오기.
            string jdata = File.ReadAllText(Application.persistentDataPath + _dataPath);
            SaveDataDictionary.s_SaveDataDictionary = JsonConvert.DeserializeObject<Dictionary<string, SaveInfo>>(jdata);
        }
    }

    // 선택된 세이브 슬롯의 데이터를 불러오기
    public void Load()
    {
        // 가장 마지막으로 누른 세이브 슬롯의 데이터를 불러온다. // 이전에 플레이 했던 플레이어 및 지역 정보를 가져온다.
        if (SaveDataDictionary.s_SaveDataDictionary[_currentSelectBtnName] != null)
        {
            SaveDataDictionary.s_SelectSaveData = SaveDataDictionary.s_SaveDataDictionary[_currentSelectBtnName];
            Debug.Log($"[장시진] 불러올 세이브 슬롯 이름: {_currentSelectBtnName}");
        }
        else
        {
            Debug.Log("[장시진] 세이브 파일 로드 실패.(선택된 세이브 슬롯의 데이터가 없습니다.)");
        }
    }

    // 인게임에서 빠져나올 때 타이틀 씬에서 선택한 슬롯의 내용을 초기화한다
    public void ResetLoadData()
    {
        SaveDataDictionary.s_SelectSaveData = null;
    }

    // 게임 데이터를 선택된 세이브 슬롯에 저장
    public void Save(string slotName, string saveName)
    {
        if (string.IsNullOrEmpty(slotName))
        {
            Debug.Log("[장시진] 선택된 세이브 슬롯이 없습니다. 세이브 슬롯을 다시 선택해주세요.");
            return;
        }

        Debug.Log($"[장시진] slotName:{slotName}, saveName:{saveName}");
        Debug.Log($"[장시진] {SaveDataDictionary.s_SaveDataDictionary[slotName]}");

        SaveDataDictionary.s_SaveDataDictionary[slotName] = new SaveInfo(
                saveName,
                DateTime.Now.ToString("yyyy.MM.dd ") + DateTime.Now.ToString("HH:mm:ss tt ")
                                                     + DateTime.Now.DayOfWeek.ToString().ToUpper().Substring(0, 3),
                GameManager.instance.playerGameObject.transform.position,
                GameManager.instance.playerGameObject.transform.rotation.eulerAngles,
                (int)GameManager.instance.playerGameObject.GetComponent<PlayerStatus>().currentHealth,
                (int)GameManager.instance.playerGameObject.GetComponent<PlayerStatus>().currentMaxstamina,
                (int)GameManager.instance.playerGameObject.GetComponent<PlayerStatus>().currentStamina,
                InventorySystem.instance.playerCoinCount,
                InventorySystem.instance.GetEquipmentSlot(),
                InventorySystem.instance.GetInventoryItems(),
                QuestSystem.instance.PlayerProgressQuestID,
                QuestSystem.instance.IsProgressQuest,
                MapPiecesController.instance.landMarkEnable,
                GameManager.instance.saveItemList.SaveMapItemList(),
                GameManager.instance.saveChestBoxList.SaveMapChestBoxList());

        // Formatting.Indented -> Json 자동으로 라인/들여쓰기 적용 [참고: https://www.csharpstudy.com/Data/Json-beautifier.aspx]
        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.s_SaveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + _dataPath, jdata);
        Debug.Log($"[장시진] {slotName}, {saveName} Save Success!!!");

        SaveSlot currentSaveSlot = GameObject.Find(slotName).GetComponent<SaveSlot>();
        currentSaveSlot.UpdateSlotText();
    }

    // ClearJsonFile 메서드는 Json SaveFile 최초 생성 시 수행해주면 됩니다. 
    // 인게임 UI 세이브 슬롯은 10개로 고정하였으며 변동이 필요할시 for문의 i Count를 바꾼 뒤 해당 메서드를 실행시키면 됩니다.  
    public void CreateJsonFile()
    {
        Dictionary<string, SaveInfo> createData = new Dictionary<string, SaveInfo>();

        for (int i = 0; i < _saveDataSlotSize; i++)
        {
            // transform은 JsonManager 오브젝트의 transform으로 설정합니다. (필수 확인: JsonManager 오브젝트의 위치(0,0,0), 회전(0,0,0)으로 설정해줘야 합니다.)
            createData[$"SaveSlot ({i})"] = new SaveInfo(
                _clearSlotName,
                "",
                Vector3.zero,
                Vector3.zero,
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
        File.WriteAllText(Application.persistentDataPath + _dataPath, jdata);

        Debug.Log($"[장시진] Save Slot All Clear!!!");
        // print(jdata); // Success Check Print
    }

    // 선택된 세이브 슬롯의 데이터를 초기화 [세이브 슬롯 삭제 시 호출]
    public void ClearSlot(string slotName)
    {
        SaveDataDictionary.s_SaveDataDictionary[slotName] =
            new SaveInfo(
                _clearSlotName,
                "",
                Vector3.zero,
                Vector3.zero,
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

        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.s_SaveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + _dataPath, jdata);

        Debug.Log($"[장시진] {slotName} 슬롯의 데이터를 초기화!!!");
    }

    public void SelectSlot(string slotName)
    {
        Debug.Log($"[장시진] {slotName} 선택되어 JsonManager 전달");
        _currentSelectBtnName = slotName;
    }

    public void UpdateSlot(string slotName)
    {
        // dataDictionary에 있는 slotName의 위치의 세이브 데이터를 가져온다.
        string jdata = JsonConvert.SerializeObject(SaveDataDictionary.s_SaveDataDictionary, Formatting.Indented);
        File.WriteAllText(Application.persistentDataPath + _dataPath, jdata);

        Debug.Log($"[장시진] {slotName} 슬롯의 데이터를 초기화!!!");
    }

    public void CheckLoadJson()
    {
        foreach (var data in SaveDataDictionary.s_SaveDataDictionary)
        {
            Debug.Log($"{data.Key}, {data.Value}");
            Debug.Log($"name:{data.Value.name}, position:{data.Value.position}, rotation:{data.Value.rotation}, hp:{data.Value.hp}, max stamina:{data.Value.maxStamina}, stamina:{data.Value.currentStamina}," +
                      $"{data.Value.saveTime}");
        }
    }

    // 세이브 파일을 불러올 때 선택한 세이브 슬롯의 저장 데이터를 반환한다. 선택된 세이브 파일이 없다면 NULL 
    public SaveInfo LoadSaveFile()
    {
        if (SaveDataDictionary.s_SelectSaveData == null || SaveDataDictionary.s_SelectSaveData.name == _clearSlotName || string.IsNullOrEmpty(SaveDataDictionary.s_SelectSaveData.name))
        {
            return null;
        }

        // selectSaveData != null
        _loadData = SaveDataDictionary.s_SelectSaveData;
        return SaveDataDictionary.s_SelectSaveData;
    }

    // 선택된 세이브 파일이 있는지 확인하는 함수 // 세이브 파일 있음: true, 세이브 파일 없음: false
    public bool CheckSaveFile()
    {
        // print($"[장시진] CheckSaveFile:{SaveDataDictionary.selectSaveData.name}");
        // 선택된 세이브 파일이 없을 때 // null 값 또는 "" 값이있는 문자열을 확인하려면 C#에서 string.IsNullOrEmpty() 메서드를 사용할 수 있습니다. 문자열이 비어 있거나 널이면 true 를 리턴.
        if (SaveDataDictionary.s_SelectSaveData == null || SaveDataDictionary.s_SelectSaveData.name == _clearSlotName || string.IsNullOrEmpty(SaveDataDictionary.s_SelectSaveData.name))
        {
            return false;
        }

        // 선택된 세이브 파일이 있을 때
        return true;
    }

    public string GetCurrentSelectBtnName()
    {
        return _currentSelectBtnName;
    }
}
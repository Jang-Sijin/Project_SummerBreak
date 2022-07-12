using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [Header("↓Excel DB Scriptable 오브젝트를 연결해주세요.[차후 Manager(싱글턴)으로 등록할 수 있도록 구조 변경 필요!]")] 
    [SerializeField] private ExcelDB dialogDB; // Import Excel File

    [Header("↓ Quest Accept Animation Canvas 오브젝트를 연결해주세요.")]
    [SerializeField] private GameObject questAcceptAnimeCanvas;

    public QuestAcceptTimeline _QuestAcceptTimeline { get; private set; }

    public int playerProgressQuest { get; set; }

    private List<QuestDBEntity> questList; // Excel QuestDBSheet의 DB리스트

    #region QuestSystem 싱글톤 설정
    public static QuestSystem instance; // QuestManager을 싱글톤으로 관리
    private void Awake()
    {
        // QuestSystem 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(instance);
        } 
        else
        {
            // 이미 QuestSystem가 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);  
        }
    }
    #endregion
    
    private void Start()
    {
        _QuestAcceptTimeline = GetComponentInChildren<QuestAcceptTimeline>();
        
        // Excel QuestDBSheet의 리스트들을 questList에 추가한다.
        questList = dialogDB.QuestDBSheet.ToList();
        
        if (JsonManager.instance.CheckSaveFile() == true)
        {
            // 이전(세이브 파일)에 진행중이었던 퀘스트ID를 할당한다,
        }
        else
        {
            playerProgressQuest = 1; // 타이틀 화면에서 새로운 게임을 선택하였다면 QuestID 1번부터 시작한다. 
        }
    }
    
    // 현재 진행중인 퀘스트ID를 반환한다. 
    public int ReturnProgressQuestDialogID()
    {
        // 현재 진행중인 퀘스트ID에 해당하는 다이얼로그ID를 반환한다.
        return questList.Where(questIterator => questIterator.QuestID == playerProgressQuest)
            .Select(questIterator => questIterator.DialogID).FirstOrDefault();
    }

    public void PrintProgressQuestDB()
    {
        var db = questList.Where(questIterator => questIterator.QuestID == playerProgressQuest).First();
        
        print($"{db.QuestID}, {db.DialogID}, {db.NpcName}, {db.QuestType}, {db.QuestTitle}, {db.QuestContent}, {db.QuestReward}");
    }
}

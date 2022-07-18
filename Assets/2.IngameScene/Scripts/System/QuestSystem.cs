using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [Header("↓Excel DB Scriptable 오브젝트를 연결해주세요.[차후 Manager(싱글턴)으로 등록할 수 있도록 구조 변경 필요!]")] 
    [SerializeField] private ExcelDB dialogDB; // Import Excel File
    
    [Header("↓MenuUI - Quest_Menu를 연결해주세요.")]
    [SerializeField] private QuestMenu _questMenu;

    public QuestAnimationTimeline QuestAnimationTimeline { get; private set; }

    public int playerProgressQuestID { get { return _playerProgressQuestID; } }
    private int _playerProgressQuestID;

    public List<QuestDBEntity> QuestDBList { get { return _questList; } }
    private List<QuestDBEntity> _questList; // Excel QuestDBSheet의 DB리스트
    
    // 퀘스트 대화가 끝났음을 확인하는 변수
    public bool IsQuestDialog
    {
        set { _isQuestDialog = value;}
        get { return _isQuestDialog; }
    }
    private bool _isQuestDialog = false; // false: 퀘스트 대화가 아닐 때, true: 퀘스트 대화일 때

    // 플레이어가 퀘스트를 받아되어 (시작전,진행중) 상태로 변경한다.
    public bool IsProgressQuest
    {
        set { _isProgressQuest = value; }
        get { return _isProgressQuest; }
    }
    private bool _isProgressQuest = false;

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
        Init();
    }

    private void Init()
    {
        QuestAnimationTimeline = GetComponentInChildren<QuestAnimationTimeline>();
        
        // Excel QuestDBSheet의 리스트들을 questList에 추가한다.
        _questList = dialogDB.QuestDBSheet.ToList();
        
        if (JsonManager.instance.CheckSaveFile() == true)
        {
            // 이전(세이브 파일)에 진행중이었던 퀘스트ID를 할당한다,
            // ex) _playerProgressQuestID = 퀘스트ID
        }
        else
        {
            _playerProgressQuestID = 1; // 타이틀 화면에서 새로운 게임을 선택하였다면 QuestID 1번부터 시작한다. 
        }

        // UI - QuestMenu 정보를 QuestSystem에서 초기화를 해준다. // 게임 실행 시 UI가 SetActive False 상태라 자체 초기화가 안되는 현상 발생. 
        _questMenu.Init();
    }

    // 현재 진행중인 퀘스트DB를 반환한다.
    public QuestDBEntity ReturnProgressQuestDB()
    {
        return _questList.Where(questIterator => questIterator.QuestID == _playerProgressQuestID).FirstOrDefault();
    }

    // 퀘스트를 수락하였을 때 콜백
    public void AcceptQuest()
    {
        // 퀘스트 UI List에 있는 QuestSlot을 보이는 상태로 변경시켜준다.
        GameObject questSlot;
        _questMenu.QuestMenuSlotList.TryGetValue(_playerProgressQuestID, out questSlot);
        questSlot.SetActive(true);
        
        // 퀘스트 수락 애니메이션 출력
        QuestAnimationTimeline.OnStartQuestAcceptAnime(ReturnProgressQuestDB().QuestTitle);
        
        // 퀘스트가 시작되어 진행중인 상태로 변경한다.
        _isProgressQuest = true;
        // 퀘스트 다이얼로그가 종료된 상태로 변경한다.
        _isQuestDialog = false;
    }
    
    // 퀘스트를 완료하였을 때 콜백
    public void CompleteQuest()
    {
        // 퀘스트 UI List에 있는 QuestSlot을 클리어 상태로 변경시켜준다.
        GameObject questSlot;
        _questMenu.QuestMenuSlotList.TryGetValue(_playerProgressQuestID, out questSlot);
        questSlot.GetComponent<QuestSlot>().SetCompleteQuestUIActive(true);
        
        // 퀘스트 클리어 애니메이션 출력
        QuestAnimationTimeline.OnStartQuestCompleteAnime(ReturnProgressQuestDB().QuestTitle);
        
        // 퀘스트가 완료되어 퀘스트가 시작 전 상태로 변경한다.
        _isProgressQuest = false;
        // 퀘스트 다이얼로그가 종료된 상태로 변경한다.
        _isQuestDialog = false;
    }

    // 현재 진행중인 퀘스트 DB를 Debug.Log로 출력한다.
    public void PrintProgressQuestDB()
    {
        var db = _questList.Where(questIterator => questIterator.QuestID == _playerProgressQuestID).First();
        
        Debug.Log($"{db.QuestID}, {db.StartDialogID}, {db.EndDialogID}, {db.NpcName}, {db.QuestType}, {db.QuestTitle}, {db.QuestContent}, {db.QuestReward}");
    }

    public void SetNextQuest() => _playerProgressQuestID++;
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [Header("↓Excel DB Scriptable 오브젝트를 연결해주세요.")] 
    [SerializeField] private ExcelDB excelDB; // Import Excel File
    
    [Header("↓MenuUI - Quest_Menu를 연결해주세요.")]
    [SerializeField] private QuestMenu _questMenu;

    public QuestAnimationTimeline QuestAnimationTimeline;

    public int PlayerProgressQuestID { get => _playerProgressQuestID; set { _playerProgressQuestID = value; } }
    private int _playerProgressQuestID;

    public List<QuestDBEntity> QuestDBList => _questList;
    private List<QuestDBEntity> _questList; // Excel QuestDBSheet의 DB리스트

    public QuestCheckTrigger QuestCheckTrigger { get { return _questCheckTrigger; } }
    private QuestCheckTrigger _questCheckTrigger;

    public void LoadQuestData(int setQuestID, bool setIsProgressQuest)
    {
        Init();
        
        _playerProgressQuestID = setQuestID;
        _isProgressQuest = setIsProgressQuest;

        for (int i = 1; i < _playerProgressQuestID; ++i)
        {
            // 퀘스트 UI List에 있는 QuestSlot을 보이는 상태로 변경시켜준다.
            GameObject questSlot;
            _questMenu.QuestMenuSlotList.TryGetValue(i, out questSlot);
            questSlot.SetActive(true);
            questSlot.GetComponent<QuestSlot>().SetCompleteQuestUIActive(true);
        }

        // 퀘스트를 받은 상태이면
        if (_isProgressQuest)
        {
            // 퀘스트 UI List에 있는 QuestSlot을 보이는 상태로 변경시켜준다.
            GameObject questSlot;
            _questMenu.QuestMenuSlotList.TryGetValue(_playerProgressQuestID, out questSlot);
            questSlot.SetActive(true);
            
            // 퀘스트 조건이 실시간으로 체크된다.
            _questCheckTrigger.StartCheckQuest(_playerProgressQuestID,_questList);
        }
    }
    
    // 퀘스트 대화가 끝났음을 확인하는 변수
    public bool IsQuestDialog
    {
        set { _isQuestDialog = value;}
        get { return _isQuestDialog; }
    }
    private bool _isQuestDialog = false; // false: 퀘스트 대화가 아닐 때, true: 퀘스트 대화일 때

    // 플레이어가 퀘스트를 받아되어 (시작전:false ,진행중:true) 상태로 변경한다.
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
            // DontDestroyOnLoad(Instance);
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
        if (!JsonManager.Instance.CheckSaveFile())
        {
            Init();
        }
    }

    private void Init()
    {
        _questCheckTrigger = GetComponent<QuestCheckTrigger>();
        QuestAnimationTimeline = GetComponentInChildren<QuestAnimationTimeline>();
        
        // Excel QuestDBSheet의 리스트들을 questList에 추가한다.
        _questList = excelDB.QuestDBSheet.ToList();
        
        _playerProgressQuestID = 1; // 타이틀 화면에서 새로운 게임을 선택하였다면 QuestID 1번부터 시작한다. 

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
        
        if (questSlot.activeSelf)
            return;
        
        questSlot.SetActive(true);
        
        // 퀘스트 수락 애니메이션 출력
        QuestAnimationTimeline.OnStartQuestAcceptAnime(ReturnProgressQuestDB().QuestTitle);
        
        // 퀘스트가 시작되어 진행중인 상태로 변경한다.
        _isProgressQuest = true;
        // 퀘스트 다이얼로그가 종료된 상태로 변경한다.
        _isQuestDialog = false;
        
        // 진행중인 퀘스트의 완료가 가능한지 조건을 체크한다. (코루틴)
        _questCheckTrigger.StartCheckQuest(_playerProgressQuestID,_questList);
    }
    
    // 퀘스트를 완료하였을 때 콜백
    public void CompleteQuestUI()
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

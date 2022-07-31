using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NpcDialogTrigger : MonoBehaviour
{
    [Header("↓Excel DB Scriptable 오브젝트를 연결해주세요.[차후 Manager(싱글턴)으로 등록할 수 있도록 구조 변경 필요!]")] 
    [SerializeField] private ExcelDB dialogDB; // Import Excel File
    [Header("↓출력을 원하는 다이얼로그 ID 번호를 입력해주세요.")] 
    [SerializeField] private int dialogID; // 대화 출력을 원하는 퀘스트ID 선택
    [Header("↓NPC VirtualCamera 오브젝트를 연결해주세요.")]
    [SerializeField] private GameObject npcVirtualCameraObj;
    [Header("↓NPC Animator가 있는 오브젝트를 연결해주세요.")]
    [SerializeField] private Animator npcAnimator; // Npc 애니메이터
    [Header("↓QuestSystem→Quest_SpeechBubble 오브젝트를 연결해주세요.")]
    [SerializeField] private GameObject showQuestMarkText; // 상호작용이 가능할 때 출력되는 UI Text

    private List<NpcDialogDBEntity> npcDialogList; // ExcelDB에 있는 NPC 다이얼로그 리스트
    public int SaveDialogID { get; set; } // 퀘스트 시작하기 전 DialogID

    // [Header("↓[Debug] 대화 목록 확인용 리스트")]
    // [SerializeField] private SortedDictionary<int, NpcDialogDBEntity> dialogDBList; // 대화 출력을 원하는 퀘스트ID 선택
    // [SerializeField] private List<NpcDialogDBEntity> useDialogList; // 선택한 퀘스트ID의 대사 목록 리스트
    // [SerializeField] private SortedDictionary<int, NpcDialogDBEntity> useDialogList; // 선택한 퀘스트ID의 대사 목록 리스트

    public bool IsExitDialog { get { return _isExitDialog; } }
    private bool _isExitDialog = true; // false: 다이얼로그 실행중, true: 다이얼로그 종료됨.

    private void Awake()
    {
        // 테스트 체크 [디버그]
        // useDialogList = this.dialogDB.DialogSheet.Where(excelDB => excelDB.DialogID == dialogID).ToList();
    }

    private void Start()
    {
        npcDialogList = dialogDB.DialogSheet.Where(excelDB => excelDB.DialogID % dialogID < 100).ToList(); // 예시) DialogID: 11000일 때 11000~11099까지 리스트에 저장
        
        SaveDialogID = dialogID; // 시작 시 DB Sheet에서의 첫 다이얼로그ID를 저장한다.
    }

    private void Update()
    {
        ActiveQuestUI();
    }

    private void ActiveQuestUI()
    {
        if (QuestSystem.instance.IsProgressQuest == false && QuestSystem.instance.ReturnProgressQuestDB().StartDialogID % dialogID < 100)
        {
            // 퀘스트가 가능하다는 UI를 출력한다.
            showQuestMarkText.gameObject.transform.position = this.gameObject.transform.position;
            // UI 색상을 설정한다.
            showQuestMarkText.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
        
            showQuestMarkText.SetActive(true);
        }
        
        if (QuestSystem.instance.IsProgressQuest == true &&
            QuestSystem.instance.ReturnProgressQuestDB().EndDialogID % dialogID < 100 && 
            QuestSystem.instance.QuestCheckTrigger.IsCanComplete == false)
        {
            // 퀘스트가 가능하다는 UI를 출력한다.
            showQuestMarkText.gameObject.transform.position = this.gameObject.transform.position;
            // UI 색상을 설정한다.
            showQuestMarkText.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.grey;
            
            showQuestMarkText.SetActive(true);
        }
        else if (QuestSystem.instance.IsProgressQuest == true &&
                 QuestSystem.instance.ReturnProgressQuestDB().EndDialogID % dialogID < 100 &&
                 QuestSystem.instance.QuestCheckTrigger.IsCanComplete == true)
        {
            // 퀘스트가 가능하다는 UI를 출력한다.
            showQuestMarkText.gameObject.transform.position = this.gameObject.transform.position;
            // UI 색상을 설정한다.
            showQuestMarkText.gameObject.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
            
            showQuestMarkText.SetActive(true);
        }
    }

    public bool ReturnDialogCompulsionCheck()
    {
        if (QuestSystem.instance.IsProgressQuest == false &&
            QuestSystem.instance.ReturnProgressQuestDB().StartDialogID % dialogID < 100)
            return true;
        else
            return false;
    }
    
    
    public void EnterPlayer()
    {
        Debug.Log("[장시진] 대화 시작");
        StartCoroutine("StartDialog");
    }

    private IEnumerator StartDialog()
    { 
        // 1. 퀘스트 대화 (현재 진행중인 퀘스트를 가지고 있는 NPC일 경우)
        if ((QuestSystem.instance.IsProgressQuest == false && QuestSystem.instance.ReturnProgressQuestDB().StartDialogID % dialogID < 100) ||
            (QuestSystem.instance.IsProgressQuest == true && QuestSystem.instance.ReturnProgressQuestDB().EndDialogID % dialogID < 100))
        {
            QuestSystem.instance.PrintProgressQuestDB();
            // 퀘스트 다이얼로그가 시작된 상태로 변경한다.
            QuestSystem.instance.IsQuestDialog = false;

            // 시작 퀘스트일 때
            if (QuestSystem.instance.IsProgressQuest == false &&
                QuestSystem.instance.ReturnProgressQuestDB().StartDialogID % dialogID < 100)
            {
                yield return new WaitUntil(() =>
                    _isExitDialog = DialogSystem.instance.UpdateDialog(npcDialogList
                            .Where(dialogDB => dialogDB.DialogID == QuestSystem.instance.ReturnProgressQuestDB().StartDialogID).ToList(), npcAnimator, npcVirtualCameraObj));
                
            }
            // 완료 퀘스트일 때
            else if (QuestSystem.instance.IsProgressQuest == true &&
                     QuestSystem.instance.ReturnProgressQuestDB().EndDialogID % dialogID < 100 &&
                     QuestSystem.instance.QuestCheckTrigger.IsCanComplete == true)
            {
                yield return new WaitUntil(() =>
                    _isExitDialog = DialogSystem.instance.UpdateDialog(npcDialogList
                        .Where(dialogDB => dialogDB.DialogID == QuestSystem.instance.ReturnProgressQuestDB().EndDialogID).ToList(), npcAnimator, npcVirtualCameraObj));
            }

            // ★다음 다이얼 로그를 출력한다.
           if (_isExitDialog) // true: 대화 끝남, false: 대화중
           {
               if (QuestSystem.instance.IsProgressQuest == false &&
                   QuestSystem.instance.ReturnProgressQuestDB().StartDialogID % dialogID < 100)
               {
                   QuestSystem.instance.IsProgressQuest = true;
                   
                   // 퀘스트 수락 버튼 UI(수락)버튼 출력
                   DialogSystem.instance.DialogUiController.SetActiveButtonObjects(true);
                   Debug.Log("[장시진] 퀘스트 수락");
               }
               else if (QuestSystem.instance.IsProgressQuest == true &&
                        QuestSystem.instance.ReturnProgressQuestDB().EndDialogID % dialogID < 100 &&
                        QuestSystem.instance.QuestCheckTrigger.IsCanComplete == true)
               {
                   // 퀘스트 클리어 시 수락 버튼이 안나오고 다이얼 로그 UI가 종료되도록 한다.  
                   DialogSystem.instance.CloseDialogUi();

                   QuestSystem.instance.QuestCheckTrigger.IsComplete = true;
                   Debug.Log("[장시진] 퀘스트 완료");
               }
               showQuestMarkText.SetActive(false);
           }
        }
        else // 2.일반 대화
        {
            // setQuestID에 해당되는 Dialog 리스트를 매개변수로 보낸다. // Linq
            yield return new WaitUntil(() =>
                _isExitDialog = DialogSystem.instance.UpdateDialog(npcDialogList
                    .Where(dialogDB => dialogDB.DialogID == SaveDialogID).ToList(), npcAnimator, npcVirtualCameraObj));
            
            if (_isExitDialog) // true: 대화 끝남, false: 대화중
            {
                // 일반 대화는 모든 대화 출력 후 다이얼로그 UI를 SetActive false를 해준다.
                // DialogSystem.Instance.CloseObjDialogUi();
                DialogSystem.instance.CloseDialogUi();
                
                // [일반 대화 예외 이벤트] - 상점 NPC
                if (PlayerEventSystem.instance.GetNearGameObject().CompareTag("ShopNpc")) // 상호작용한 NPC가 ShopNpc일 경우
                {
                    ShopSystem.instance.OpenShopCanvas();
                }
                else if (PlayerEventSystem.instance.GetNearGameObject().CompareTag("MoveShopNPC"))
                {
                    MoveShopSystem.instance.OpenShopCanvas();
                }
            }
        }
    }
    
    private void SetNextDialog() // 다이얼로그ID가 퀘스트ID가 되기 전(90번대)이라면 다음 다이얼로그ID로 변경해준다. // [퀘스트 클리어 또는 다음 다이얼로그를 출력하고 싶을 때 해당 함수를 출력한다.]
    {
        var findCount = npcDialogList.Find(entity => entity.DialogID == SaveDialogID + 1).DialogID;
        print($"{findCount}");
        
        // 저장된 기본 다이얼로그 ID값을 1증가시켜 다음 다이얼로그ID로 변경해준다.(퀘스트 다이얼로그 제외)
        if(findCount != 0 && SaveDialogID % dialogID < 90)
            SaveDialogID++;
    }
}

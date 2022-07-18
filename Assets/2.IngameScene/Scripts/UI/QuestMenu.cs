using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class QuestMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questRightPanelTitleName;
    [SerializeField] private TextMeshProUGUI _questRightPanelNpcName;
    [SerializeField] private TextMeshProUGUI _questRightPanelContent;

    [SerializeField] private Transform _questSlotListPanel;
    private Dictionary<int, GameObject> _questMenuSlotList = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> QuestMenuSlotList { get { return _questMenuSlotList; } }

    private const string _prefabRoot = "UI";
    private const string _uiQuestSlotPrefabPath = "Quest/QuestSlot";

    public static Action<QuestSlot> ClickSlot;

    private void Awake()
    {
        ClickSlot = (QuestSlot questSlot) =>
        {
            SelectQuestSlot(questSlot);
        };
    }

    private void Start()
    {
    }

    public void Init()
    {
        foreach (var entity in QuestSystem.instance.QuestDBList)
        {
            Object questSlotPrefabResources = Resources.Load(_prefabRoot + "/" + _uiQuestSlotPrefabPath);
            // 퀘스트 슬롯들을 생성하여 목록(List)에 추가한다. 
            GameObject createSlot = Instantiate(questSlotPrefabResources) as GameObject;
            _questMenuSlotList.Add(entity.QuestID, createSlot);
            createSlot.name = questSlotPrefabResources.name; // 오브젝트명의 (Clone) 삭제
            createSlot.transform.SetParent(_questSlotListPanel, true); // _questSlotListPanel에 Slot을 추가

            // 퀘스트 ID와 Title이름으로 슬롯을 구성한다.
            QuestSlot createQuestSlot = createSlot.GetComponent<QuestSlot>();
            Debug.Log("QuestID:" + entity.QuestID + ", QuestTitle:" + entity.QuestTitle);
            createQuestSlot.Init(entity.QuestID, entity.QuestTitle);
            
            createSlot.SetActive(false); // 초기화할 때 퀘스트 슬롯을 비활성화 시킨다.
        }   
    }

    // 퀘스트 슬롯을 마우스 좌클릭로 눌렀을 때 아래의 업데이트가 필요하다.
    public void SelectQuestSlot(QuestSlot selectQuestSlot)
    {
        foreach (var entity in QuestSystem.instance.QuestDBList)
        {
            if (entity.QuestID + ". " + entity.QuestTitle == selectQuestSlot.SlotQuestTitleName)
            {
                _questRightPanelTitleName.text = entity.QuestTitle;
                _questRightPanelNpcName.text = entity.NpcName;
                _questRightPanelContent.text = entity.QuestContent;
            }
        }
    }
}

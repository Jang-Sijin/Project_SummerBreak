using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    NpcDialogInteraction = 0,
    ObjInterInteraction = 1,
    ItemCheck = 2,
    CoinCheck = 3
}

public class QuestCheckTrigger : MonoBehaviour
{
    // 퀘스트 완료 트리거 //
    // ★Npc Dialog Trigger에서만 변경되는 값입니다.
    public bool IsComplete { get { return _isComplete; } set { _isComplete = value; } }
    private bool _isComplete = false;
    
    // 완료가 가능한 여부(false: 퀘스트 완료 조건에 불충족[완료 불가능], true: 퀘스트 완료 조건에 충족[완료 가능]) //
    // ★해당 클래스에서만 변경되는 값입니다.
    public bool IsCanComplete { get { return _isCanComplete; } }
    private bool _isCanComplete = false;
    
    private QuestDBEntity _progressQuestEntity = new QuestDBEntity();

    [SerializeField] private Item[] itemDB;
    
    public void StartCheckQuest(int progressQuestID, List<QuestDBEntity> questDBList)
    {
        foreach (var entity in questDBList)
        {
            if (entity.QuestID == progressQuestID)
            {
                _progressQuestEntity = entity;
            }
        }


        // QuestProcess
        switch (_progressQuestEntity.QuestType)
        {
            case (int)QuestType.NpcDialogInteraction:
                StartCoroutine(QuestEventHandler_0());
                break;
            case (int)QuestType.ObjInterInteraction:
                StartCoroutine(QuestEventHandler_1());
                break;
            case (int)QuestType.ItemCheck:
                StartCoroutine(QuestEventHandler_2());
                break;
            case (int)QuestType.CoinCheck:
                StartCoroutine(QuestEventHandler_3());
                break;
        }
    }
    
    private void ResetTrigger()
    {
        _isComplete = false;
        _isCanComplete = false;
    }

    private void QuestCompleteProcess()
    {
        // 1. 퀘스트 클리어 처리 UI.
        QuestSystem.instance.CompleteQuestUI();
        // 2. 다음 퀘스트ID를 위한 셋팅을 수행한다.
        QuestSystem.instance.SetNextQuest();
        // 3. 퀘스트 수행과 관련된 트리거 조건들을 초기화한다.
        ResetTrigger();
    }

    private IEnumerator QuestEventHandler_0()
    {
        // 완료가 가능한 조건으로 변경한다. (1. NPC와의 대화만으로 퀘스트가 완료되는 조건) [1. 퀘스트 클리어 가능]
        _isCanComplete = true;
        
        while (true)
        {
            // EndDialogID를 가지고 있는 NPC와 대화하면 퀘스트를 완료한다. [2. 퀘스트 완료 및 보상]
            if (QuestSystem.instance.IsProgressQuest == true && _isComplete == true)
            {
                if (_progressQuestEntity.QuestID == 1)
                {
                    QuestCompleteProcess();
                }
                else if (_progressQuestEntity.QuestID == 4)
                {
                    // [퀘스트 완료 보상]
                    var findItem = Array.Find(itemDB, item => item.itemName == "작은 검");
                    InventorySystem.instance.AcquireItem(findItem, 1);
                    
                    QuestCompleteProcess();
                }

                break;
            }
            
            yield return null;
        }
    }

    private IEnumerator QuestEventHandler_1()
    {

        while (true)
        {
            // 완료가 가능한 조건으로 변경한다. (1. NPC와의 대화만으로 퀘스트가 완료되는 조건) [1. 퀘스트 클리어 가능]
            if (_progressQuestEntity.QuestID == 3)
            {
                // 깃펜을 장비한 상태에서 햇빛마을에 있는 분수대를 지도에 그려보고 촌장 제라드에게 다시 방문하자.
                bool landMarkEnable = MapPiecesController.instance.landMarkEnable[0];
                _isCanComplete = (landMarkEnable == true) ? true : false;
            }
            
            // EndDialogID를 가지고 있는 NPC와 대화하면 퀘스트를 완료한다. [2. 퀘스트 완료 및 보상]
            if (QuestSystem.instance.IsProgressQuest == true && _isComplete == true)
            {
                if (_progressQuestEntity.QuestID == 3)
                {
                    QuestCompleteProcess();
                }

                break;
            }
            
            yield return null;
        }
    }
    
    private IEnumerator QuestEventHandler_2()
    {
        while (true)
        {
            // 완료가 가능한 조건을 체크한다. (1. NPC와의 대화만으로 퀘스트가 완료되는 조건) [1. 퀘스트 클리어 가능]
            if (_progressQuestEntity.QuestID == 2)
            {
                _isCanComplete = InventorySystem.instance.FindInventorySlotItem("깜깜잉크") > 0 ? true : false;
            }
            else if (_progressQuestEntity.QuestID == 5)
            {
                _isCanComplete = InventorySystem.instance.FindInventorySlotItem("솔솔허브") >= 10 ? true : false;
            }
            else if (_progressQuestEntity.QuestID == 6)
            {
                _isCanComplete = (InventorySystem.instance.FindInventorySlotItem("말랑버섯(빨강)") >= 7) && 
                                 (InventorySystem.instance.FindInventorySlotItem("말랑버섯(갈색)") >= 7) ? true : false;
            }
            else if (_progressQuestEntity.QuestID == 7)
            {
                _isCanComplete = InventorySystem.instance.FindInventorySlotItem("민트 불가사리") >= 15 ? true : false;
            }
            else if (_progressQuestEntity.QuestID == 9)
            {
                _isCanComplete = (InventorySystem.instance.FindInventorySlotItem("반짝수정(블루)") >= 3) &&
                                 (InventorySystem.instance.FindInventorySlotItem("반짝수정(퍼플)") >= 3) &&
                                 (InventorySystem.instance.FindInventorySlotItem("반짝수정(그린)") >= 3) ? true : false;
            }
            
            
            // EndDialogID를 가지고 있는 NPC와 대화하면 퀘스트를 완료한다. [2. 퀘스트 완료 및 보상]
            if (QuestSystem.instance.IsProgressQuest == true && _isComplete == true)
            {
                if (_progressQuestEntity.QuestID == 2)
                {
                    // [퀘스트2] : 아무런 보상이 없음.
                    QuestCompleteProcess();
                }
                else if (_progressQuestEntity.QuestID == 5)
                {
                    // [퀘스트5]
                    // [퀘스트 완료를 위한 아이템 소모]
                    InventorySystem.instance.FindSetCountInventorySlotItem("솔솔허브", -10);
                    
                    // [퀘스트 완료 보상]
                    var findItem = Array.Find(itemDB, item => item.itemName == "폭신침낭");
                    InventorySystem.instance.AcquireItem(findItem, 1);
                    
                    QuestCompleteProcess();
                }
                else if (_progressQuestEntity.QuestID == 6)
                {
                    // [퀘스트6]
                    // [퀘스트 완료를 위한 아이템 소모]
                    InventorySystem.instance.FindSetCountInventorySlotItem("말랑버섯(빨강)", -7);
                    InventorySystem.instance.FindSetCountInventorySlotItem("말랑버섯(갈색)", -7);
                    
                    QuestCompleteProcess();
                }
                else if (_progressQuestEntity.QuestID == 7)
                {
                    // [퀘스트7]
                    // [퀘스트 완료를 위한 아이템 소모]
                    InventorySystem.instance.FindSetCountInventorySlotItem("민트 불가사리", -15);
                    
                    QuestCompleteProcess();
                }
                else if (_progressQuestEntity.QuestID == 9)
                {
                    // [퀘스트9]
                    // [퀘스트 완료를 위한 아이템 소모]
                    InventorySystem.instance.FindSetCountInventorySlotItem("반짝수정(블루)", -3);
                    InventorySystem.instance.FindSetCountInventorySlotItem("반짝수정(퍼플)", -3);
                    InventorySystem.instance.FindSetCountInventorySlotItem("반짝수정(그린)", -3);

                    // [퀘스트 완료 보상]
                    var findItem = Array.Find(itemDB, item => item.itemName == "태양석");
                    InventorySystem.instance.AcquireItem(findItem, 2);
                    
                    QuestCompleteProcess();
                }
                
                break;
            }
            yield return null;
        }
    }

    private IEnumerator QuestEventHandler_3()
    {
        while (true)
        {
            // 완료가 가능한 조건을 체크한다. (1. NPC와의 대화만으로 퀘스트가 완료되는 조건) [1. 퀘스트 클리어 가능]
            if (_progressQuestEntity.QuestID == 8)
            {
                _isCanComplete = InventorySystem.instance.playerCoinCount > 250 ? true : false;
            }
            
            // EndDialogID를 가지고 있는 NPC와 대화하면 퀘스트를 완료한다. [2. 퀘스트 완료 및 보상]
            if (QuestSystem.instance.IsProgressQuest == true && _isComplete == true)
            {
                if (_progressQuestEntity.QuestID == 8)
                {
                    // [퀘스트9]
                    // [퀘스트 완료를 위한 재화(코인) 소모]
                    InventorySystem.instance.SetPlayerCoinCount(-250);
                    
                    // [퀘스트 완료 보상]
                    var findItem = Array.Find(itemDB, item => item.itemName == "태양석");
                    InventorySystem.instance.AcquireItem(findItem, 2);

                    QuestCompleteProcess();
                }

                break;
            }
            yield return null;
        }
    }
}

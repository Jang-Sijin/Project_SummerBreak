using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [스크립트 최초 생성자: 장시진]
// 플레이어의 이벤트 처리와 관련된 스크립트입니다.
// 수정을 할 때에는 관련 주석을 달아주시고 내용 앞에는 이름을 적어주시면 편합니다. (누가 구현했는지 확인하기 위해서)
// 예시) [이름] ~을 수행한다.

public class PlayerEventSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionText; // 상호작용이 가능할 때 출력되는 UI Text

    // [키 입력 체크]
    private bool KeyDown; // 버튼(상호작용)
    // [플레이어 근처에 있는 오브젝트]
    public GameObject NearObject { get { return nearObject; } }
    private GameObject nearObject;

    private bool isLandMarkArea = false;
    
    #region Singleton
    public static PlayerEventSystem instance; // PlayerEventSystem 싱글톤으로 관리
    private void Awake()
    {
        // Dialog System 싱글톤 설정
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion Singleton

    private void Update()
    {
        // 키 입력 체크
        GetInput();

        // 상호작용
        Interaction();
    }

    private void GetInput()
    {
        // 상호작용 버튼인 e Key를 눌렀을 때
        KeyDown = Input.GetButtonDown("Interaction");
    }

    private void Interaction()
    {
        if (KeyDown && nearObject != null)
        {
            if (nearObject.CompareTag("QuestNpc"))
            {
                // 다이얼로그 시작 코루틴 시작
                NpcDialogTrigger npcDialogTrigger = nearObject.GetComponent<NpcDialogTrigger>();
                
                npcDialogTrigger.EnterPlayer();
            }
            else if (nearObject.CompareTag("DialogObj"))
            {
                // 다이얼로그 시작 코루틴 시작
                ObjDialogTrigger objDialogTrigger = nearObject.GetComponent<ObjDialogTrigger>();
                
                objDialogTrigger.EnterPlayer();
            }
            else if (nearObject.CompareTag("LandMarkObj"))
            {
                // 다이얼로그 시작 코루틴 시작
                ObjDialogTrigger objDialogTrigger = nearObject.GetComponent<ObjDialogTrigger>();
                objDialogTrigger.EnterPlayer();
                
                PlayerStatus playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
                if (playerStatus.currentItem == PlayerStatus.item.interaction_quillPen &&
                    (InventorySystem.instance.FindInventorySlotItem("깜깜잉크") > 0) &&
                    !nearObject.GetComponent<MapOpenTrigger>().GetMapPieceable())
                {
                    
                    if (nearObject.GetComponent<MapOpenTrigger>().landMarkNumber == 5)
                    {
                        for (int i = 0; i < 4; ++i)
                        {
                            if (MapPiecesController.instance.landMarkEnable[i] == false)
                            {
                                Debug.Log("[이민호] 지도를 다 못채움");
                                return;
                            }
                        }
                    }
                    InventorySystem.instance.FindSetCountInventorySlotItem("깜깜잉크", -1);
                    MapOpenTrigger mapOpenTrigger = nearObject.GetComponent<MapOpenTrigger>();
                    mapOpenTrigger.SetActiveMapPiece();
                }

            }
            else if (nearObject.CompareTag("ShopNpc") || nearObject.CompareTag("MoveShopNPC"))
            {
                // 다이얼로그 시작 코루틴 시작
                NpcDialogTrigger npcDialogTrigger = nearObject.GetComponent<NpcDialogTrigger>();
                npcDialogTrigger.EnterPlayer();
                
                // 상점 UI 출력
                // ShopSystem.Instance.OpenShopCanvas();
            }
            else if (nearObject.CompareTag("ChestObj"))
            {
                // 1.상자 아이템을 여는 함수를 수행한다. -> 지금 상자를 열 수 있는가? 없는가?
                // 2-1.열 수 있으면 애니메이션을 수행시키고 + 코인을 생성한다.
                // 2-2.열 수 없으면 아무것도 수행되지 않는다.
                
                // 상호작용을 누르면 애니메이션 실행.
                // 로직 처리는 어디서 하고 내가 원하는 조건인지 판단을 아예 할 수 없음. 게임 실행 도중에
                
                nearObject.GetComponent<OpenChestCoin>().CheckStateChestBox();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("QuestNpc") || other.CompareTag("DialogObj") || other.CompareTag("ShopNpc") || other.CompareTag("MoveShopNPC") || other.CompareTag("LandMarkObj") || other.CompareTag("ChestObj"))
        {
            // print($"[장시진]: Player-NPC Collider 충돌 성공 -> 상호작용 가능");
            nearObject = other.gameObject;
            
            // 트리거가 발생 UI(E키)를 활성화(출력)한다.
            interactionText.gameObject.SetActive(true);
        }

        if (!isLandMarkArea && other.CompareTag("LandMarkArea"))
        {
            isLandMarkArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Item"))
        {
            // 트리거가 발생 UI(E키)를 비활성화(출력X)한다.
            interactionText.gameObject.SetActive(false);
        }
        
        if (other.CompareTag("QuestNpc"))
        {
            // print($"[장시진]: Player-NPC Collider 충돌 실패 -> 상호작용 불가능");
            
            // 다이얼로그 시작 코루틴 중지
            NpcDialogTrigger npcDialogTrigger = nearObject.GetComponent<NpcDialogTrigger>();
            DialogSystem.instance.ResetDialog(); // Dialog UI 초기화
            npcDialogTrigger.StopCoroutine("StartDialog");
            nearObject = null;
            
            // 트리거가 발생 UI(E키)를 비활성화(출력X)한다.
            interactionText.gameObject.SetActive(false);
        }
        else if (other.CompareTag("DialogObj"))
        {
            // 다이얼로그 시작 코루틴 중지
            ObjDialogTrigger objDialogTrigger = nearObject.GetComponent<ObjDialogTrigger>();
            DialogSystem.instance.ResetDialog(); // Dialog UI 초기화
            objDialogTrigger.StopCoroutine("StartDialog");
            nearObject = null;
            
            // 트리거가 발생 UI(E키)를 비활성화(출력X)한다.
            interactionText.gameObject.SetActive(false);
        }
        else if (other.CompareTag("ShopNpc") || other.CompareTag("MoveShopNPC"))
        {
            // print($"[장시진]: Player-NPC Collider 충돌 실패 -> 상호작용 불가능");
            
            // 다이얼로그 시작 코루틴 중지
            NpcDialogTrigger npcDialogTrigger = nearObject.GetComponent<NpcDialogTrigger>();
            DialogSystem.instance.ResetDialog(); // Dialog UI 초기화
            npcDialogTrigger.StopCoroutine("StartDialog");
            nearObject = null;
            
            // 트리거가 발생 UI(E키)를 비활성화(출력X)한다.
            interactionText.gameObject.SetActive(false);
        }
        else if (other.CompareTag("LandMarkObj"))
        {
            ObjDialogTrigger objDialogTrigger = nearObject.GetComponent<ObjDialogTrigger>();
            DialogSystem.instance.ResetDialog(); // Dialog UI 초기화
            objDialogTrigger.StopCoroutine("StartDialog");
            nearObject = null;
            
            // 트리거가 발생 UI(E키)를 비활성화(출력X)한다.
            interactionText.gameObject.SetActive(false);
        }
        else if (other.CompareTag("ChestObj"))
        {
            nearObject = null;
            interactionText.gameObject.SetActive(false);
        }
        
        if (isLandMarkArea && other.CompareTag("LandMarkArea"))
        {
            isLandMarkArea = false;
        }
    }

    public bool GetIsLandMarkArea()
    {
        return isLandMarkArea;
    }
    
    public GameObject GetNearGameObject()
    {
        return nearObject;
    }
}

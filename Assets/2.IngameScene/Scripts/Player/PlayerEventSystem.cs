using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [스크립트 최초 생성자: 장시진]
// 플레이어의 이벤트 처리와 관련된 스크립트입니다.

public class PlayerEventSystem : MonoBehaviour
{
    // [키 입력 체크]
    private bool eKeyDown; // e버튼(상호작용)
    
    // [플레이어 근처에 있는 오브젝트]
    private GameObject nearObject;

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
        eKeyDown = Input.GetButtonDown("Interaction");
    }

    private void Interaction()
    {
        if (eKeyDown && nearObject != null)
        {
            if (nearObject.CompareTag("QuestNpc"))
            {
                print($"{eKeyDown}");
                // 다이얼로그 시작 코루틴 시작
                NpcDialogTrigger npcDialogTrigger = nearObject.GetComponent<NpcDialogTrigger>();
                npcDialogTrigger.EnterPlayer();
            }
            else if (nearObject.CompareTag("DialogObj"))
            {
                print($"{eKeyDown}");
                // 다이얼로그 시작 코루틴 시작
                ObjDialogTrigger objDialogTrigger = nearObject.GetComponent<ObjDialogTrigger>();
                objDialogTrigger.EnterPlayer();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Item"))
        {
            // 트리거가 발생 UI(E키)를 활성화(출력)한다.
            transform.Find("Player_SpeechBubble").gameObject.SetActive(true);
        }

        if (other.CompareTag("QuestNpc") || other.CompareTag("DialogObj"))
        {
            // print($"[장시진]: Player-NPC Collider 충돌 성공 -> 상호작용 가능");
            nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Item"))
        {
            // 트리거가 발생 UI(E키)를 비활성화(출력X)한다.
            transform.Find("Player_SpeechBubble").gameObject.SetActive(false);
        }

        if (other.CompareTag("QuestNpc"))
        {
            // print($"[장시진]: Player-NPC Collider 충돌 실패 -> 상호작용 불가능");
            
            // 다이얼로그 시작 코루틴 중지
            NpcDialogTrigger npcDialogTrigger = nearObject.GetComponent<NpcDialogTrigger>();
            DialogSystem.instance.ResetDialog(); // Dialog UI 초기화
            npcDialogTrigger.StopCoroutine("StartDialog");
            nearObject = null;
        }
        else if (other.CompareTag("DialogObj"))
        {
            // 다이얼로그 시작 코루틴 중지
            ObjDialogTrigger objDialogTrigger = nearObject.GetComponent<ObjDialogTrigger>();
            DialogSystem.instance.ResetDialog(); // Dialog UI 초기화
            objDialogTrigger.StopCoroutine("StartDialog");
            nearObject = null;
        }
    }
}

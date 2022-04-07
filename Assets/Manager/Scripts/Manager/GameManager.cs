using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // [변수]
    //---------------------------------------------------------------------------------
    
    [Header("플레이어 설정")]
    public GameObject playerGameObject;
    public Transform loadPlayerTransform; // "주의: 디버그 출력용입니다."

    [Header("인벤토리 설정")]
    public GameObject inventorySlotsParent;
    private Slot[] slots;

    [Header("마우스 커서 설정")]
    [SerializeField] private float cursorDisappearTime;
    private float cursorCheckStartTime;
    private Vector3 beforeMousePosition;
    private bool isMoveMouse = true;
    [SerializeField] private Texture2D cursorDefault;
    [SerializeField] private Texture2D cursorClick;

    [Header("인게임 시간 정보")] 
    [SerializeField] private TimeController timeController; 
    //---------------------------------------------------------------------------------
    
    #region Game Manager 싱글톤 설정
    public static GameManager instance; // Game Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Game Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } 
        else
        {
            // 이미 Game Manager가 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);  
        }
    }
    #endregion

    private void Start()
    {
        // 해당 오브젝트의 자식 slot 오브젝트를 slots 배열에 할당한다.
        slots = inventorySlotsParent.GetComponentsInChildren<Slot>();

        // 시작시 마우스 커서 기본으로 설정
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        // 시작시 마우스 위치 현재 마우스 위치로 설정
        beforeMousePosition = Input.mousePosition;
        // 마우스 커서 이미지 할당 (Default:기본)
        StartCoroutine(SetMouseCursor());
    }

    // 인게임 시간 정지
    public void InGameTimeStop() => Time.timeScale = 0;
    // 인게임 시간 시작
    public void InGameTimeStart() => Time.timeScale = 1;

    // 게임 종료
    public void QuitGame() => Application.Quit();

    // 아이템을 획득했을 때 인벤토리에 데이터(아이템)을 저장한다.
    public void AcquireItem(Item item, int count = 1)
    {
        // 장비 아이템이 아닐 경우 수행한다. (장비 아이템은 같은 종류여도 따로 slot에 count를 하도록 구현함.)
        if (Item.ItemType.Equipment != item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null) // 아이템 슬롯에 아이템이 있을 때만 수행한다.
                {
                    if (slots[i].item.itemName == item.itemName) // 중복된 아이템이 있을 때
                    {
                        // 아이템의 개수를 증가시켜준다.
                        slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) // 아이템 슬롯이 비어있을 때만 수행한다.
            {
                slots[i].AddItem(item, count);
                return;
            }
        }
    }

    // 마우스 설정
    private IEnumerator SetMouseCursor()
    {
        while (true)
        {
            // 마우스 이벤트 체크
            EventCheckMouse();
            CheckMouseMove();

            yield return null;
        }
    }

    private void EventCheckMouse()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭을 했을 때
        {
            // 마우스 커서 이미지 cursorClick로 바꿈
            Cursor.SetCursor(cursorClick, Vector2.zero, CursorMode.Auto);
            
            // 이벤트가 있으면 마우스의 움직임이 있다는 것.
            isMoveMouse = true;
        }
        if (Input.GetMouseButtonUp(0)) // 마우스 좌클릭을 뗏을 때
        {
            // 마우스 커서 이미지 cursorDefault 바꿈
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
            
            // 이벤트가 있으면 마우스의 움직임이 있다는 것.
            isMoveMouse = false;
        }
    }

    private void CheckMouseMove()
    {
        if (beforeMousePosition.Equals(Input.mousePosition) && isMoveMouse)
        {
            // 마우스가 움직이고 있지 않음
            isMoveMouse = false;
            
            // 시간 카운트 시작
            cursorCheckStartTime = Time.time;
        }
        else if(!beforeMousePosition.Equals(Input.mousePosition))
        {
            // Debug.Log($"[장시진] 이전 마우스 위치: {beforeMousePosition}, 현재 마우스 위치: {Input.mousePosition}");
            
            // 마우스가 움직이고 있음
            isMoveMouse = true;
            // 마우스의 위치가 이동되었으면 현재 마우스 위치를 받는다. 
            beforeMousePosition = Input.mousePosition;
        }

        // Time.time - cursorCheckStartTime -> 현재 시간과 마우스가 움직이지 않았던 시간을 빼면  
        if (!isMoveMouse && Time.time - cursorCheckStartTime > cursorDisappearTime)
        {
            // Debug.Log($"[장시진] {Time.time - cursorCheckStartTime}초 동안 마우스 이동이 없습니다. (커서 숨김)");
            
            // 마우스를 보이지 않도록 설정한다.
            Cursor.visible = false;
        }
        else if(isMoveMouse)
        {
            Cursor.visible = true;
            cursorCheckStartTime = 0.0f;
        }
    }
    
    public DateTime GetInGameTime()
    {
        return timeController.InGameTime();
    }
}
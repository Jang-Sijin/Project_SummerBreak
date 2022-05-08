using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("마우스 커서 설정")]
    [SerializeField] private float cursorDisappearTime;
    private float cursorCheckStartTime;
    private Vector3 beforeMousePosition;
    private bool isMoveMouse = true;
    [SerializeField] private Texture2D cursorDefault;
    [SerializeField] private Texture2D cursorClick;

    private void Awake()
    {
        // 시작시 마우스 커서 기본으로 설정
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        // 시작시 마우스 위치 현재 마우스 위치로 설정
        beforeMousePosition = Input.mousePosition;
        // 마우스 커서 이미지 할당 (Default:기본)
        StartCoroutine(SetMouseCursor());
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
}

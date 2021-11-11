using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Movement Keys")] // 이동 관련 키
    public KeyCode Jump;            // 글라이딩, 플랩, 클라이밍
    
    [Header("Gameplay keys")]      // 게임 플레이 관련 키
    public KeyCode Use;            // 아이템 사용
    public KeyCode Menu;           // 인벤토리
    public KeyCode Option;         // 게임 옵션

    private float playerBodyMovementSmoothness = 5f; // 플레이어 부드러운 이동 
    
    private void Update()
    {
        MovementDirection(); // 플레이어 이동
    }
    
    public static float horizontalFactor;
    public static float verticalFactor;
    
    public void MovementDirection() // 플레이어 이동
    {
        horizontalFactor = Mathf.Lerp(horizontalFactor, Input.GetAxis("Horizontal"), Time.deltaTime * playerBodyMovementSmoothness);
        verticalFactor = Mathf.Lerp(verticalFactor, Input.GetAxis("Vertical"), Time.deltaTime * playerBodyMovementSmoothness);
    }

    public bool IsJumping() // 뛰기 체크
    {
        return Input.GetKey(Jump);
    }
}

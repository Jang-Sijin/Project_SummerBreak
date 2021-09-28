using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PlayerInputManager : MonoBehaviour
{
 public CharacterController player;
    public PlayerStatus playerstatus;
    
    private Vector2 moveDirection;

    private float flapSpendStamina = 10.0f;
    
    public bool moveDoingCheck;
    public bool JumpDoingCheck;
    public bool FlapDoingCheck;
    public bool clickDoingCheck;
    
    private Stopwatch swMove = new Stopwatch();
    private Stopwatch swSpace = new Stopwatch();
    
    private void Awake()
    {
        moveDoingCheck = false;
    }
    
    // input WASD
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log(context.phase.ToString());   
        }
        else if (context.performed)
        {
            moveDoingCheck = true;
            Debug.Log(context.phase.ToString());
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            swMove.Reset();
            moveDoingCheck = false;
            Debug.Log(context.phase.ToString());
        }
        
    }
    
    // input Space
    public void OnSpace(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!player.isGrounded && playerstatus.currentStamina > 0.0f)
            {
                playerstatus.TakeStamina(flapSpendStamina);
                FlapDoingCheck = true;
                Debug.Log("Flap : " + context.phase.ToString());   
            }
            else if (player.isGrounded)
            {
                JumpDoingCheck = true;
                Debug.Log("Jump : " + context.phase.ToString());
            }
        }
        else if (context.canceled)
        {
            swSpace.Reset();
            FlapDoingCheck = false;
            JumpDoingCheck = false;
            Debug.Log(context.phase.ToString());
        }
    }

    // Getter
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public Stopwatch GetswMove()
    {
        return swMove;
    }

    public Stopwatch GetsSpace()
    {
        return swSpace;
    }
}

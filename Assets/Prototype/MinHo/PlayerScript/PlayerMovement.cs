using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour
{
public enum playerState
    {
        idleState,
        runState,
        jumpState,
        failState,
        flapState,
        glideState
    }
    
    public playerState currentState;
    public PlayerInputManager inputmanager;
    public PlayerStatus playerstatus;
    public CharacterController controller;

    public float curspeed;
    public float jumpHeight;
    public float gravity;
    private Vector3 curVelocity;
    
    public float liftFactor = 0.1f;
    public float dragValue = 0.05f;
    public float forwardSpeed;
    
    
    void Start()
    {
        currentState = playerState.idleState;
        curspeed = playerstatus.walkSpeed;
        gravity = -9.81f;
        jumpHeight = 3.5f;
    }
    // Update is called once per frame
    void Update()
    {
        Idle();
        if (inputmanager.moveDoingCheck)
        {
            inputmanager.GetswMove().Start();
            if (inputmanager.GetswMove().ElapsedMilliseconds >= 1000 && controller.isGrounded)
            {
                curspeed = playerstatus.runSpeed;
                inputmanager.GetswMove().Stop();
            }
            Move(inputmanager.GetMoveDirection(), curspeed);
        }
        else
        {
            curspeed = playerstatus.walkSpeed;
        }

        if (inputmanager.JumpDoingCheck)
        {
            inputmanager.GetswMove().Stop();
            inputmanager.GetsSpace().Start();
            if (inputmanager.GetsSpace().ElapsedMilliseconds >= 1000)
            {
                Glider();
                inputmanager.GetsSpace().Stop();
            }
            else
            {
                Jump();
            }
        }
        else if (inputmanager.FlapDoingCheck)
        {
            inputmanager.GetsSpace().Start();
            if (inputmanager.GetsSpace().ElapsedMilliseconds >= 1000)
            {
                Glider();
                inputmanager.GetsSpace().Stop();
            }
            else
            {
                Flap();
            }
        }
    }

    void Idle()
    {
        currentState = playerState.idleState;
        
        curVelocity.y += gravity * Time.deltaTime;
        controller.Move(curVelocity * Time.deltaTime);
    }

    void Glider()
    {
        currentState = playerState.glideState;
        Debug.Log("글라이딩");
    }
    void Jump()
    {
        currentState = playerState.jumpState;
        Debug.Log("점프");
        curVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
    }

    void Flap()
    {
        currentState = playerState.flapState;
        
        curVelocity.y = Mathf.Sqrt(-1f * jumpHeight * gravity);
    }
    
    void Move(Vector2 direction, float speed)
    {
        currentState = playerState.runState;
        float moveSpeed = speed;

        Vector3 move = new Vector3(inputmanager.GetMoveDirection().x, 0, inputmanager.GetMoveDirection().y);
        transform.LookAt(transform.position + move);
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}

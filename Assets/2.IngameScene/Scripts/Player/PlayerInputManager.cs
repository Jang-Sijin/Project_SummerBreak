using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PlayerInputManager : MonoBehaviour
{
    public PlayerMovement player;
    public PlayerStatus playerstatus;
    //private FreeClimb freeClimb;
    
    private Vector2 moveDirection;

    private float flapSpendStamina = 10.0f;
    
    public bool moveDoingCheck;
    public bool JumpDoingCheck;
    public bool FlapDoingCheck;
    public bool GlideDoingCheck;
    public bool ClimbDoingCheck;
    public bool spaceClickCheck;
    public bool SlidingCheck;
    
    private Stopwatch swMove = new Stopwatch();
    private Stopwatch swSpace = new Stopwatch();

    public bool EnableLog;

    
    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        //freeClimb = GetComponent<FreeClimb>();
        moveDoingCheck = false;
    }

    private void FixedUpdate()
    {
        
        player.currentFrameLerp = false;
        player.currentFrameLerpDirection = Vector3.zero;
        player.IsGround();

        if (player.isGrounded)
        {
            //freeClimb.CancelClimb();
            player.CancelClimb();
            if (GlideDoingCheck)
            {
                GlideDoingCheck = false;
            }
            if (moveDoingCheck)
            {
                player.Move(moveDirection);
            }
            if (JumpDoingCheck)
            {
                player.Jump(moveDirection);
            }

            if (!moveDoingCheck && !JumpDoingCheck)
            {
                player.Ground_Idle();
            }
        }
        else if (player.inWater && !player.isGrounded)
        {
            if (moveDoingCheck)
            {
                player.SwimMove(moveDirection);
            }

            if (JumpDoingCheck)
            {
                player.Jump(moveDirection);
            }

            if (!moveDoingCheck && !JumpDoingCheck)
            {
                player.Swim_idle();
            }
        }
        else if (!player.isGrounded)
        {

            if (ClimbDoingCheck)
            {
                if (moveDoingCheck)
                {
                    Vector2 input = player.SquareToCircle(new Vector2(moveDirection.x, moveDirection.y));
                    Debug.Log($"moveDir{moveDirection}input: {input}");
                    player.Climbing(input);
                }
                else
                {
                    player.Climb_Idle();
                }
            }
            else if (GlideDoingCheck)
            {
            player.Glider(moveDirection, moveDoingCheck);
            } 
            else if(FlapDoingCheck)
            {
                player.Flap();
                FlapDoingCheck = false;
            }
            else if (moveDoingCheck && !GlideDoingCheck)
            {
                player.Failing();
                player.AirMove(moveDirection);
            }
            else if (!GlideDoingCheck)
            {
                player.Failing();
            }
        }
        player.lastFrameLerp = player.currentFrameLerp;       
        player.lastFrameLerpDirection = player.currentFrameLerpDirection;
    }   

    private void Update()
    {
        player.CheckForClimb();
        
        
        if (moveDoingCheck && player.isGrounded)
        {
            swMove.Start();
            if (swMove.ElapsedMilliseconds >= 1000)
            {
                player.curspeed = playerstatus.runSpeed;
                swMove.Stop();
            }
        }
        else
        {
            player.curspeed = playerstatus.walkSpeed;
        }
        
        if (player.isClimbed && spaceClickCheck)
        {
            player.EnterClimb();
            ClimbDoingCheck = true;
            GlideDoingCheck = false;
            JumpDoingCheck = false;
            FlapDoingCheck = false;
            spaceClickCheck = true;
        }
        else if (!player.isClimbed && !GlideDoingCheck && spaceClickCheck)
        {
            swMove.Stop();
            swSpace.Start();
            if (swSpace.ElapsedMilliseconds >= 500)
            {
                ClimbDoingCheck = false;
                GlideDoingCheck = true;
                JumpDoingCheck = false;
                FlapDoingCheck = false;
                spaceClickCheck = true;
                swSpace.Reset();
            }
        }

        if (!ClimbDoingCheck && !spaceClickCheck && player.isClimbed)
        {
            player.Sliding();
            player.CancelClimb();
        }
        
        if (!ClimbDoingCheck && !player.isClimbed)
        {
            ClimbDoingCheck = false;
            player.CancelClimb();
        }

        if (!GlideDoingCheck)
        {
            player.rollAngle = 0.0f;
        }
    }

    // input WASD
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(EnableLog)
                Debug.Log(context.phase.ToString());
        }
        else if (context.performed)
        {
            moveDoingCheck = true;
            moveDirection = context.ReadValue<Vector2>();

            if (EnableLog)
                Debug.Log(context.phase.ToString());
        }
        else if (context.canceled)
        {
            swMove.Reset();
            moveDoingCheck = false;

            if(EnableLog)
                Debug.Log(context.phase.ToString());
        }
        
    }

    // input LeftClick
    public void OnLeftCheck(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (EnableLog)
                Debug.Log(context.phase.ToString());

            if (player.isGrounded && playerstatus.currentItem == PlayerStatus.item.attack)
            {
                player.Attack();
            }
            
        }
        else if (context.canceled)
        {
            if (EnableLog)
                Debug.Log(context.phase.ToString());
        }
    }

    
    // input Space
    public void OnSpace(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (EnableLog)
                Debug.Log(context.phase.ToString());
        }
        else if (context.performed)
        {
            if (EnableLog)
                Debug.Log(context.phase.ToString());
            if (!player.isGrounded && playerstatus.currentStamina > 0.0f)
            {
                //playerstatus.TakeStamina(flapSpendStamina);
                FlapDoingCheck = true;
                spaceClickCheck = true;
                if (EnableLog)
                    Debug.Log("Flap : " + context.phase.ToString());   
            }
            else if (player.isGrounded || (player.inWater))
            {
                JumpDoingCheck = true;
                spaceClickCheck = true;
                GlideDoingCheck = false;
                if (EnableLog)
                    Debug.Log("Jump : " + context.phase.ToString());
            }

        }
        else if (context.canceled)
        {
            swSpace.Reset();
            FlapDoingCheck = false;
            JumpDoingCheck = false;
            GlideDoingCheck = false;
            spaceClickCheck = false;
            ClimbDoingCheck = false;
            if (EnableLog)
                Debug.Log(context.phase.ToString());
        }
    }
}

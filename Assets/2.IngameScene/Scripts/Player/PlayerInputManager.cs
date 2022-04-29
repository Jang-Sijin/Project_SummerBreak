using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerMovement player;
    private PlayerStatus playerstatus;
    //private FreeClimb freeClimb;
    
    private Vector2 moveDirection;

    // private float flapSpendStamina = 10.0f;

    public GameObject GlideTrail_Right;
    public GameObject GlideTrail_Left;
    
    [SerializeField]
    private bool moveDoingCheck;
    [SerializeField]
    private bool JumpDoingCheck;
    [SerializeField]
    private bool FlapDoingCheck;
    [SerializeField]
    private bool GlideDoingCheck;
    [SerializeField]
    private bool ClimbDoingCheck;
    [SerializeField]
    private bool spaceClickCheck;
    [SerializeField]
    private bool SlidingCheck;
    
    private Stopwatch swMove = new Stopwatch();
    private Stopwatch swSpace = new Stopwatch();
    private float flapSpendStamina = 10.0f;
    

    public bool EnableLog;

    
    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        playerstatus = GetComponent<PlayerStatus>();
        //freeClimb = GetComponent<FreeClimb>();
        moveDoingCheck = false;
    }

    private void FixedUpdate()
    {
        player.currentFrameLerp = false;
        player.currentFrameLerpDirection = Vector3.zero;
        player.IsGround();
        player.isSwimed();
        player.Swim_idle();

        if (player.isGrounded)
        {
            //freeClimb.CancelClimb();
            //player.CancelClimb();
            if (GlideDoingCheck)
            {
                GlideDoingCheck = false;
            }
            if (JumpDoingCheck)
            {
                //Debug.Log("[이민호] 육지점프");
                player.Jump(moveDirection);
            }
            else if (moveDoingCheck)
            {
                player.Move(moveDirection);
            }
            else
            {
                player.Ground_Idle();
            }
        }
        else if (player.isSwim && !player.isGrounded)
        {
            if (JumpDoingCheck)
            {
                //Debug.Log("[이민호] 수중점프");
                player.Jump(moveDirection);
            }

            if (!JumpDoingCheck)
            {
                player.SwimMove(moveDirection, moveDoingCheck);
            }
        }
        else if (!player.isGrounded)
        {

            if (ClimbDoingCheck)
            {
                if (moveDoingCheck)
                {
                    Vector2 input = player.SquareToCircle(new Vector2(moveDirection.x, moveDirection.y));
                    //Debug.Log($"moveDir{moveDirection}input: {input}");
                    player.Climbing(input);
                }
                else
                {
                    player.Climb_Idle();
                }
            }
            else if (GlideDoingCheck)
            { 
                GlideTrail_Left.SetActive(true);
                GlideTrail_Right.SetActive(true);
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
        if (player.isClimbedUp)
        {
            spaceClickCheck = false;
            ClimbDoingCheck = false;
        }
        
        if (moveDoingCheck && player.isGrounded)
        {
            swMove.Start();
            if (swMove.ElapsedMilliseconds >= 1000)
            {
                player.curspeed = playerstatus.runSpeed;
                swMove.Stop();
            }
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

        if (!ClimbDoingCheck && !spaceClickCheck && player.isClimbed && !player.isSwim)
        {
            player.Sliding();
            player.CancelClimb();
        }
        
        if (!ClimbDoingCheck && !player.isClimbed && !player.isSwim)
        {
            ClimbDoingCheck = false;
            player.CancelClimb();
        }

        if (!GlideDoingCheck && GlideTrail_Left)
        {
            player.rollAngle = 0.0f;
            GlideTrail_Left.SetActive(false);
            GlideTrail_Right.SetActive(false);
        }

        if (!player.inWater)
        {
            player.isSwim = false;
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
            if (!player.hited && !player.attacked)
            {
                moveDoingCheck = true;
                moveDirection = context.ReadValue<Vector2>();
            }

            if (EnableLog)
                Debug.Log(context.phase.ToString());
        }
        else if (context.canceled)
        {
            swMove.Reset();
            moveDoingCheck = false;
            player.curspeed = playerstatus.walkSpeed;
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

            if (!player.hited && player.isGrounded && playerstatus.currentItem == PlayerStatus.item.attack)
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

            if (!player.hited &&!player.attacked && !player.isSwim && !player.isGrounded && playerstatus.currentStamina > 0.0f)
            {
                if(!playerstatus.DebugMod)
                    playerstatus.TakeStamina(flapSpendStamina);
                
                FlapDoingCheck = true;
                spaceClickCheck = true;
                if (EnableLog)
                    Debug.Log("[이민호] Flap : " + context.phase.ToString());   
            }
            else if (!player.hited && !player.attacked && (player.isGrounded || player.isSwim))
            {
                JumpDoingCheck = true;
                spaceClickCheck = true;
                GlideDoingCheck = false;
                if (EnableLog)
                    Debug.Log("[이민호] Jump : " + context.phase.ToString());
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

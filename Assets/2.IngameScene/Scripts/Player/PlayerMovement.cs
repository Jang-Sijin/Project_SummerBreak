using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour
{
    public enum playerState
    {
        Ground_idleState,
        runState,
        jumpState,
        failState,
        flapState,
        glideState,
        Swim_idleState,
        walkState,
        swimmingState,
        climb_idleState,
        climbing,
        sliding
    }
    
    public playerState currentState;

    public PlayerStatus playerstatus;
    private Rigidbody m_rigidbody;

    public float curspeed;
    [SerializeField]
    private float jumpPower = 5.0f; 
    [SerializeField]
    private float flapPower = 3.0f; 
    [SerializeField]
    float glideGravityMultiplier = 0.01f;
    [SerializeField]
    private float distoGround = 1.5f;
    [SerializeField]
    private float lerpAngleSpeed = 10.0f;
    
    public float gravity= -9.81f;
    private Vector3 curVelocity;
    Vector2 forwardDirection = Vector2.zero;


    public bool isGrounded = false;

    private float startTime = 0.0f;
    private float LerpTime = 0.0f;
    private float lastTime = 0.0f;
    public bool lastFrameLerp;
    public bool currentFrameLerp;
    public Vector3 lastFrameLerpDirection;
    public Vector3 currentFrameLerpDirection;
    private float LerpAngle;
    private Quaternion lerpStartRotation;
    private float lastRollRotation;
    private float rollAngle = 0.0f;
    private bool RLcheck = false;           // false: right, left: true
    
    //Swim
    public float waterSurface, d_fromWaterSurface;
    public bool inWater;
    float swimLevel = 0.25f;

    //Climb
    public bool isClimbed;
    
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        currentState = playerState.Ground_idleState;
        curspeed = playerstatus.walkSpeed;
    }
    
    public void Ground_Idle()
    {
        currentState = playerState.Ground_idleState;
    }

    public void Jump(Vector2 direction)
    {
        forwardDirection = Vector2.zero;
        currentState = playerState.jumpState;
        
        Vector3 jumpDirection = new Vector3(0.0f, jumpPower, 0.0f);
        //Debug.Log("점프함");
        m_rigidbody.AddForce(jumpDirection,ForceMode.Impulse);
    }

    public void Failing()
    {
        if (m_rigidbody.velocity.y < 0)
        {
            currentState = playerState.failState;
        }
    }

    public void Flap()
    {
        forwardDirection = Vector2.zero;
        currentState = playerState.flapState;
        Vector3 jumpDirection = new Vector3(0.0f, flapPower, 0.0f);
        //Dbug.Log("플랩");
        m_rigidbody.AddForce(jumpDirection,ForceMode.Impulse);
    }

    public void AirMove(Vector2 direction)
    {
        forwardDirection = Vector2.zero;
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        curspeed = playerstatus.walkSpeed;
        Vector3 velocity = moveDirection * curspeed;
        velocity.y = m_rigidbody.velocity.y;
        float angle = Mathf.Atan2(moveDirection.x,moveDirection.z) * Mathf.Rad2Deg;
        m_rigidbody.velocity = velocity;
        m_rigidbody.rotation = Quaternion.Euler(0,angle,0);
    }
    
    public void Glider(Vector2 direction, bool moveCheck)
    {
        Vector3 moveDirection = Vector3.zero;
        currentState = playerState.glideState;
        Vector3 LerpTimeDirection;
        //Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        //Debug.Log($"MoveCheck : {moveCheck}");
        if (moveCheck == true)
        {
            moveDirection = new Vector3(direction.x, 0, direction.y);
            
            Vector3 velocity = moveDirection * 3.0f;
            
            velocity.y = gravity * glideGravityMultiplier;
            
            m_rigidbody.velocity = velocity;
            
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            //m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, Time.deltaTime * 3.0f);
            
            // 보간이 되는 중인지 확인
            if (Mathf.Abs(Quaternion.Dot(m_rigidbody.rotation, newRotation)) <= 0.9999f)
            {
                currentFrameLerp = true;
                currentFrameLerpDirection = moveDirection;
                
                bool isFirstFrameOfLerp = !lastFrameLerp && currentFrameLerp;
                bool isKeepLerp = lastFrameLerp && currentFrameLerp;
                bool isMoveDirectionChangedOnLerp = isKeepLerp && (lastFrameLerpDirection != currentFrameLerpDirection);
                
                
                if (isFirstFrameOfLerp || isMoveDirectionChangedOnLerp)
                {
                    startTime = Time.time;
                    Vector3 rigidbodyForward = m_rigidbody.rotation * Vector3.forward;
                    rigidbodyForward.y = 0.0f;
                    rigidbodyForward.Normalize();
                    
                    lerpStartRotation = Quaternion.LookRotation(rigidbodyForward);
                    LerpAngle = Quaternion.Angle(newRotation, lerpStartRotation);
                    
                    LerpTime = LerpAngle / lerpAngleSpeed;

                }


                if (LerpTime > 0)
                {
                    lastRollRotation = m_rigidbody.rotation.eulerAngles.y;
                    float curTime = Time.time - startTime;
                    Vector3 finalRotation = Quaternion.Slerp(lerpStartRotation, newRotation, curTime / LerpTime)
                        .eulerAngles;
                    float increaseOrDecrease = finalRotation.y - lastRollRotation; // 나중방향과 현재방향의 각도의 증감분
                    if (increaseOrDecrease < 0)
                    {
                        RLcheck = false;
                    }
                    else if (increaseOrDecrease > 0)
                    {
                        RLcheck = true;
                    }

                    if (curTime / LerpTime <= 0.5555f)
                    {
                        if (!RLcheck)
                        {
                            rollAngle += 2;
                        }
                        else
                        {   
                            rollAngle -= 2;
                        }
                        
                        //Debug.Log("Roll 회전");
                    }
                    else
                    {
                        if (!RLcheck)
                        {
                            rollAngle -= 2;
                        }
                        else
                        {
                            rollAngle += 2;
                        }
                        //Debug.Log("Roll 원상복구회전");
                    }

                    rollAngle = Mathf.Clamp(rollAngle, -90.0f, 90.0f);
                    finalRotation.z = rollAngle;
                    m_rigidbody.rotation = Quaternion.Euler(finalRotation);
                }
            }
        }
        else
        {
            forwardDirection = Vector2.zero;
            moveDirection = new Vector3(direction.x, 0, direction.y);
            curspeed = 4.0f; // 가속이 붙은 속도
            Vector3 velocity = moveDirection;

            velocity.y = m_rigidbody.velocity.y;
            float lift = Mathf.Cos( Mathf.PI / 4 );
            float drag = Mathf.Sin( Mathf.PI / 4 );
            // 양력과 항력이 적용된 급하강
            m_rigidbody.AddForce(new Vector3(moveDirection.x * Mathf.Abs(m_rigidbody.velocity.y) * drag / lift
                                , m_rigidbody.velocity.y * drag / lift
                                , moveDirection.z * Mathf.Abs(m_rigidbody.velocity.y)) * drag / lift, ForceMode.Acceleration);
            // 중력 가속도만 적용된 급하강
            //m_rigidbody.AddForce(new Vector3(moveDirection.x, m_rigidbody.velocity.y,moveDirection.z),ForceMode.Acceleration);
            Quaternion newRotation = Quaternion.LookRotation(velocity);
            //m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, Time.deltaTime * 5.0f);
            //m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, 1.0f);
            
            //Debug.Log("글라이딩");
        }
    }
    
    
    
    public void Move(Vector2 direction)
    {
        forwardDirection = Vector2.zero;
        if (curspeed != playerstatus.runSpeed)
        {
            currentState = playerState.walkState;
        }
        else
        {
            currentState = playerState.runState;
        }

        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        float angle = Mathf.Atan2(moveDirection.x,moveDirection.z) * Mathf.Rad2Deg;
        m_rigidbody.velocity = moveDirection * curspeed;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation,0.5f);
    }

    public void Swim_idle()
    {
        d_fromWaterSurface = waterSurface - transform.position.y;
        d_fromWaterSurface = Mathf.Clamp(d_fromWaterSurface, float.MinValue, waterSurface);

        if (d_fromWaterSurface >= swimLevel)
        {
            d_fromWaterSurface = swimLevel;
            currentState = playerState.Swim_idleState;
            Vector3 translateWater = new Vector3(m_rigidbody.position.x, waterSurface - d_fromWaterSurface,
                m_rigidbody.position.z);
            m_rigidbody.position = translateWater;
            m_rigidbody.useGravity = false;
            m_rigidbody.velocity = Vector3.zero;
        }

    }

    public void SwimMove(Vector2 direction)
    {
        Swim_idle();

        currentState = playerState.swimmingState;
        
        forwardDirection = Vector2.zero;
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        
        Debug.Log("수중 이동");
        m_rigidbody.AddForce(moveDirection,ForceMode.Impulse);
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation,0.5f);
        
    }



    
    public void IsGround()
    {

        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Water"));
        
        isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, distoGround + 0.1f, layerMask);
        
        //isGrounded = Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, distoGround + 0.1f);
        
        

        if (!isClimbed && (inWater == false || isGrounded))
        {
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
        }
    }
    public void CheckForClimb()
    {
        Vector3 origin = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, 0.6f))
        {
            Debug.Log("sdfa");
            isClimbed = true;
        }
        else
        {
            isClimbed = false;
        }
    }
    public void EnterClimb()
    {
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;

    }

    public void Climb_Idle()
    {
        currentState = playerState.climb_idleState;
    }
    public void CancelClimb()
    {
        isClimbed = false;
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
    }

    public void Sliding()
    {
        currentState = playerState.sliding;
    }

    public void Climbing(Vector2 input)
    {
        Vector3 offset = transform.TransformDirection(Vector2.one * 0.5f);
        Vector3 checkDirection = Vector3.zero;
        int k = 0;

        for (int i = 0; i < 4; i++)
        {
            RaycastHit checkHit;
            if (Physics.Raycast(transform.position + offset,
                transform.forward,
                out checkHit))
            {
                checkDirection += checkHit.normal;
                k++;
            }

            offset = Quaternion.AngleAxis(90f, transform.forward) * offset;
        }

        checkDirection /= k;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -checkDirection, out hit))
        {
            m_rigidbody.isKinematic = false;
            m_rigidbody.position = Vector3.Lerp(m_rigidbody.position, hit.point + hit.normal * 0.5f,
                5f * Time.fixedDeltaTime);
            transform.forward = Vector3.Lerp(transform.forward, -hit.normal, 10f * Time.fixedDeltaTime);

            currentState = playerState.climbing;
            m_rigidbody.velocity = transform.TransformDirection(input) * 2.0f;
        }
        //m_rigidbody.velocity = Vector3.up * 5f + hit.normal * 2f;
    }


    public Vector2 SquareToCircle(Vector2 input)
    {
        return (input.sqrMagnitude >= 1f) ? input.normalized : input;
    }

}

using System;
using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.VFX;

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
        sliding,
        attack,
        hit
    }
    
    public playerState currentState;
    public VisualEffect walkEffect;
    public VisualEffect jumpEffect;
    public VisualEffect flapEffect;

    private PlayerStatus playerstatus;
    private Rigidbody m_rigidbody;

    public Animator _animator;
    
    public float curspeed;
    [SerializeField]
    private float jumpPower = 5.0f; 
    [SerializeField]
    private float flapPower = 3.0f; 
    [SerializeField]
    float glideGravityMultiplier = 0.01f;
    [SerializeField]
    private float lerpAngleSpeed = 10.0f;
    
    private float gravity= -9.81f;


    public bool isGrounded = false;
    [SerializeField] 
    private bool isSlope;

    private float startTime = 0.0f;
    private float LerpTime = 0.0f;
    // private float lastTime = 0.0f;
    public bool lastFrameLerp;
    public bool currentFrameLerp;
    public Vector3 lastFrameLerpDirection;
    public Vector3 currentFrameLerpDirection;
    private float LerpAngle;
    private Quaternion lerpStartRotation;
    private float lastRollRotation;
    public float rollAngle = 0.0f;
    private bool RLcheck = false;           // false: right, left: true
    public bool attacked = false;
    public bool hited = false;
    
    //Swim
    public float waterSurface;
    public float d_fromWaterSurface;
    public bool inWater;
    public bool isSwim;
    public float swimLevel = 0.6f;

    //Climb
    public bool isClimbed;
    
    //Slope
    private float maxSlopeAngle = 50.0f;
    private RaycastHit slopeHit;
    
    //Hit
    public SkinnedMeshRenderer bodyRenderer;
    public SkinnedMeshRenderer capeRenderer;
    private Material bodyMaterial;
    private Material capeMaterial;
    [SerializeField] 
    private bool invincible = false;
    public GameObject respawnPoint;
    
    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        playerstatus = GetComponent<PlayerStatus>();
        currentState = playerState.Ground_idleState;
        curspeed = playerstatus.walkSpeed;
        bodyMaterial = bodyRenderer.material;
        capeMaterial = capeRenderer.material;
        bodyMaterial.SetFloat("RedLv", 0.0f);
        capeMaterial.SetFloat("RedLv",0.0f);
    }

    public void Ground_Idle()
    {
        if (currentState != playerState.hit)
        {
            currentState = playerState.Ground_idleState;

            m_rigidbody.velocity = m_rigidbody.velocity * 0.1f;
        }
    }

    public void Jump(Vector2 direction)
    {
            currentState = playerState.jumpState;
            Vector3 jumpDirection = new Vector3(0.0f, jumpPower, 0.0f);
            jumpEffect.Play();
            Debug.Log("[이민호] 점프함");
            m_rigidbody.AddForce(jumpDirection, ForceMode.Impulse);
    }

    public void Move(Vector2 direction)
    {
        if (curspeed != playerstatus.runSpeed)
        {
            currentState = playerState.walkState;
        }
        else
        {
            currentState = playerState.runState;
        }
        Vector3 moveDirection = new Vector3(direction.x, 0.0f, direction.y);
        if (isSlope)
        {
            
            m_rigidbody.velocity = GetSlopeMoveDirection(moveDirection) * curspeed;
        }
        else
        {
            m_rigidbody.velocity = moveDirection * curspeed;
        }
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation,0.5f);
        walkEffect.Play();
    }

    public void Failing()
    {
        if (!isClimbed && m_rigidbody.velocity.y < 0)
        {
            currentState = playerState.failState;
        }
    }

    public void Flap()
    {
        curspeed = playerstatus.walkSpeed;
        currentState = playerState.flapState;
        _animator.Rebind();
        _animator.Play("Flap");
        flapEffect.Reinit();
        flapEffect.Play();
        Vector3 jumpDirection = new Vector3(0.0f, flapPower, 0.0f);
        Debug.Log("[이민호] 플랩");
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.AddForce(jumpDirection,ForceMode.Impulse);
    }

    public void AirMove(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
        Vector3 velocity = moveDirection * curspeed;
        velocity.y = m_rigidbody.velocity.y;
        float angle = Mathf.Atan2(moveDirection.x,moveDirection.z) * Mathf.Rad2Deg;
        //Debug.Log("[이민호] 공중이동");
        m_rigidbody.velocity = velocity;
        m_rigidbody.rotation = Quaternion.Euler(0,angle,0);
    }
    
    public void Glider(Vector2 direction, bool moveCheck)
    {
        curspeed = playerstatus.walkSpeed;
        Vector3 moveDirection = Vector3.zero;
        currentState = playerState.glideState;
        // Vector3 LerpTimeDirection;
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
            //Debug.Log($"rigid:{m_rigidbody.rotation}, new:{newRotation}");
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

    public void isSwimed()
    {
        if (inWater)
        {
            d_fromWaterSurface = waterSurface - transform.position.y;
            d_fromWaterSurface = Mathf.Clamp(d_fromWaterSurface, float.MinValue, waterSurface);
            isSwim = d_fromWaterSurface >= swimLevel;
        }
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

    public void SwimMove(Vector2 direction, bool moveCheck)
    {
        if (!isSwim)
        {
            Swim_idle();
        }

        if (moveCheck == true)
        {
            currentState = playerState.swimmingState;

            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

            m_rigidbody.AddForce(moveDirection, ForceMode.Impulse);
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, 0.5f);
        }
    }


    public void Attack()
    {
        if (playerstatus.currentItem == PlayerStatus.item.attack)
        {
            m_rigidbody.velocity = Vector3.zero;
            currentState = playerState.attack;
        }
    }

    
    public void IsGround()
    {
        //Debug.DrawRay(transform.position, -Vector3.up, Color.red);
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Water"));
        OnSlope();
        //isGrounded = Physics.Raycast(transform.position, -Vector3.up, layerMask);
        //isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), distoGround, layerMask);
        //Debug.DrawLine(transform.position,transform.position - Vector3.up * 0.1f,Color.red,1.0f);
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.1f, layerMask) ||
                     isSlope;

        if (!isClimbed && (inWater == false || isGrounded))
        {
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
        }
    }
    
    

    
    private void OnSlope()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("Water"));
        
        //Debug.DrawLine(transform.position,transform.position - Vector3.up * 0.3f,Color.red,0.5f);
        if (Physics.Raycast(transform.position, -Vector3.up, out slopeHit, 
            0.1f,layerMask))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            isSlope = angle < maxSlopeAngle && angle != 0;
            if (isSlope)
            {
                //Debug.Log($"[이민호] SlopeAngle:{angle}");
            }
        }
        else
        {
            //Debug.Log("[이민호] 아님!");
            isSlope = false;
        }
    }

    private Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    
    public void CheckForClimb()
    {
            Vector3 origin = new Vector3(transform.position.x,transform.position.y + 0.7f, transform.position.z);
            //Debug.DrawLine(origin,origin + Vector3.forward * 1.0f,Color.red,1.0f);
            RaycastHit hit;
            if (!isGrounded && Physics.Raycast(origin, transform.forward, out hit, 1.0f))
            {
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
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
    }

    public void Sliding()
    {
        if (!isGrounded && isClimbed)
        {
            currentState = playerState.sliding;
        }
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

            offset = Quaternion.AngleAxis(80f, transform.forward) * offset;
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
    
    public void HitStart(float damageValue, Rigidbody monsterRigidbody)
    {
        if (playerstatus.GetCurHealth() <= 0)
        {
            hited = false;
            invincible = false;
            bodyMaterial.SetFloat("RedLv", 0.0f);
            capeMaterial.SetFloat("RedLv",0.0f);
            this.transform.position = respawnPoint.transform.position;
            playerstatus.ReSetCurHealth();
        }
        else if (!invincible)
        {
            hited = true;
            invincible = true;
            bodyMaterial.SetFloat("RedLv", 0.1f);
            capeMaterial.SetFloat("RedLv",0.1f);
            playerstatus.HitHealth(damageValue);
            currentState = playerState.hit;
            StartCoroutine(HitInvincible());
            StartCoroutine(HitBlink());
            Vector3 differnce = transform.position - monsterRigidbody.transform.position;
            differnce = differnce.normalized * 5.0f;
            m_rigidbody.AddForce(differnce,ForceMode.Impulse);
        }
    }
    
    IEnumerator HitInvincible()
    {
        yield return new WaitForSeconds(2.0f);
        invincible = false;
    }
    IEnumerator HitBlink()
    {
        while (invincible)
        {
            yield return new WaitForSeconds(0.2f);
            bodyRenderer.enabled = false;
            capeRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            bodyRenderer.enabled = true;
            capeRenderer.enabled = true;
        }
    }
    
    public void HitEnd()
    {
        hited = false;
        bodyMaterial.SetFloat("RedLv", 0.0f);
        capeMaterial.SetFloat("RedLv",0.0f);
        currentState = playerState.Ground_idleState;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using UnityEngine.Playables;
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
        hit,
        die
    }
    
    public playerState currentState;
    
    public VisualEffect jumpEffect;
    public VisualEffect flapEffect;

    private PlayerStatus playerstatus;
    private Rigidbody m_rigidbody;

    public Animator _animator;
    
    public float curspeed;
    public float swimSpeed = 3.0f;
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
    [SerializeField] 
    private GameObject waterSplash;
    public float waterSurface;
    public float d_fromWaterSurface;
    public bool inWater;
    public bool isSwim;
    public float swimLevel = 0.6f;

    //Climb
    public bool isClimbed;
    public bool isClimbedUp;
    public Vector3 checkDirection = Vector3.zero;
    public bool climbFlap = false;
    [SerializeField]
    
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
    
    [SerializeField] private GameObject opitionUi;
    
    private bool glideLiftDrag = true;
    private bool glideQuater = true;
    
    [SerializeField] private GameObject LiftDragTextObj;
    [SerializeField] private GameObject QuaterTextObj;

    public bool slidingCheck = false;

    public bool checkMonsterFollow = false;

    [SerializeField] private float climpSpeed = 3.5f;

    [SerializeField] 
    private bool isDead = false;

    [SerializeField] 
    private GameObject reSpawnScene;
    
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
        if (currentState != playerState.hit || currentState != playerState.die)
        {
            currentState = playerState.Ground_idleState;
            isClimbedUp = false;
            climbFlap = false;
            checkDirection = Vector3.zero;
            m_rigidbody.velocity = m_rigidbody.velocity * 0.1f;
        }
    }

    public void SetDebugMod(bool value)
    {
        glideQuater = value;
        glideLiftDrag = value;
    }
    
    public void Jump(Vector2 direction)
    {
            currentState = playerState.jumpState;
            Vector3 jumpDirection = new Vector3(0.0f, jumpPower, 0.0f);
            if (isSwim)
            {
                ParticleSystem particleSystem = waterSplash.GetComponent<ParticleSystem>();
                particleSystem.Play();
            }
            else if(playerstatus.playerInPeak)
            {
                flapEffect.Reinit();
                flapEffect.Play();
            }
            else
            {
                jumpEffect.Play();
            }
            //Debug.Log("[이민호] 점프함");
            m_rigidbody.AddForce(jumpDirection, ForceMode.Impulse);
    }

    public void Move(Vector2 direction)
    {
        isClimbedUp = false;
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
    }

    public void Failing()
    {
        if (!isClimbed && m_rigidbody.velocity.y < 0)
        {
            currentState = playerState.failState;
            climbFlap = false;
            isClimbedUp = false;
        }
    }

    public void Flap()
    {
        isClimbedUp = false;
        curspeed = playerstatus.walkSpeed;
        currentState = playerState.flapState;
        _animator.Rebind();
        _animator.Play("Flap");
        flapEffect.Reinit();
        flapEffect.Play();
        SoundManager.Instance.PlaySFX(8);
        Vector3 jumpDirection = new Vector3(0.0f, flapPower, 0.0f);
        if (playerstatus.GetDebugMod() == false)
        {
            playerstatus.TakeStamina(10.0f);
        }

        //Debug.Log("[이민호] 플랩");
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.AddForce(jumpDirection,ForceMode.Impulse);
    }

    public void AirMove(Vector2 direction)
    {
        if (!isClimbedUp)
        {
            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
            Vector3 velocity = moveDirection * curspeed;
            velocity.y = m_rigidbody.velocity.y;
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            //Debug.Log("[이민호] 공중이동");
            m_rigidbody.velocity = velocity;
            m_rigidbody.rotation = Quaternion.Euler(0, angle, 0);
        }
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
            //Debug.Log($"rigid:{lerpStartRotation}, new:{newRotation}");
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
                    Quaternion finalRotation = Quaternion.Slerp(lerpStartRotation, newRotation, curTime / LerpTime);
                    if (!glideQuater)
                    {
                        m_rigidbody.rotation = finalRotation;
                    }
                    else
                    {
                        Vector3 final = finalRotation.eulerAngles;
                        float increaseOrDecrease = final.y - lastRollRotation; // 나중방향과 현재방향의 각도의 증감분
                        //Debug.Log($"[이민호]curTime: {curTime}, LerpTime: {LerpTime}");
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
                                rollAngle += 2f;
                            }
                            else
                            {
                                rollAngle -= 2f;
                            }

                            //Debug.Log("[이민호] Roll 회전");
                        }
                        else
                        {
                            if (!RLcheck)
                            {
                                rollAngle -= 2f;
                            }
                            else
                            {
                                rollAngle += 2f;
                            }
                            //Debug.Log("[이민호] Roll 원상복구회전");
                        }

                        rollAngle = Mathf.Clamp(rollAngle, -65.0f, 65.0f);
                        final.z = rollAngle;

                        m_rigidbody.rotation = Quaternion.Euler(final);
                    }
                    //Debug.Log($"[이민호] rotation:{m_rigidbody.rotation}, new:{newRotation}");
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
            if (glideLiftDrag)
            {
                m_rigidbody.AddForce(new Vector3(moveDirection.x * Mathf.Abs(m_rigidbody.velocity.y) * drag / lift
                    , m_rigidbody.velocity.y * drag / lift
                    , moveDirection.z * Mathf.Abs(m_rigidbody.velocity.y)) * drag / lift, ForceMode.Acceleration);
            }
            // 중력 가속도만 적용된 급하강
            else
            {
                m_rigidbody.AddForce(new Vector3(moveDirection.x, m_rigidbody.velocity.y, moveDirection.z),
                    ForceMode.Acceleration);
            }

            //Quaternion newRotation = Quaternion.LookRotation(velocity);
            //m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, Time.deltaTime * 5.0f);
            //m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, 1.0f);
            
            //Debug.Log("글라이딩");
        }
    }

    public void GlideLiftDragActive()
    {
        if (!glideLiftDrag)
        {
            glideLiftDrag = true;
            LiftDragTextObj.SetActive(false);
        }
        else
        {
            glideLiftDrag = false;
            LiftDragTextObj.SetActive(true);
        }
    }

    public void GlideQuaterActive()
    {
        if (!glideQuater)
        {
            glideQuater = true;
            QuaterTextObj.SetActive(false);
        }
        else
        {
            glideQuater = false;
            QuaterTextObj.SetActive(true);
        }
    }
    
    public void isSwimed()
    {
        if (inWater)
        {
            d_fromWaterSurface = waterSurface - transform.position.y;
            d_fromWaterSurface = Mathf.Clamp(d_fromWaterSurface, float.MinValue, waterSurface);
            isSwim = d_fromWaterSurface >= swimLevel;
            if (isSwim)
            {
                checkMonsterFollow = true;
            }
        }
        else
        {
            waterSurface = 0.0f;
        }
    }
    public void Swim_idle()
    {
        d_fromWaterSurface = waterSurface - transform.position.y;
        d_fromWaterSurface = Mathf.Clamp(d_fromWaterSurface, float.MinValue, waterSurface);
        if (d_fromWaterSurface >= swimLevel)
        {
            
            if (playerstatus.currentStamina < playerstatus.currentMaxstamina)
            {
                StartCoroutine(HealStamina(1.0f));
            }
            d_fromWaterSurface = swimLevel;
            currentState = playerState.Swim_idleState;
            Vector3 translateWater = new Vector3(m_rigidbody.position.x, waterSurface - d_fromWaterSurface,
                m_rigidbody.position.z);
            m_rigidbody.position = translateWater;
            m_rigidbody.useGravity = false;
            m_rigidbody.velocity = Vector3.zero;
            
            if (slidingCheck)
            {
                slidingCheck = false;
            }
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
            m_rigidbody.useGravity = false;
            currentState = playerState.swimmingState;

            Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);

            m_rigidbody.AddForce(moveDirection * swimSpeed, ForceMode.Impulse);
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, newRotation, 0.5f);
        }
    }


    public void Equipment()
    {
        if (!opitionUi.activeSelf)
        {
            if (playerstatus.currentItem == PlayerStatus.item.attack)
            {
                m_rigidbody.velocity = Vector3.zero;
                currentState = playerState.attack;
            }
            else if (playerstatus.currentItem == PlayerStatus.item.interaction_sleep)
            {
                GameManager.instance.SetInGameTime(7);
            }
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
        if (playerstatus.currentStamina < playerstatus.currentMaxstamina && isGrounded)
        {
            if (slidingCheck)
            {
                slidingCheck = false;
            }

            StartCoroutine(HealStamina(1.0f));
        }

        if (isGrounded)
        {
            checkMonsterFollow = false;
        }
        
        //isGrounded = Physics.CheckSphere(transform.position, 0.1f, layerMask) || isSlope;
        if (!isClimbed && (inWater == false || isGrounded))
        {
            m_rigidbody.useGravity = true;
            m_rigidbody.isKinematic = false;
        }
    }
    
    
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, 6.0f);
        
        //Gizmos.DrawWireSphere(transform.position,0.1f);
        
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
        int layerMask = (1 << LayerMask.NameToLayer("Grounded"));
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
        //Debug.DrawLine(origin,origin + Vector3.forward * 1.0f,Color.red,1.0f);
        RaycastHit hit;
        if (!isGrounded && Physics.Raycast(origin, transform.forward, out hit, 1.0f, layerMask))
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
        checkDirection = Vector3.zero;
    }

    public void Sliding()
    {
        if (!isGrounded && isClimbed && !climbFlap)
        {
            slidingCheck = true;
            isClimbedUp = false;
            currentState = playerState.sliding;
        }
        else
        {
            slidingCheck = false;
        }
    }

    public bool ClimbingUp()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Grounded"));
        Vector3 HeadPostion = transform.position + transform.up.normalized * 1.3f;
        //Debug.DrawRay(HeadPostion, transform.forward.normalized * 1.0f, Color.red);
        if (!Physics.Raycast(HeadPostion, transform.forward.normalized, 1.0f,layerMask))
        {
            return true;
        }

        return false;
    }
    
    public void Climbing(Vector2 input)
    {
        int layerMask = (1 << LayerMask.NameToLayer("Grounded"));

        Vector3 offset = transform.TransformDirection(Vector2.one * 0.5f);
        checkDirection = Vector3.zero;
        int k = 0;

        for (int i = 0; i < 4; i++) 
        { 
            RaycastHit checkHit; 
            if (Physics.Raycast(transform.position + offset, 
                transform.forward, 
                out checkHit,layerMask))
            { 
                checkDirection += checkHit.normal; 
                k++;
            }
            
            offset = Quaternion.AngleAxis(80.0f, transform.forward) * offset; 
            //Debug.Log($"check: {checkDirection}");
        }
        
        checkDirection /= k; 
        checkDirection = checkDirection.normalized; 
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, -checkDirection, out hit, 1.0f,layerMask))
        {

            //Debug.Log($"[이민호] 됨");
            m_rigidbody.isKinematic = false;
            m_rigidbody.position = Vector3.Lerp(m_rigidbody.position, hit.point + hit.normal * 0.5f, 
                5f * Time.fixedDeltaTime);
            
            transform.forward = Vector3.Lerp(transform.forward, -hit.normal, 10f * Time.fixedDeltaTime);
            
            currentState = playerState.climbing; 
            m_rigidbody.velocity = transform.TransformDirection(input) * 2.0f;
        }
        
        if (checkDirection != Vector3.zero && !isClimbedUp && ClimbingUp() && input == Vector2.up)
        {
            //Debug.Log("[이민호] 머리가 빔");
            isClimbedUp = true;
            curspeed = playerstatus.walkSpeed;
            _animator.Rebind();
            _animator.Play("Flap");
            flapEffect.Reinit();
            climbFlap = true;
            currentState = playerState.flapState;
            flapEffect.Play();
            //Debug.Log("[이민호] 플랩");
            m_rigidbody.velocity = Vector3.zero;
            Vector3 jumpDirection = Vector3.up * 6.0f;
            Vector3 playerDir = transform.forward;
            playerDir.y = 0.0f;
            playerDir = playerDir.normalized;
            //Debug.DrawRay(this.transform.position,playerDir * 1.0f,Color.red,1.0f);
            Vector3 climbUpDirection = playerDir * climpSpeed + jumpDirection;
            m_rigidbody.velocity = climbUpDirection;
            //m_rigidbody.AddForce(jumpDirection,ForceMode.VelocityChange);
        }
            //m_rigidbody.velocity = Vector3.up * 5f + hit.normal * 2f;
    }


    public Vector2 SquareToCircle(Vector2 input)
    {
        return (input.sqrMagnitude >= 1f) ? input.normalized : input;
    }
    
    public void HitStart(float damageValue, Rigidbody monsterRigidbody)
    {
        playerstatus.HitHealth(damageValue);
        if (playerstatus.GetCurHealth() <= 0)
        {

            currentState = playerState.die;
            Debug.Log("[이민호] 죽음");
            this.gameObject.layer = LayerMask.NameToLayer("Player");
            hited = false;
            invincible = false;
            bodyMaterial.SetFloat("RedLv", 0.0f);
            capeMaterial.SetFloat("RedLv",0.0f);
            isDead = true;
        }
        else if (!invincible)
        {
            Debug.Log("[이민호] 플레이어 피격");
            SoundManager.Instance.PlaySFX(1);
            this.gameObject.layer = LayerMask.NameToLayer("Player_invincible");
            hited = true;
            invincible = true;
            bodyMaterial.SetFloat("RedLv", 0.1f);
            capeMaterial.SetFloat("RedLv",0.1f);
            currentState = playerState.hit;
            StartCoroutine(HitInvincible());
            StartCoroutine(HitBlink());
            Vector3 differnce = transform.position - monsterRigidbody.transform.position;
            differnce = differnce.normalized * 5.0f;
            m_rigidbody.AddForce(differnce, ForceMode.Impulse);
        }
    }

    IEnumerator HitInvincible()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("[이민호] 무적 해제");
        invincible = false;
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    
    
    
    IEnumerator HealStamina(float value)
    {
        playerstatus.HealthStamina(value);
        yield return new WaitForSeconds(0.1f);
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

    public void DeadPlayerEnd()
    {
        PlayableDirector respawn = reSpawnScene.GetComponent<PlayableDirector>();
        respawn.Play();
    }
    
    public void SetInvincible(bool value)
    {
        invincible = value;
    }

    public void RespawnPlayer()
    {
        transform.position = respawnPoint.transform.position;
        currentState = playerState.Ground_idleState;
        playerstatus.ReSetCurHealth();
    }

    public void IsDeadedPlayer()
    {
        isDead = false;
    }

    public void StartDie()
    {
        SoundManager.Instance.PlaySFX(7);
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInputManager m_inputManager;
    private PlayerMovement m_PlayerMovement;

    private Animator m_animation;
    void Start()
    {
        m_animation = GetComponent<Animator>();
        m_inputManager = GetComponent<PlayerInputManager>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
            m_animation.SetBool("Attack",m_PlayerMovement.currentState == PlayerMovement.playerState.attack);
        
            m_animation.SetBool("Sliding",m_PlayerMovement.currentState == PlayerMovement.playerState.sliding);
        
            m_animation.SetBool("isGrounded", m_PlayerMovement.isGrounded == true);
            
            m_animation.SetBool("Run", m_PlayerMovement.currentState == PlayerMovement.playerState.runState);
            
            m_animation.SetBool("Walk", m_PlayerMovement.currentState == PlayerMovement.playerState.walkState);

            m_animation.SetBool("Jump", m_PlayerMovement.currentState == PlayerMovement.playerState.jumpState);
            
            m_animation.SetBool("Ground_Idle", m_PlayerMovement.currentState == PlayerMovement.playerState.Ground_idleState);
            
            m_animation.SetBool("Swimming", m_PlayerMovement.currentState == PlayerMovement.playerState.swimmingState);

            m_animation.SetBool("Swim_Idle", m_PlayerMovement.currentState == PlayerMovement.playerState.Swim_idleState);
            
            m_animation.SetBool("Gliding", m_PlayerMovement.currentState == PlayerMovement.playerState.glideState);
            
            m_animation.SetBool("Fall", m_PlayerMovement.currentState == PlayerMovement.playerState.failState);

            m_animation.SetBool("Flap", m_PlayerMovement.currentState == PlayerMovement.playerState.flapState);
            
            m_animation.SetBool("Climb_Idle",m_PlayerMovement.currentState == PlayerMovement.playerState.climb_idleState);
            
            m_animation.SetBool("Climbing",m_PlayerMovement.currentState == PlayerMovement.playerState.climbing);
    }
}

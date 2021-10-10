using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // 몬스터의 이동 속도
    public float speed = 10f;

    private MonsterState _monsterState; // 몬스터의 상태
    private Coroutine _monsterCoroutine; // 몬스터 행동 코루틴
    private Rigidbody _monsterRigidbody; // 몬스터 rigidbody
    private GameObject _player;
    
    [SerializeField] 
    private GameObject monsterPrefab;

    private void Awake()
    {
        // 몬스터 오브젝트 생성과 동시에 1번 Get
        _monsterRigidbody = GetComponent<Rigidbody>();
        _monsterState = MonsterState.Alive; // 몬스터가 생성되었으니 초기 설정 때 생존 상태로 설정해준다.
    }

    private void OnEnable()
    {
        // 몬스터 코루틴 시작
        _monsterCoroutine = StartCoroutine(MonsterCoroutine());
    }

    private IEnumerator MonsterCoroutine()
    {
        // 초기화 설정 진행
        Init();
        
        // 몬스터 FSM 시작
        while (true)
        {
            switch (_monsterState)
            {
                case MonsterState.Alive:
                    Alive();
                    break;
                case MonsterState.Dead:
                    Dead();
                    break;
            }
            
            yield return null;
        }
    }

    private void Init()
    {
        _player = GameObject.Find("Player");
    }

    private void Alive()
    {
        // 사용자를 찾아 다니게 만듬
        var playerDirection = (_player.transform.position - transform.position).normalized; // 플레이어 위치 - 몬스터 위치 -> 벡터값으로 만듬
        _monsterRigidbody.AddForce(playerDirection * Time.deltaTime * speed, ForceMode.VelocityChange);
    }

    private void Dead()
    {
        // Disable 되어서 Object Pool Manager에게 관리 받을 수 있는 상태로 되야 함.
        gameObject.SetActive(false);

        if(_monsterCoroutine!=null) StopCoroutine(_monsterCoroutine);
        
        Destroy(monsterPrefab);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (monsterPrefab == GameObject.Find("SpawnPoint").GetComponent<SphereCollider>())
        {
            Debug.Log("몬스터가 콜라이더와 충돌함");
            _monsterState = MonsterState.Dead;
        }
    }
}

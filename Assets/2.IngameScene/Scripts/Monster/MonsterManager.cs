using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class MonsterManager : MonoBehaviour
{

    public enum monsterType
    {
        nightMonster,
        green_slimyee,
        red_slimyee,
        green_longyee,
        red_longyee
    }

    public monsterType curMonsterType;
    [SerializeField]
    private float health;

    public VisualEffect explodeEffect;
    public float thrust = 5.0f;
    public float knockTime = 4.0f;
    [SerializeField]
    private int inTime;
    private Rigidbody m_rigidbody;
    private SpawnLoot spawnLoot;

    [SerializeField]
    private Vector3 spawnPoint;
    public bool checkHit = false;

    public SkinnedMeshRenderer bodyRenderer;
    public SkinnedMeshRenderer eyeRenderer;
    private Material bodyMaterial;
    private Material eyeMaterial;

    private bool inCameravisible = false;

    //status
    //private float damgeValue;
    //private float speed;

    private bool dead = false;
    void Start()
    {
        // 01
        if (curMonsterType == monsterType.green_slimyee || curMonsterType == monsterType.nightMonster)
        {
            health = 10.0f;
            //damgeValue = 10.0f;
        }
        // 02
        else if (curMonsterType == monsterType.green_longyee || curMonsterType == monsterType.red_slimyee)
        {
            health = 20.0f;
            //damgeValue = 20.0f;
        }
        // 03
        else if(curMonsterType == monsterType.red_longyee)
        {
            health = 30.0f;
            //damgeValue = 30.0f;
        }
        // slimyee
        if (curMonsterType == monsterType.green_slimyee || curMonsterType == monsterType.red_slimyee 
                                                        || curMonsterType == monsterType.nightMonster)
        {
            //speed = 2.0f;
        }
        // longyee
        else if (curMonsterType == monsterType.green_longyee || curMonsterType == monsterType.red_longyee)
        {
            //speed = 3.0f;
        }

        spawnPoint = this.transform.position;
        m_rigidbody = GetComponent<Rigidbody>();
        spawnLoot = GetComponent<SpawnLoot>();
        bodyMaterial = bodyRenderer.material;
        eyeMaterial = eyeRenderer.material;
        bodyMaterial.SetFloat("RedLv", 0.0f);
        eyeMaterial.SetFloat("RedLv",0.0f);
    }

    private void Update()
    {
        if (!dead)
        {
            MonsterVisible();
        }
    }

    private void MonsterVisible()
    {
        //Camera mainCamera = Camera.main;
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        var point = transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                if (inCameravisible)
                {
                    dead = true;
                    //Debug.Log("[이민호] 밖으로 나감");
                    Destroy(gameObject);
                }

                return;
            }
        }

        inCameravisible = true;
    }
    public void TakeHit()
    {
        health -= 10.0f;
        //Debug.Log($"[이민호] 몬스터 체력: {health}");
        bool isDead = health <= 0;

        if (isDead)
        {
            VisualEffect newExplodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
            newExplodeEffect.Play();
            Destroy(newExplodeEffect.gameObject, 0.5f);
            spawnLoot.spawnLoot = true;
        }
    }

    public Vector3 RandomPoint(float range)
    {
        
        Vector3 randomPoint = spawnPoint + Random.insideUnitSphere * range;

        Debug.DrawRay(randomPoint, Vector3.up, Color.red, 1);
        StartCoroutine(moveCoolTime());
        
        return randomPoint;
    }
    
    private IEnumerator moveCoolTime()
    {
        yield return new WaitForSeconds(3.0f);
    }
    
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, 6.0f);
        
        //Gizmos.DrawWireSphere(this.transform.position,1.0f);
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Equipment_Attack"))
        {
            checkHit = true;
        }
    }
    
    public void KnockBack()
    {
        Rigidbody player = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
        if (player != null)
        {
            Vector3 difference = transform.position - player.transform.position;
            difference = difference.normalized * thrust;
            m_rigidbody.AddForce(difference, ForceMode.Impulse);
            StartCoroutine(KnockCo(m_rigidbody));
        }
    }

    public void MonsterHitStart()
    {
        bodyMaterial.SetFloat("RedLv", 0.1f);
        eyeMaterial.SetFloat("RedLv",0.1f);
    }

    public void MonsterHitEnd()
    {
        bodyMaterial.SetFloat("RedLv", 0.0f);
        eyeMaterial.SetFloat("RedLv",0.0f);
    }

    private IEnumerator KnockCo(Rigidbody monster)
    {
        if (monster != null)
        {
            yield return new WaitForSeconds(knockTime);
            monster.velocity = Vector3.zero;
        }
    }
}
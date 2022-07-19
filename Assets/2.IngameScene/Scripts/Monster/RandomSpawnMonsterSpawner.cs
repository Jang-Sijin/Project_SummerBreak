using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class RandomSpawnMonsterSpawner : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> monsters = new List<GameObject>();

    
    [SerializeField] 
    private List<GameObject> spawnMonsters = new List<GameObject>();
    
    [SerializeField] 
    [Range(1, 99)] 
    private int minNumber = 2;

    [SerializeField] 
    [Range(2, 100)]
    private int maxNumber = 3;
    
    [SerializeField] 
    public bool hasBeenCollected = false;

    [SerializeField] 
    private float spawnRange = 15.0f;

    [SerializeField] private bool spawnCheck = false;
    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < spawnMonsters.Count; ++i)
        {
            if (spawnMonsters[i] == null)
            {
                spawnMonsters.RemoveAt(i);
            }
        }

        if (!spawnMonsters.Any())
        {
            MonsterVisible();
        }

        if (!hasBeenCollected && SpawnTrigger())
        {
            
            Spawn();
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
                hasBeenCollected = false;
            }
        }
    }

    private bool SpawnTrigger()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        //Gizmos.DrawWireSphere(transform.position, 6.0f);
        if (Physics.CheckSphere(transform.position, spawnRange, layerMask))
        {
            //Debug.Log("[이민호] 생성 범위내로 옴");
            return true;
        }

        return false;
    }

    private void Spawn()
    {
        hasBeenCollected = true;
        spawnCheck = (Random.value > 0.5f);
        if (spawnCheck)
        {
            int number = Random.Range(minNumber, maxNumber);
            StartCoroutine(CreateMonster(number));
        }
        //Debug.Log("[이민호] 몬스터 생성");
    }

    IEnumerator CreateMonster(int number)
    {
        float range_x;
        float range_z;
        for (int i = 0; i < number; i++)
        {
            range_x = Random.Range((spawnRange / 2) * -1, spawnRange / 2);
            range_z = Random.Range((spawnRange / 2) * -1, spawnRange / 2);
            Vector3 origin = new Vector3(this.transform.position.x + range_x, this.transform.position.y, this.transform.position.z + range_z);
            GameObject tempLoot = Instantiate(monsters[Random.Range(0, monsters.Count)],origin,Quaternion.identity);
            spawnMonsters.Add(tempLoot);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, spawnRange);
        
        Gizmos.DrawWireCube(this.transform.position,new Vector3(spawnRange,0.0f,spawnRange));
    }
}

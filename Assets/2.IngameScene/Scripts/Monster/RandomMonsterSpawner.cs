using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMonsterSpawner : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> loot = new List<GameObject>();

    [SerializeField] 
    [Range(1, 99)] 
    private int minNumber = 2;

    [SerializeField] 
    [Range(2, 100)]
    private int maxNumber = 3;


    public bool hasBeenCollected = false;

    private bool spawnMonster = false;

    [SerializeField] 
    private float spawnRange = 15.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (!hasBeenCollected && SpawnTrigger())
        {
            spawnMonster = false;
            Spawn();
        }
    }

    private bool SpawnTrigger()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        //Gizmos.DrawWireSphere(transform.position, 6.0f);
        if (Physics.CheckSphere(transform.position,15.0f, layerMask))
        {
            //Debug.Log("[이민호] 생성 범위내로 옴");
            return true;
        }
        
        return false;
    }
    private void Spawn()
    {
        hasBeenCollected = true;
        int number = Random.Range(minNumber, maxNumber);
        StartCoroutine(CreateMonster(number));
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
            GameObject tempLoot = Instantiate(loot[Random.Range(0, loot.Count)],origin,Quaternion.identity);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 15.0f);
        
        Gizmos.DrawWireCube(this.transform.position,new Vector3(15.0f,0.0f,15.0f));
    }

    
}

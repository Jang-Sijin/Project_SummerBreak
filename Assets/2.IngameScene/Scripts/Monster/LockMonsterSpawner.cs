using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LockMonsterSpawner : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> monsters = new List<GameObject>();
    
    [SerializeField] 
    private List<GameObject> spawnPoints = new List<GameObject>();
    
    
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
        int number = spawnPoints.Count;
        StartCoroutine(CreateMonster(number));
        //Debug.Log("[이민호] 몬스터 생성");
    }

    IEnumerator CreateMonster(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject tempLoot = Instantiate(monsters[Random.Range(0, monsters.Count)],spawnPoints[i].transform.position,Quaternion.identity);
            yield return new WaitForSeconds(0.001f);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(this.transform.position, spawnRange);
        
        //Gizmos.DrawWireCube(this.transform.position,new Vector3(spawnRange,0.0f,spawnRange));
    }
}

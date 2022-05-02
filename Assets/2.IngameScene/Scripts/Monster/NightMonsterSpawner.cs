using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NightMonsterSpawner : MonoBehaviour
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


    public bool hasBeenCollected = false;
    
    private Vector3 playerPos;

    [SerializeField] 
    private int spawnCount;

    [SerializeField] 
    private int coolTime = 0;

    [SerializeField] 
    private int inTime;

    [SerializeField] 
    private bool dayCheck = false;
    
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

        if (SpawnTrigger() && !hasBeenCollected)
        {
            Spawn();
        }
    }
    private bool SpawnTrigger()
    {
        int timer = inTime;
        inTime = GameManager.instance.GetInGameTime().Hour;
        if (inTime == 20 && !dayCheck)
        {
            dayCheck = true;
            spawnCount = Random.Range(0, 3);
        }

        if (dayCheck && (inTime > 5 && inTime < 7))
        {
            dayCheck = false;
        }
        
        if ((inTime < 5 && inTime >= 0) || (inTime >= 21 && inTime <= 23))
        {
            //Debug.Log("[이민호] 생성 범위내로 옴");
            if ( timer != inTime)
            {
                ++coolTime;
                if (coolTime >= 3)
                {
                    coolTime = 0;
                    if (!spawnMonsters.Any() && spawnCount > 0)
                    {
                        hasBeenCollected = false;
                        --spawnCount;
                        Debug.Log("[이민호] 나이트 몬스터 생성");
                        return true;
                    }
                }
            }
            
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
        for (int i = 0; i < number; ++i)
        {
            playerPos = this.gameObject.transform.parent.gameObject.transform.position;
            playerPos.y = playerPos.y + 3.0f;
            Vector3 origin = playerPos + Random.insideUnitSphere * 3.0f;
            GameObject tempLoot =
                Instantiate(monsters[Random.Range(0, monsters.Count)], origin, Quaternion.identity);
            spawnMonsters.Add(tempLoot);
            yield return new WaitForSeconds(1.0f);
        }
    }
    
}

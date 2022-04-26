using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NightMonsterSpawner : MonoBehaviour
{
    
    [SerializeField] 
    private List<GameObject> monsters = new List<GameObject>();

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

    private int coolTime;
    
    // Update is called once per frame
    void Update()
    {
        
        if (SpawnTrigger() && !hasBeenCollected)
        {
            Spawn();
        }
    }
    private bool SpawnTrigger()
    {
        int inTime = GameManager.instance.GetInGameTime().Hour;
        
        if ((inTime < 5 && inTime >= 0) || (inTime >= 20 && inTime <= 23))
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
        for (int i = 0; i < number; ++i)
        {
            playerPos = this.gameObject.transform.parent.gameObject.transform.position;
            playerPos.y = playerPos.y + 3.0f;
            Vector3 origin = playerPos + Random.insideUnitSphere * 3.0f;
            GameObject tempLoot =
                Instantiate(monsters[Random.Range(0, monsters.Count)], origin, Quaternion.identity);
            yield return new WaitForSeconds(1.0f);
        }
    }
    
}

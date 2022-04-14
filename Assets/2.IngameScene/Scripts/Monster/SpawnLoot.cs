using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnLoot : MonoBehaviour
{

    [SerializeField] 
    private List<GameObject> loot = new List<GameObject>();

    [SerializeField] 
    [Range(1, 99)] 
    private int minNumber = 2;
    
    [SerializeField] 
    [Range(2, 100)] 
    private int maxNumber = 20;

    [SerializeField] 
    private Transform spawnPoint;

    private bool hasBeenCollected = false;

    public bool spawnLoot = false;

    private int coinCount = -1;
    
    private void OnValidate()
    {
        if (minNumber > maxNumber)
            maxNumber = minNumber + 1;
    }

    void Update()
    {
        if (spawnLoot && !hasBeenCollected)
        {
            spawnLoot = false;
            Loot();
        }
        if (coinCount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void Loot()
    {
        hasBeenCollected = true;
        int number = Random.Range(minNumber, maxNumber);
        coinCount = (number / 5) + (number % 5);
        //Debug.Log($"[이민호] number:{number}");
        StartCoroutine(CreateLoot(number));
        //Debug.Log("[이민호] 한번");
    }

    IEnumerator CreateLoot(int number)
    {
        int copper = number % 5;
        int sliver = number / 5;
        if (sliver != 0)
        {
            for (int i = 0; i < sliver; i++)
            {
                coinCount--;
                GameObject tempLoot = Instantiate(loot[1]);
                tempLoot.transform.position = spawnPoint.position;
                yield return new WaitForSeconds(0.001f);
            }
        }
        else
        {
            for (int i = 0; i < copper; i++)
            {
                coinCount--;
                GameObject tempLoot = Instantiate(loot[0]);
                tempLoot.transform.position = spawnPoint.position;
                yield return new WaitForSeconds(0.001f);
            }
        }

    }
    
}

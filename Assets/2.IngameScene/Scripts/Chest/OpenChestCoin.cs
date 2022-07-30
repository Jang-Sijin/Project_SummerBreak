using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChestCoin : MonoBehaviour
{    
    [SerializeField] 
    private List<GameObject> loot = new List<GameObject>();

    [SerializeField] 
    private Transform spawnPoint;

    private bool hasBeenCollected = false;

    public bool spawnLoot = false;
    
    
    void Update()
    {
        if (spawnLoot && !hasBeenCollected)
        {
            spawnLoot = false;
            Loot();
        }
    }

    public void OpenStart()
    {
        spawnLoot = true;
    }
    
    private void Loot()
    {
        hasBeenCollected = true;
        //Debug.Log($"[이민호] number:{number}");
        if (loot.Count > 0)
        {
            StartCoroutine(CreateLoot(loot.Count));
        }
        //Debug.Log("[이민호] 한번");
    }

    IEnumerator CreateLoot(int number)
    {
        for (int i = number; i > 0; i--)
        {
            GameObject tempLoot = Instantiate(loot[i - 1]);
            tempLoot.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(0.001f);
        }
    }
}

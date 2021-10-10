using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonster : MonoBehaviour
{
    public GameObject player;
    public GameObject monsterPrefab;
    public int enemyCount = 3;
    public float spawnRate = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                var monsterGameObject = Instantiate(monsterPrefab);
                monsterGameObject.transform.position = transform.position;
            }
        }
    }
}

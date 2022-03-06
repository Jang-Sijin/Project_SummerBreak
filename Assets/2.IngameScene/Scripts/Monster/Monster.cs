using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private MeshRenderer material;
    void Start()
    {
        material = GetComponent<MeshRenderer>();
        material.material.color = Color.white;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (material.material.color == Color.white)
        {
            material.material.color = Color.red;
        }
        else
        {
            material.material.color = Color.white;
        }
    }
    
    
}
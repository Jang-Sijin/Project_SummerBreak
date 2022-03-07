using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }
}

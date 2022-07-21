using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgeText : MonoBehaviour
{
    [SerializeField] 
    private float destoryTime = 3.0f;

    [SerializeField] 
    private Vector3 offset = new Vector3(0, 2, 0);

    [SerializeField] private Vector3 randomizeIntensty = new Vector3(0.5f, 0, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destoryTime);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(Random.Range(-randomizeIntensty.x, randomizeIntensty.x),
            Random.Range(-randomizeIntensty.y, randomizeIntensty.y),
            Random.Range(-randomizeIntensty.z, randomizeIntensty.z));
    }
    
}

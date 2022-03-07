using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == player)
        {
            if (player.inWater == false)
            {
                player.inWater = true;
            }
            
            if (player.waterSurface != transform.position.y)
            {
                player.waterSurface = transform.position.y;
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == player)
        {
            if (player.inWater == true)
            {
                player.inWater = false;
            }

            if (player.waterSurface != transform.position.y)
            {
                player.waterSurface = transform.position.y;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

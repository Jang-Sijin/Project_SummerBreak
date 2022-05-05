using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
    public PlayerMovement player;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InGameTimeStop()
    {
        Time.timeScale = 0;
    }
    
    public void InGameTimeStart()
    {
        Time.timeScale = 1;
    }
}

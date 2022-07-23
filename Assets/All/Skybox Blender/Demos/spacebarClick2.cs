﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacebarClick2 : MonoBehaviour
{
    public SkyboxBlender skyboxScript;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            skyboxScript.SkyboxBlend(true);
        }

        //stop blending
        if(Input.GetKeyDown(KeyCode.E)) {
            skyboxScript.StopSkyboxBlend(false);
        }
    }
}

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SkyboxBlender : MonoBehaviour
{
    public Material[] skyboxMaterials;                                              //an array of all the skybox materials
    public bool makeFirstMaterialSkybox;                                            //flag whether to make the first material the default skybox of the scene

    public float blendSpeed = 0.5f;                                                 //the speed of blending
    float totalBlendValue = 1f;                                                     //the maximum blend value to reach
    public float timeToWait = 0f;                                                   //the time to wait before blending the next material
    public bool loop = true;                                                        //loop the materials again when reaches last one

    public float rotateTo;                                                          //rotate the skybox to certain value
    public float rotationSpeed;                                                     //speed of skybox rotation
    
    bool blend;                                                                     //flag to start blending
    float blendValue;                                                               //increment the value of the blend with delta time within Update method
    
    float defaultBlend;                                                             //save the default blend value on startup to return to on application quit
    float defaultRotation;                                                          //save the default rotation value to return to on app quit
    Material defaultSkyboxMaterial;                                                 //save the default skybox material
    
    float rotationSpeedValue;                                                       //increment the rotation speed
    bool rotateSkybox;                                                              //flag whether rotation should occur or not

    Material skyboxBlenderMaterial;                                                 //cache the material with the Skybox Blender Shader
    int index = 0;                                                                  //current material index (in blending)
    public int CurrentIndex {
        get { return index; }
    }

    Texture currentTexture;                                                         //get the current scene skybox texture, to check wheather it's the same as the first material
    bool notSameTex;                                                                //flag that the current skybox texture and the first skybox material are not the same and need blending
    bool comingFromLoop;

    bool singleBlend = false;                                                       //flag that we want only a single blend
    bool stillRunning = false;                                                      //flag that blending is still running even when waiting for coroutines 

    bool singleBlending = false;                                                    //flag that a single blend is needed
    bool stopped = false;


    void Awake()
    {
        skyboxBlenderMaterial = Resources.Load("Material & Shader/Skybox Blend Material", typeof(Material)) as Material;
        
        if (skyboxBlenderMaterial) {
            defaultBlend = skyboxBlenderMaterial.GetFloat("_BlendCubemaps");
            defaultRotation = skyboxBlenderMaterial.GetFloat("_Rotation");
            defaultSkyboxMaterial = skyboxBlenderMaterial;
            inspectorAndAwakeChanges();
        }else{
            Debug.LogWarning("Can't find Skybox Blend Material in resources. Please re-import!");
        }
    }

    void Update() 
    {
        
        //skybox blending
        if (blend) {

            blendValue += Time.deltaTime * blendSpeed;
            skyboxBlenderMaterial.SetFloat("_BlendCubemaps", blendValue);
            stopped = false;     
            
            if (skyboxBlenderMaterial.GetFloat("_BlendCubemaps") >= totalBlendValue) {
                StopAllCoroutines();
                blend = false;
                blendValue = 0f;
                
                skyboxBlenderMaterial.SetFloat("_BlendCubemaps", totalBlendValue);
                skyboxBlenderMaterial.SetFloat("_BlendCubemaps", 0f);
                
                skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[index].GetTexture("_Tex"));
                skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[index].GetColor("_Tint"));
            
                RenderSettings.skybox = skyboxBlenderMaterial;

                //increment index and blend if not reached end
                if ((index + 1) < skyboxMaterials.Length) {
                    
                    if (!comingFromLoop) {
                        index++;
                    }

                    comingFromLoop = false;
                    skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[index].GetTexture("_Tex"));
                    skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[index].GetColor("_Tint"));

                    if (index + 1 < skyboxMaterials.Length) {
                        skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[index+1].GetTexture("_Tex"));
                        skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[index+1].GetColor("_Tint"));
                    }
                    
                    if (index - (skyboxMaterials.Length - 1) > 0) {
                        if (!singleBlend) blend = true;
                    }else{
                        if (!singleBlend) StartCoroutine(waitForTimebeforeBlending(true));
                    }
                }

                //if reached end and loopable
                if ((index + 1) == skyboxMaterials.Length) {
                    
                    if (loop) {
                        
                        if (singleBlend) {
                            stillRunning = false;
                            return;
                        }

                        skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[index].GetTexture("_Tex"));
                        skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[0].GetTexture("_Tex"));
                        
                        skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[index].GetColor("_Tint"));
                        skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[0].GetColor("_Tint"));

                        RenderSettings.skybox = skyboxBlenderMaterial;
                        comingFromLoop = true;
                        
                        StartCoroutine(waitForTimebeforeBlending(false));
                    }else{
                        stillRunning = false;
                    }
                }
            }
        }

        //single blending
        if (singleBlending) {

            blendValue += Time.deltaTime * blendSpeed;
            skyboxBlenderMaterial.SetFloat("_BlendCubemaps", blendValue);
            stopped = false; 
            
            if (skyboxBlenderMaterial.GetFloat("_BlendCubemaps") >= totalBlendValue) {
                StopAllCoroutines();
                singleBlending = false;
                blendValue = 0f;
                
                skyboxBlenderMaterial.SetFloat("_BlendCubemaps", totalBlendValue);
                skyboxBlenderMaterial.SetFloat("_BlendCubemaps", 0f);
                
                skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[index].GetTexture("_Tex"));
                skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[index].GetColor("_Tint"));
            
                RenderSettings.skybox = skyboxBlenderMaterial;

                stillRunning = false;
            }
        }
        
        //blending for if the current skybox is not the same as first material
        if (notSameTex) {

            blendValue += Time.deltaTime * blendSpeed;
            skyboxBlenderMaterial.SetFloat("_BlendCubemaps", blendValue);
            stopped = false;
            
            if (skyboxBlenderMaterial.GetFloat("_BlendCubemaps") >= totalBlendValue) {

                StopAllCoroutines();
                notSameTex = false;
                blendValue = 0f;
                
                skyboxBlenderMaterial.SetFloat("_BlendCubemaps", totalBlendValue);
                skyboxBlenderMaterial.SetFloat("_BlendCubemaps", 0f);

                skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[0].GetTexture("_Tex"));
                skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[1].GetTexture("_Tex"));

                skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[0].GetColor("_Tint"));
                skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[1].GetColor("_Tint"));

                RenderSettings.skybox = skyboxBlenderMaterial;

                if (singleBlend) {
                    stillRunning = false;
                }else{
                    StartCoroutine(waitForTimebeforeBlending(false));
                }
            }
        }

        //skybox rotation
        if (rotateSkybox) {
            rotationSpeedValue += Time.deltaTime * rotationSpeed;
            if(skyboxBlenderMaterial.GetFloat("_Rotation") < rotateTo){
                skyboxBlenderMaterial.SetFloat("_Rotation", rotationSpeedValue);
            }else{
                rotateSkybox = false;
                skyboxBlenderMaterial.SetFloat("_Rotation", rotateTo);
            }
        }
        
        //when in editor mode, set the skybox material in the skybox blend material
        if (Application.isEditor) {
            if (RenderSettings.skybox.HasProperty("_Tex")) {
                skyboxBlenderMaterial.SetTexture("_Tex", RenderSettings.skybox.GetTexture("_Tex"));
                skyboxBlenderMaterial.SetColor("_Tint", RenderSettings.skybox.GetColor("_Tint"));
            }
        }
    }

    //trigger the skybox blend
    public void SkyboxBlend(bool singlePassBlend = false)
    {
        if (stillRunning) return;
        
        StopAllCoroutines();
        currentTexture = RenderSettings.skybox.GetTexture("_Tex");
        
        if (singlePassBlend) {
            if (index == 0 && currentTexture != skyboxMaterials[0].GetTexture("_Tex")) {
                skyboxBlenderMaterial.SetTexture("_Tex", RenderSettings.skybox.GetTexture("_Tex"));
                skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[0].GetTexture("_Tex"));

                skyboxBlenderMaterial.SetColor("_Tint", RenderSettings.skybox.GetColor("_Tint"));
                skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[0].GetColor("_Tint"));

                notSameTex = true;
            }else{

                if (!stopped){
                    if (index+1 >= skyboxMaterials.Length) {
                        index = 0;
                    }else{
                        index++;
                    }
                }

                skyboxBlenderMaterial.SetTexture("_Tex", RenderSettings.skybox.GetTexture("_Tex"));
                skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[index].GetTexture("_Tex"));

                skyboxBlenderMaterial.SetColor("_Tint", RenderSettings.skybox.GetColor("_Tint"));
                skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[index].GetColor("_Tint"));

                singleBlending = true;
            }
            
            RenderSettings.skybox = skyboxBlenderMaterial;
            rotateSkybox = true;
            stillRunning = true;
        }else{
            //if only one element then blend from current scene skybox to the first material
            if (skyboxMaterials.Length == 1) {
                if(currentTexture != skyboxMaterials[0].GetTexture("_Tex")){
                    skyboxBlenderMaterial.SetTexture("_Tex", RenderSettings.skybox.GetTexture("_Tex"));
                    skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[0].GetTexture("_Tex"));

                    skyboxBlenderMaterial.SetColor("_Tint", RenderSettings.skybox.GetColor("_Tint"));
                    skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[0].GetColor("_Tint"));

                    RenderSettings.skybox = skyboxBlenderMaterial;
                }
            }else{
                if (skyboxMaterials[0] != null) {
                    if (currentTexture == skyboxMaterials[0].GetTexture("_Tex")) {
                        skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[0].GetTexture("_Tex"));
                        skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[0].GetColor("_Tint"));
                        RenderSettings.skybox = skyboxBlenderMaterial;  
                    }else{
                        skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[0].GetTexture("_Tex"));
                        skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[0].GetColor("_Tint"));

                        RenderSettings.skybox = skyboxBlenderMaterial;
                        notSameTex = true;
                        rotateSkybox = true;
                    }
                }
            }

            //flag some vars to start the blending in Update
            if (!notSameTex) {
                blend = true;
                stillRunning = true;
                rotateSkybox = true;
            }
        }

        singleBlend = singlePassBlend;
    }

    //stop the blending
    public void StopSkyboxBlend(bool resetBlends = false)
    {
        StopAllCoroutines();
        blend = false;
        notSameTex = false;
        singleBlending = false;
        stillRunning = false;
        rotateSkybox = false;
        stopped = true;

        if (resetBlends) blendValue = 0f;
    }

    //wait for time for normal blending
    IEnumerator waitForTimebeforeBlending(bool normalBlending = true)
    {
        
        yield return new WaitForSeconds(timeToWait);

        //means blending within the materials list
        if (!normalBlending) {
            index = 0;
        }

        blend = true;
    }

    //return the material to original blend
    void OnApplicationQuit()
    {
        skyboxBlenderMaterial.SetFloat("_BlendCubemaps", defaultBlend);
        skyboxBlenderMaterial.SetFloat("_Rotation", defaultRotation);
        
        if (currentTexture != null) { 
            skyboxBlenderMaterial.SetTexture("_Tex", currentTexture);
        }
        
        RenderSettings.skybox = defaultSkyboxMaterial;
    }

    //change skyboxes and material textures on inspector change and script awake
    void inspectorAndAwakeChanges()
    {
        if(makeFirstMaterialSkybox){
            if(skyboxMaterials.Length >= 1){
                if(skyboxMaterials[0] != null){ 
                    skyboxBlenderMaterial.SetTexture("_Tex", skyboxMaterials[0].GetTexture("_Tex"));
                    skyboxBlenderMaterial.SetColor("_Tint", skyboxMaterials[0].GetColor("_Tint"));
                    RenderSettings.skybox = skyboxBlenderMaterial;  
                }
            }else{
                Debug.LogWarning("You need to set a material first to make it the skybox");
            }
        }

        if(skyboxMaterials != null){
            if(skyboxMaterials.Length > 1){
                if(skyboxMaterials[1] != null){
                    skyboxBlenderMaterial.SetTexture("_Tex2", skyboxMaterials[1].GetTexture("_Tex"));
                    skyboxBlenderMaterial.SetColor("_Tint2", skyboxMaterials[1].GetColor("_Tint"));
                }
            }
        }
    }

    //trigger functionalities on inspector change
    void OnValidate()
    {
        if(skyboxBlenderMaterial == null)
            skyboxBlenderMaterial = Resources.Load("Material & Shader/Skybox Blend Material", typeof(Material)) as Material;

        inspectorAndAwakeChanges();
    }
}

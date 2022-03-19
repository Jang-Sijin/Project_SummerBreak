using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractionTextBar : MonoBehaviour
{
    public TextMeshPro playerName;
    
    [SerializeField]
    private Transform cam;
    
    void Start()
    {
        if (Camera.main is { }) cam = Camera.main.transform;
    }
    
    void Update()
    {
        var rotation = cam.rotation;
        playerName.transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}

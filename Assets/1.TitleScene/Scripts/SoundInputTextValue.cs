using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundInputTextValue : MonoBehaviour
{
    [SerializeField]
    [Header("음량 텍스트")] 
    private TMP_InputField soundInputField;
    
    [SerializeField]
    [Header("음량 슬라이더")]
    private Slider soundSlider;
    
    void Start()
    {
        soundInputField.text = soundSlider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

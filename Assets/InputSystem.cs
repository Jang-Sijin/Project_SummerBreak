using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [Header("게임플레이 키설정")]
    public KeyCode option;

    [Header("게임 옵션 오브젝트")]
    [SerializeField]
    private GameObject opTionUI;
    public static bool showOptionUI = false;
    private bool isOptionUIOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(option))
        {
            showOptionUI = !showOptionUI;
        }

        if (showOptionUI)
        {
            OptionUIOpen();
        }
        else
        {
            OptionUIClose();
        }
    }

    private void OptionUIOpen()
    {
        if (isOptionUIOpen)
            return;
        else
        {
            opTionUI.SetActive(true);
            isOptionUIOpen = true;
            GameManager.instance.InGameTimeStop();
        }
    }
    
    private void OptionUIClose()
    {
        if (!isOptionUIOpen)
            return;
        else
        {
            opTionUI.SetActive(false);
            isOptionUIOpen = false;
            GameManager.instance.InGameTimeStart();
        }
    }
}

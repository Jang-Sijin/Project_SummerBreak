using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private string saveNameText;

    private void Start()
    {
        ResetFunctionUI();
        SetFunctionUI();
    }

    private void SetFunctionUI()
    {
        inputField.onValueChanged.AddListener(ChangedInputField);
    }

    private void ResetFunctionUI()
    {
        inputField.onValueChanged.RemoveAllListeners();
    }

    private void ChangedInputField(string inputText)
    {
        saveNameText = inputText;
    }

    public void ClickSaveButton()
    {
        JsonManager.instance.Save(JsonManager.instance.GetCurrentSelectBtnName(), saveNameText);
    }
}

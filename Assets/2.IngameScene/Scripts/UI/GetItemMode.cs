using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetItemMode : MonoBehaviour
{
    [SerializeField] private ItemList _itemList;
    
    [SerializeField] private TMP_InputField _inputNameField;
    [SerializeField] private string _itemNameText;
    
    [SerializeField] private TMP_InputField _inputItemCountField;
    [SerializeField] private string _itemItemCountText;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetFunctionUI();
        SetFunctionUI();
    }

    private void SetFunctionUI()
    {
        _inputNameField.onValueChanged.AddListener(ChangedInputItemNameField);
        _inputItemCountField.onValueChanged.AddListener(ChangedInputItemCountField);
    }

    private void ResetFunctionUI()
    {
        _inputNameField.onValueChanged.RemoveAllListeners();
        _inputItemCountField.onValueChanged.RemoveAllListeners();

        _itemNameText = null;
        _itemItemCountText = null;
    }

    private void ChangedInputItemNameField(string inputText)
    {
        _itemNameText = inputText;
    }
    
    private void ChangedInputItemCountField(string inputText)
    {
        _itemItemCountText = inputText;
    }

    public void ClickGetItemYesButton()
    {
        if (_itemNameText != null && _itemItemCountText != null)
        {
            try
            {
                var findItem = Array.Find(_itemList.ItemDBList, item => item.itemName == _itemNameText);

                if (findItem != null)
                {
                    InventorySystem.instance.AcquireItem(findItem, Int32.Parse(_itemItemCountText));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "Debug Mode: [장시진] 아이템 획득 치트 오류 발생...");
                throw;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemToolTip : MonoBehaviour
{
    private RectTransform _rectTransform;

    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemInfoText;
    
    void Start()
    {
        _rectTransform = this.GetComponent<RectTransform>();
    }

    public void ShowToolTip(string itemName, string itemInfo)
    {
        gameObject.SetActive(true);
        StartCoroutine(UpdateMousePosition());
        _itemNameText.text = itemName;
        _itemInfoText.text = itemInfo;
    }

    private IEnumerator UpdateMousePosition()
    {
        Vector3 addPosition = new Vector3(10, -10);
        
        while (true)
        {
            print("마우스 갱신중");
            if (Input.mousePresent)
            {
                _rectTransform.position = Input.mousePosition + addPosition;
            }

            yield return null;
        }
    }
    
    public void HideToolTip()
    {
        gameObject.SetActive(false);
        StopCoroutine(UpdateMousePosition());
        _itemNameText.text = "";
        _itemInfoText.text = "";
    }
}

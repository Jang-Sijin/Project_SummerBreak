using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemToolTip : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemInfoText;
    
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }

    // 아이템 툴팁 UI 출력, itemName: 아이템 이름, itemInfo: 아이템 정보
    public void ShowToolTip(string itemName, string itemInfo)
    {
        gameObject.SetActive(true); // 툴팁 UI 활성화
        StartCoroutine(UpdateMousePosition());
        itemNameText.text = itemName; // 아이템 이름
        itemInfoText.text = itemInfo; // 아이템 정보
    }
    
    // 마우스 위치가 변경되면 툴팁 위치도 함께 이동하도록 설정
    private IEnumerator UpdateMousePosition()
    {
        Vector3 addPosition = new Vector3(10, -10);
        
        while (true)
        {
            if (Input.mousePresent)
            {
                Debug.Log("[장시진] 마우스 갱신중 " + rectTransform.position);
                rectTransform.position = Input.mousePosition + addPosition;
            }
           
            yield return null;
        }
    }
    
    public void HideToolTip()
    {
        gameObject.SetActive(false);
        StopCoroutine(UpdateMousePosition());
        itemNameText.text = "";
        itemInfoText.text = "";
    }
}

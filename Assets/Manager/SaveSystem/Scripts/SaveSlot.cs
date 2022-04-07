using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("↓ 자식 오브젝트의 컴포넌트")]
    [SerializeField] private TextMeshProUGUI slotTitleNameText;
    [SerializeField] private TextMeshProUGUI slotTimeText;

    private void Start()
    {
        SetTextMeshPro();
    }

    private void OnEnable()
    {
        // SetTextMeshPro();
    }

    private void SetTextMeshPro()
    {
        slotTitleNameText.text = JsonManager.instance.saveDataDictionary[this.gameObject.name].name;
        slotTimeText.text = JsonManager.instance.saveDataDictionary[this.gameObject.name].saveTime;
    }

    public void ClearSlot()
    {
        JsonManager.instance.ClearSlot(this.gameObject.name);
        SetTextMeshPro();
    }

    public void UpdateSlotText()
    {
        SetTextMeshPro();
    }

    // 마우스 클릭(Click) 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 클릭을 하였을 때
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            print($"{eventData.button}");
            
            JsonManager.instance.SelectSlot(this.gameObject.name);
            SetTextMeshPro();
        }
    }
}

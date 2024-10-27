using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        slotTitleNameText.text = SaveDataDictionary.s_SaveDataDictionary[this.gameObject.name].name;
        slotTimeText.text = SaveDataDictionary.s_SaveDataDictionary[this.gameObject.name].saveTime;
    }

    public void ClearSlot()
    {
        JsonManager.Instance.ClearSlot(this.gameObject.name);
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
            print($"{this.gameObject.name}");
            print("세이브 슬롯 마우스 좌클릭으로 선택됨.");
            
            JsonManager.Instance.SelectSlot(this.gameObject.name);
            SetTextMeshPro();
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveRightPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI slotTitleName;
    [SerializeField] private TextMeshProUGUI slotSaveTime;
    [SerializeField] private TextMeshProUGUI slotContents;

    private string _currentClickSlot;

    public void ClickSaveButton()
    {
        if (_currentClickSlot != null)
        {
            print($"{_currentClickSlot} ClickSaveButton");
            // JsonManager.Instance.Save(); //_currentClickSlot

            SaveSlot currentSaveSlot = GameObject.Find(_currentClickSlot).GetComponent<SaveSlot>();
            currentSaveSlot.UpdateSlotText();
        }
    }

    public void ClickLoadButton()
    {
        
    }

    public void ShowRightPanel(string saveSlotKey, string titleName, string saveTime, string contents)
    {
        _currentClickSlot = saveSlotKey;
        slotTitleName.text = titleName;
        slotSaveTime.text = saveTime;
        slotContents.text = contents;
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class QuestSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("↓ 자식 오브젝트의 컴포넌트")]
    [SerializeField] private TextMeshProUGUI _slotQuestTitleName;
    public string SlotQuestTitleName { get { return _slotQuestTitleName.text; } }
    [SerializeField] private GameObject _slotQuestCompleteStamp;
    [SerializeField] private GameObject _slotQuestQuestCompleteImage;

    // QuestSlot이 생성될 때 아래의 Init을 불러야 한다.
    public void Init(int questID, string setQuestTitleName)
    {
        _slotQuestTitleName.text = questID + ". " + setQuestTitleName;
        _slotQuestCompleteStamp.SetActive(false);
        _slotQuestQuestCompleteImage.SetActive(false);
    }

    // 퀘스트를 클리어하면 해당 함수를 호출해야 한다.
    public void SetCompleteQuestUIActive(bool isActive)
    {
        _slotQuestCompleteStamp.SetActive(isActive);
        _slotQuestQuestCompleteImage.SetActive(isActive);
    }

    // 마우스 클릭(Click) 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 왼쪽 클릭을 하였을 때
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"{this.gameObject.name}");
            Debug.Log("퀘스트 슬롯 마우스 좌클릭으로 선택됨.");
            
            // 해당 오브젝트를 QuestMenu에 콜백으로 넘겨준다.
            QuestMenu.ClickSlot(this);
        }
    }
}

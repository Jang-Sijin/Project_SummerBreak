using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class DialogUiController : MonoBehaviour
{
	[Header("[다이얼로그 UI]")]
	[Header("다이얼로그가 끝난 후 이벤트 등록은 해당 스크립트에 구현")]
	public Canvas npcDialogCanvas;				// 캔버스 UI
	public TextMeshProUGUI nameText;			// 현재 대사중인 캐릭터 이름 출력 Text UI
	public TextMeshProUGUI dialogText;			// 대화창 Image UI
	public GameObject objectArrow;				// 대사가 완료되었을 때 출력되는 커서 오브젝트
	public Button yesBtn;						// 퀘스트 수락

#if UNITY_EDITOR
	//private void OnValidate()
	//{
	//	npcDialogCanvas = npcDialogCanvas ?? GameObject.Find("NpcDialog_Canvas").GetComponent<Canvas>();
	//	nameText = nameText ?? GameObject.Find("NpcDialog_Name_Text").GetComponent<TextMeshProUGUI>();
	//	dialogText = dialogText ?? GameObject.Find("NpcDialog_Dialogue_Text").GetComponent<TextMeshProUGUI>();
	//	yesBtn = yesBtn ?? GameObject.Find("NpcDialog_Accept_Btn").GetComponent<Button>();
	//	noBtn = noBtn ?? GameObject.Find("NpcDialog_Refuse_Btn").GetComponent<Button>();
	//
	//	CheckComponentValidate();
	//}
#endif

	void CheckComponentValidate()
	{
		Debug.Assert(npcDialogCanvas, $"canvas is null");
		Debug.Assert(nameText, $"nameText is null");
		Debug.Assert(dialogText, $"dialogText is null");
		Debug.Assert(objectArrow, $"objectArrow is null");
		Debug.Assert(yesBtn, $"yesBtn is null");
	}

	public void Init()
	{
		CheckComponentValidate();
		
		// Dialogue UI 모두 비활성화
		CanvasClose();
		SetActiveArrowObject(false);
		SetActiveButtonObjects(false);
		SetActiveTextObjects(false);
		
		yesBtn.onClick.AddListener(ClickAcceptButton);
	}

	public void SetActiveTextObjects(bool visible)
	{
		nameText.gameObject.SetActive(visible);
		dialogText.gameObject.SetActive(visible);
	}
	
	public void SetActiveArrowObject(bool visible)
	{
		objectArrow.gameObject.SetActive(visible);
	}

	public void SetActiveButtonObjects(bool visible)
	{
		yesBtn.gameObject.SetActive(visible);
	}
	
	public void CanvasOpen()
	{
		npcDialogCanvas.gameObject.SetActive(true);
	}

	public void CanvasClose()
	{
		npcDialogCanvas.gameObject.SetActive(false);
	}
	
	// 캐릭터 알파 값 변경
	// Color color = speaker.spriteImage.color;
	// color.a = visible == true ? 1 : 0.2f;
	// speaker.spriteImage.color = color;

	/*public void StartTask(List<NpcDialogDBEntity> taskData)
	{
		Debug.Assert(taskData != null, $"task Data is null");
		Debug.Assert(0 < taskData.Count, $"task Data is null");
		npcDialogCanvas.gameObject.SetActive(true);
	
		_taskDatas = taskData;
	
		yesBtn.gameObject.SetActive(false);
		noBtn.gameObject.SetActive(false);

		StartCoroutine(TaskCo());
	}

	IEnumerator TaskCo()
	{
		foreach (var task in _taskDatas)
		{
			nameText.text = $"{task.Name}";
			dialogText.text = $"{task.Dialog}";
	
			yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
			yield return null;
		}
		yesBtn.gameObject.SetActive(true);
		noBtn.gameObject.SetActive(true);
	}*/

	public void AcceptQuest()
	{
		//QuestSystem 
		//List<int> _questList;
		//AddQeust 해서
		//Quest ID만 추가시켜주면
		//Type
		
		// Monster 
		// Event _dieEvent;
		// _dieEvent();
		// _dieEvent += QuestCheck;
		// _dieEvent += AddExp;

		//QuestCheck
		//monster id 
		// 내가가진 퀘스트중에 Type이 0인거 찍고
		// 그중에서 Param1이 내 ID랑 같으면 
		// 값하나추가시켜서 DB저장


		//몬스터가죽으면 --> CallBack 에  퀘스트체크도 하나넣어주는거임 
		//
	}

	public void ClickAcceptButton()
	{
		// 퀘스트 다이얼로그가 끝난 뒤 수락 버튼을 눌렀을 경우 AcceptQuest 함수를 수행한다.
		if (QuestSystem.instance.IsProgressQuest == true)
		{
			// 퀘스트 받을 때
			QuestSystem.instance.AcceptQuest();
		}
	}

	public void ClickCancelButton()
	{
		// [No 버튼는 기획 설계상 삭제되었습니다. 다음 업데이트에서 해당 함수 삭제 필요.]
		// 다이얼로그 No 버튼을 눌렀을 경우 // 대화 종료 이벤트 발생
		// QuestSystem.Instance.IsQuestDialog = false;
	}
}

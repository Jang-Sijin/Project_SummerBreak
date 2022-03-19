using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class DialogUiController : MonoBehaviour
{
	public Canvas npcDialogCanvas;				// 캔버스 UI
	public TMPro.TextMeshProUGUI nameText;		// 현재 대사중인 캐릭터 이름 출력 Text UI
	public TMPro.TextMeshProUGUI dialogText;	// 대화창 Image UI
	public GameObject objectArrow;				// 대사가 완료되었을 때 출력되는 커서 오브젝트
	public Button yesBtn;						// 퀘스트 수락
	public Button noBtn;						// 퀘스트 거절

#if UNITY_EDITOR
	private void OnValidate()
	{
		npcDialogCanvas = npcDialogCanvas ?? GameObject.Find("NpcDialog_Canvas").GetComponent<Canvas>();
		nameText = nameText ?? GameObject.Find("NpcDialog_Name_Text").GetComponent<TMPro.TextMeshProUGUI>();
		dialogText = dialogText ?? GameObject.Find("NpcDialog_Main_Text").GetComponent<TMPro.TextMeshProUGUI>();
		yesBtn = yesBtn ?? GameObject.Find("YesBtn").GetComponent<Button>();
		noBtn = noBtn ?? GameObject.Find("NoBtn").GetComponent<Button>();

		CheckComponentValidate();
	}
#endif

	void CheckComponentValidate()
	{
		Debug.Assert(npcDialogCanvas, $"canvas is null");
		Debug.Assert(nameText, $"nameText is null");
		Debug.Assert(dialogText, $"dialogText is null");
		Debug.Assert(objectArrow, $"objectArrow is null");
		Debug.Assert(yesBtn, $"yesBtn is null");
		Debug.Assert(noBtn, $"noBtn is null");
	}

	public void Init()
	{
		CheckComponentValidate();
		
		// Dialogue UI 모두 비활성화
		CanvasClose();
		SetActiveArrowObject(false);
		SetActiveButtonObjects(false);
		SetActiveTextObjects(false);
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
		noBtn.gameObject.SetActive(visible);
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
}

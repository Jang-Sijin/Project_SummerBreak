using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class NpcDialogSystem : MonoBehaviour
{
	[SerializeField]
	private Canvas npcDialogCanvas;
	[SerializeField]
	private TMPro.TextMeshProUGUI nameText;
	[SerializeField]
	private TMPro.TextMeshProUGUI dialogText;
	[SerializeField]
	private Button yesBtn;
	[SerializeField]
	private Button noBtn;

	private List<QuestDialogDBEntity> taskDatas = new List<QuestDialogDBEntity>();

#if UNITY_EDITOR
	private void OnValidate()
	{
		nameText = nameText ?? GameObject.Find("NameText").GetComponent<TMPro.TextMeshProUGUI>();
		dialogText = dialogText ?? GameObject.Find("DialogTask").GetComponent<TMPro.TextMeshProUGUI>();
		yesBtn = yesBtn ?? GameObject.Find("YesBtn").GetComponent<Button>();
		noBtn = noBtn ?? GameObject.Find("NoBtn").GetComponent<Button>();
		npcDialogCanvas = npcDialogCanvas ?? this.transform.GetComponentInParent<Canvas>();

		CheckComponentValidate();
	}
#endif

	void CheckComponentValidate()
	{
		Debug.Assert(nameText, $"_nameText is null");
		Debug.Assert(dialogText, $"_dialogText is null");
		Debug.Assert(yesBtn, $"_yesBtn is null");
		Debug.Assert(noBtn, $"_noBtn is null");
		Debug.Assert(npcDialogCanvas, $"_canvas is null");
	}

	public void Init()
	{
		CheckComponentValidate();
	}

	public void StartTask(List<QuestDialogDBEntity> taskData)
	{
		Debug.Assert(taskData != null, $"task Data is null");
		Debug.Assert(0 < taskData.Count, $"task Data is null");
		npcDialogCanvas.gameObject.SetActive(true);

		taskDatas = taskData;

		yesBtn.gameObject.SetActive(false);
		noBtn.gameObject.SetActive(false);

		StartCoroutine(TaskCo());
	}

	IEnumerator TaskCo()
	{
		foreach (var task in taskDatas)
		{
			nameText.text = $"{task.name}";
			dialogText.text = $"{task.dialog}";

			yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
			yield return null;
		}
		yesBtn.gameObject.SetActive(true);
		noBtn.gameObject.SetActive(true);
	}

	public void Open()
    {
		npcDialogCanvas.gameObject.SetActive(true);
	}

	public void Close()
	{
		npcDialogCanvas.gameObject.SetActive(false);
	}

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

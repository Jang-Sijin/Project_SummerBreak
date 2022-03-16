using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DialogSystem : MonoBehaviour
{
	[Header("↓숫자가 작아질수록 타이핑 속도가 빨라집니다.")] 
	[SerializeField] private float typingSpeed = 0.05f; // 텍스트 타이핑 효과의 재생 속도
	[SerializeField] private bool isAutoStart = true; // 자동 시작 여부
	
	[Header("↓NpcDialogSystem 오브젝트를 연결해주세요.")]
	[SerializeField] private DialogUiController dialogUi; // 대화창(DialogCanvas) UI 컨트롤

	private bool _isFirst = true; // 최초 1회만 호출하기 위한 변수
	private int _currentDialogIndex = -1; // 현재 대사 순번
	private bool _isTypingEffect = false; // 텍스트 타이핑 효과를 재생중인지
	
	#region Singleton
	public static DialogSystem instance; // DialogSystem을 싱글톤으로 관리
	private void Awake()
	{
		// Dialog System 싱글톤 설정
		if (instance == null)
		{
			instance = this;
		}
	}
	#endregion Singleton

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		dialogUi.Init();
	}

	public bool UpdateDialog(List<NpcDialogDBEntity> dialogList)
	{
		// 대사 분기가 시작될 때 1회만 호출
		if (_isFirst == true)
		{
			// 초기화. 캐릭터 이미지는 활성화하고, 대사 관련 UI는 모두 비활성화
			Setup();

			// 자동 재생(isAutoStart=true)으로 설정되어 있으면 첫 번째 대사 재생
			if ( isAutoStart ) SetNextDialog(dialogList);

			_isFirst = false;
		}

		if (Input.GetMouseButtonDown(0)) // 마우스 버튼을 누르면 조건 실행
		{
			// 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
			if (_isTypingEffect == true)
			{
				_isTypingEffect = false;

				// 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
				StopCoroutine("OnTypingText");
				dialogUi.dialogText.text = dialogList[_currentDialogIndex].Dialog;

				// 대사가 완료되었을 때 출력되는 커서 활성화
				dialogUi.SetActiveArrowObject(true);

				return false;
			}

			// 대사가 남아있을 경우 다음 대사 진행
			if (dialogList.Count > _currentDialogIndex + 1)
			{
				SetNextDialog(dialogList);
			}
			else
			{
				// 대사가 더 이상 없을 경우 확인 버튼 또는 퀘스트 수락, 퀘스트 거절 버튼을 활성화
				dialogUi.SetActiveButtonObjects(true);
				return true;
			}
		}
		return false;
	}
	
	private void SetNextDialog(List<NpcDialogDBEntity> dialogList)
	{
		// 1. 대화 UI창이 활성화가 안되었으면 활성화한다.
		OpenDialogUi();
		
		// 2. 대사가 시작될 때는 Arrow UI가 나오지 않도록 설정
		dialogUi.SetActiveArrowObject(false);
		
		// 3. 다음 대사를 진행할 수 있도록 Index증가
		_currentDialogIndex++;

		// 4. 현재 화자 이름 텍스트 설정
		dialogUi.nameText.text = dialogList[_currentDialogIndex].Name;
		
		// 5. 현재 화자의 대사 텍스트 설정
		dialogUi.dialogText.text = dialogList[_currentDialogIndex].Dialog;
		
		// 6. 타이핑 효과 코루틴 실행
		StartCoroutine("OnTypingText", dialogList);
	}

	private IEnumerator OnTypingText(List<NpcDialogDBEntity> dialogList)
	{
		int index = 0;
		_isTypingEffect = true;

		// 텍스트의 끝까지 // 텍스트를 한글자씩 타이핑치듯 재생
		while ( index <= dialogList[_currentDialogIndex].Dialog.Length )
		{
			dialogUi.dialogText.text = dialogList[_currentDialogIndex].Dialog.Substring(0, index);

			index++;
		
			// 타이핑 속도
			yield return new WaitForSeconds(typingSpeed);
		}
		_isTypingEffect = false;

		// 대사가 완료되었을 때 출력되는 화살표 애니메이션 오브젝트 활성화
		dialogUi.SetActiveArrowObject(true);
	}

	private void OpenDialogUi()
	{
		// 캔버스가 활성화되어 있지 않으면 실행
		if (dialogUi.npcDialogCanvas.gameObject.activeSelf == false)
		{
			dialogUi.CanvasOpen();
			dialogUi.SetActiveTextObjects(true);
		}
	}

	private void CloseDialogUi()
	{
		// 캔버스가 활성화되어 있으면 UI를 비활성화 되도록 변경
		if (dialogUi.npcDialogCanvas.gameObject.activeSelf == true)
		{
			dialogUi.CanvasClose();
			dialogUi.SetActiveTextObjects(false);
		}
	}

	public void ResetDialog()
	{
		StopCoroutine("OnTypingText");
		_isFirst = true; // 최초 1회만 호출하기 위한 변수
		_currentDialogIndex = -1; // 현재 대사 순번
		_isTypingEffect = false; // 텍스트 타이핑 효과를 재생중인지
		CloseDialogUi();
	}
}
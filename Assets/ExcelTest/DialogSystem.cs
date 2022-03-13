using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogSystem: MonoBehaviour
{
	[SerializeField] private int branch;
	[SerializeField] private DialogDB dialogDB;
	
	[Header("���ڰ� �۾������� Ÿ���� �ӵ��� �������ϴ�.")]
	[SerializeField]
	private	float				typingSpeed = 0.1f;			// �ؽ�Ʈ Ÿ���� ȿ���� ��� �ӵ�
	[SerializeField]
	private	bool				isAutoStart = true;			// �ڵ� ���� ����
	[SerializeField]
	private	Speaker[]			speakers;					// ��ȭ�� �����ϴ� ĳ���͵��� UI �迭
	[SerializeField]
	private	List<DialogData>	dialogs;					// ���� �б��� ��� ��� ����Ʈ
	private	bool				isFirst = true;				// ���� 1ȸ�� ȣ���ϱ� ���� ����
	private	int					currentDialogIndex = -1;	// ���� ��� ����
	private	int					currentSpeakerIndex = 0;	// ���� ���� �ϴ� ȭ��(Speaker)�� speakers �迭 ����
	private	bool				isTypingEffect = false;		// �ؽ�Ʈ Ÿ���� ȿ���� ���������

	private void Awake()
	{
		for (int i = 0; i < dialogDB.TutorialSheet.Count; ++i)
		{
			if (dialogDB.TutorialSheet[i].branch == branch)
			{
				DialogData dialogData;
				dialogData.speakerIndex = dialogDB.TutorialSheet[i].speaker;
				dialogData.name = dialogDB.TutorialSheet[i].name;
				dialogData.dialogue = dialogDB.TutorialSheet[i].dialog;
				
				dialogs.Add(dialogData);
			}
		}
		
		Setup();
	}

	private void Setup()
	{
		// ��� ��ȭ ���� ���ӿ�����Ʈ ��Ȱ��ȭ
		for ( int i = 0; i < speakers.Length; ++ i )
		{
			SetActiveObjects(speakers[i], false);
			
			// ĳ���� �̹����� ���̵��� ����
			speakers[i].spriteImage.gameObject.SetActive(true);
		}
	}

	public bool UpdateDialog()
	{
		// ��� �бⰡ ���۵� �� 1ȸ�� ȣ��
		if ( isFirst == true )
		{
			// �ʱ�ȭ. ĳ���� �̹����� Ȱ��ȭ�ϰ�, ��� ���� UI�� ��� ��Ȱ��ȭ
			Setup();

			// �ڵ� ���(isAutoStart=true)���� �����Ǿ� ������ ù ��° ��� ���
			if ( isAutoStart ) SetNextDialog();

			isFirst = false;
		}

		if ( Input.GetMouseButtonDown(0) )
		{
			// �ؽ�Ʈ Ÿ���� ȿ���� ������϶� ���콺 ���� Ŭ���ϸ� Ÿ���� ȿ�� ����
			if ( isTypingEffect == true )
			{
				isTypingEffect = false;
				
				// Ÿ���� ȿ���� �����ϰ�, ���� ��� ��ü�� ����Ѵ�
				StopCoroutine("OnTypingText");
				speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
				// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
				speakers[currentSpeakerIndex].objectArrow.SetActive(true);

				return false;
			}

			// ��簡 �������� ��� ���� ��� ����
			if ( dialogs.Count /*Length*/ > currentDialogIndex + 1 )
			{
				SetNextDialog();
			}
			// ��簡 �� �̻� ���� ��� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ� true ��ȯ
			else
			{
				// ���� ��ȭ�� �����ߴ� ��� ĳ����, ��ȭ ���� UI�� ������ �ʰ� ��Ȱ��ȭ
				for ( int i = 0; i < speakers.Length; ++ i )
				{
					SetActiveObjects(speakers[i], false);
					
					// SetActiveObjects()�� ĳ���� �̹����� ������ �ʰ� �ϴ� �κ��� ���� ������ ������ ȣ��
					speakers[i].spriteImage.gameObject.SetActive(false);
				}

				return true;
			}
		}

		return false;
	}

	private void SetNextDialog()
	{
		// ���� ȭ���� ��ȭ ���� ������Ʈ ��Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], false);

		// ���� ��縦 �����ϵ��� 
		currentDialogIndex ++;

		// ���� ȭ�� ���� ����
		currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;

		// ���� ȭ���� ��ȭ ���� ������Ʈ Ȱ��ȭ
		SetActiveObjects(speakers[currentSpeakerIndex], true);
		// ���� ȭ�� �̸� �ؽ�Ʈ ����
		speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
		
		// ���� ȭ���� ��� �ؽ�Ʈ ����
		//speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
		StartCoroutine("OnTypingText");
	}

	private void SetActiveObjects(Speaker speaker, bool visible)
	{
		speaker.imageDialog.gameObject.SetActive(visible);
		speaker.textName.gameObject.SetActive(visible);
		speaker.textDialogue.gameObject.SetActive(visible);

		// ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� false
		speaker.objectArrow.SetActive(false);

		// ĳ���� ���� �� ����
		Color color = speaker.spriteImage.color;
		color.a = visible == true ? 1 : 0.2f;
		speaker.spriteImage.color = color;
	}

	private IEnumerator OnTypingText()
	{
		int index = 0;
		
		isTypingEffect = true;

		// �ؽ�Ʈ�� ������ // �ؽ�Ʈ�� �ѱ��ھ� Ÿ����ġ�� ���
		while ( index <= dialogs[currentDialogIndex].dialogue.Length )
		{
			speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue.Substring(0, index);

			index ++;
		
			// Ÿ���� �ӵ�
			yield return new WaitForSeconds(typingSpeed);
		}

		isTypingEffect = false;

		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� Ȱ��ȭ
		speakers[currentSpeakerIndex].objectArrow.SetActive(true);
	}
}

[Serializable]
public struct Speaker
{
	public	Image	        spriteImage;		// ĳ���� �̹��� (û��/ȭ�� ���İ� ����)
	public	Image			imageDialog;		// ��ȭâ Image UI
	public	TextMeshProUGUI	textName;			// ���� ������� ĳ���� �̸� ��� Text UI
	public	TextMeshProUGUI	textDialogue;		// ���� ��� ��� Text UI
	public	GameObject		objectArrow;		// ��簡 �Ϸ�Ǿ��� �� ��µǴ� Ŀ�� ������Ʈ
}

[Serializable]
public struct DialogData
{
	public	int		speakerIndex;	// �̸��� ��縦 ����� ���� DialogSystem�� speakers �迭 ����
	public	string	name;			// ĳ���� �̸�
	[TextArea(3, 5)]
	public	string	dialogue;		// ���
}


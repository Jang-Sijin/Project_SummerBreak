using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ExcelDB : ScriptableObject
{
	public List<DialogDBEntity> TutorialSheet; // Tutorial 시트
	public List<NpcDialogDBEntity> DialogSheet; // NPC 다이얼로그 시트
	public List<ObjDialogDBEntity> ObjectDialogSheet; // Object 다이얼로그(정보 출력) 시트
	public List<QuestDBEntity> QuestDBSheet; // 퀘스트 DB 시트
} 

//////////////////////////////////////////////////////////
/// 아래부터는 DB의 구조체 형식을 정의한 직렬화된 구조체입니다. ///
//////////////////////////////////////////////////////////

[System.Serializable]
public struct DialogDBEntity
{
	public int branch;      // 몇 번째 스토리인가? 
	public int speaker;     // spearkers의 Element 번호
	public string name;     // 말하는 인물의 이름
	public string dialog;   // 대화 내용(엑셀시트)
}

[System.Serializable]
public struct NpcDialogDBEntity
{
	public int DialogID;
	public string Name;
	public string Dialog;
}

[System.Serializable]
public struct ObjDialogDBEntity
{
	public int DialogID;
	public string Name;
	public string ObjectDialog;
}

[System.Serializable]
public struct QuestDBEntity
{
	public int QuestID;
	public int StartDialogID;
	public int EndDialogID;
	public int QuestType;
	public string NpcName;
	public string QuestTitle;
	public string QuestContent;
	public string QuestReward;
}
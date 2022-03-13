using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DialogDB : ScriptableObject
{
	public List<DialogDBEntity> TutorialSheet;
	public List<QuestDialogDBEntity> QuestSheet;
}

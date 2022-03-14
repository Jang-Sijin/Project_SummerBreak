using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class ExcelDB : ScriptableObject
{
	public List<DialogDBEntity> TutorialSheet; // Replace 'EntityType' to an actual type that is serializable.
	public List<NpcDialogDBEntity> DialogSheet; // Replace 'EntityType' to an actual type that is serializable.
}

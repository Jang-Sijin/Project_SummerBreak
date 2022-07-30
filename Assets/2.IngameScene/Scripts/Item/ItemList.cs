using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New ItemList")] // 마우스 우클릭 + Create에서 추가할 수 있습니다.
public class ItemList : ScriptableObject
{
    public Item[] ItemDBList;
}
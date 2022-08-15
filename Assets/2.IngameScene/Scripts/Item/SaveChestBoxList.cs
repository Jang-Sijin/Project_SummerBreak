using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveChestBoxList : MonoBehaviour
{
    private Dictionary<string, bool> chestBoxList = new Dictionary<string, bool>();

    public Dictionary<string, bool> SaveMapChestBoxList()
    {
        OpenChestCoin[] allChestBoxList = gameObject.GetComponentsInChildren<OpenChestCoin>();

        foreach (var boxObj in allChestBoxList)
        {
            chestBoxList.Add(boxObj.name, boxObj.hasBeenCollected);
        }

        return chestBoxList;
    }
    
    public void LoadMapChestBoxList(Dictionary<string, bool> loadChestBoxList)
    {
        OpenChestCoin[] allChestBoxList = gameObject.GetComponentsInChildren<OpenChestCoin>();

        foreach (var boxObj in allChestBoxList)
        {
            loadChestBoxList.TryGetValue(boxObj.name, out boxObj.hasBeenCollected);

            if (boxObj.hasBeenCollected == true)
            {
                boxObj.OpenAnimation();
            }   
        }
    }
}

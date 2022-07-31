using UnityEngine;

public class NPCDialogCompulsion : MonoBehaviour
{
    [SerializeField] private NpcDialogTrigger _npcDialogTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventorySystem.instance.FindInventorySlotItem("작은 검") == 0 &&
                _npcDialogTrigger.ReturnDialogCompulsionCheck())
            {
                _npcDialogTrigger.EnterPlayer();
            }
        }
    }
}

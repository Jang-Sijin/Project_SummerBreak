using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item itemDB;
    
    [SerializeField] private AudioClip takeItemAudioClip;
    [SerializeField] private Inventory playerInventory;
    
    private void OnTriggerEnter(Collider other) // 콜라이더 박스 -> 충돌이 일어났을 때 트리거 발생
    {
        if (other.CompareTag("Player")) // 플레이어 Tag와 충돌했을 때
        {
            CanPickUp();
            InfoDisappear();
            // Debug.Log("[장시진] OnTriggerEnter Item & Player");
        }
    }

    private void CanPickUp()
    {
        SoundManager.instance.SfxPlay("TakeItemAudio", takeItemAudioClip); // 아이템 획득 효과음 실행
        GameManager.instance.AcquireItem(itemDB);
        // playerInventory.AcquireItem(itemDB); [위의 코드로 바꿈 -> 더이상 안씀]

        Debug.Log($"[장시진] {itemDB.itemName} 아이템 획득!");
    }
    
    private void InfoDisappear()
    {
        Destroy(this.gameObject); // 아이템 오브젝트 삭제
    }
}

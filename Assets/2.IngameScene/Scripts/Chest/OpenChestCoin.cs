using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChestCoin : MonoBehaviour
{    
    [SerializeField] 
    private List<GameObject> coinObjList = new List<GameObject>();

    [SerializeField] 
    private Transform spawnPoint;

    // 상자가 열려있는지 상태를 체크한다.
    // false: 닫힘, true: 열림
    public bool hasBeenCollected = false;

    public void OpenStart()
    {
        if (!hasBeenCollected)
        {
            Loot();
            hasBeenCollected = true;
        }
    }

    public void StartChestOpenSFX()
    {
        SoundManager.Instance.PlaySFX(5);
    }
    
    private void Loot()
    {
        //Debug.Log($"[이민호] number:{number}");
        if (coinObjList.Count > 0)
        {
            StartCoroutine(CreateLoot(coinObjList.Count));
        }
        //Debug.Log("[이민호] 한번");
    }

    IEnumerator CreateLoot(int number)
    {
        for (int i = number; i > 0; i--)
        {
            GameObject tempLoot = Instantiate(coinObjList[i - 1]);
            tempLoot.transform.position = spawnPoint.position;
            yield return new WaitForSeconds(0.001f);
        }
    }

    public bool CheckStateChestBox()
    {
        if (!hasBeenCollected)
        {
            // 박스 열리는 효과음 수행.
            StartChestOpenSFX();
            // 박스 열리는 애니메이션 수행.
            OpenAnimation();

            return true;
        }
        else
        {
            return false;
        }
    }
    public void OpenAnimation()
    {
        this.GetComponent<Animator>().SetBool("IsOpen", true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerHpImageArrayParent;
    [SerializeField] private GameObject playerStaminaImageArrayParent;
    [SerializeField] private TextMeshProUGUI playerCoinTextMesh;

    private Image[] playerHpImageArray;
    private Image[] playerStaminaImageArray;

    private void Start()
    {
        // HP, Stamina 이미지들을 각각의 배열로 관리한다.
        playerHpImageArray = playerHpImageArrayParent.GetComponentsInChildren<Image>();
        playerStaminaImageArray = playerStaminaImageArrayParent.GetComponentsInChildren<Image>();

        playerCoinTextMesh.text = InventorySystem.instance.GetPlayerCoinCount().ToString();
    }

    public void UpdatePlayerCoinCountUI()
    {
        playerCoinTextMesh.text = InventorySystem.instance.GetPlayerCoinCount().ToString();
    }
}

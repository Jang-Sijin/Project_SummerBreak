using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOptioKeyInput : MonoBehaviour
{

    [SerializeField] private List<GameObject> menuList = new List<GameObject>();
    [SerializeField] private GameObject InventoryUi;
    
    private bool qKeyDown = false;
    private bool eKeyDown = false;

    [SerializeField] private int curOptionNumber = 0;

    [SerializeField] private GameObject saveMenu;
    [SerializeField] private GameObject keyMenu;
    [SerializeField] private GameObject audioMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!saveMenu.activeSelf && !keyMenu.activeSelf && !audioMenu.activeSelf)
        {
            curOptionNumber = CurrentOptionNumber();
            
            GetInput();
            
            Interaction();
        }
    }

    private int CurrentOptionNumber()
    {
        for (int i = 0; i < menuList.Count; ++i)
        {
            if (menuList[i].activeSelf)
            {
                return i;
            }
        }

        return menuList.Count + 1;
    }
    
    private void GetInput()
    {
        if (!eKeyDown)
        {
            qKeyDown = Input.GetButtonDown("OptionNext");
        }
        
        if (!qKeyDown)
        {
            eKeyDown = Input.GetButtonDown("OptionPre");
        }
        
    }
    
    private void Interaction()
    {
        
        if (qKeyDown)
        {
            NextMenu();
            SoundManager.Instance.PlaySFX(3);
        }
        else if (eKeyDown)
        {
            PreMenu();
            SoundManager.Instance.PlaySFX(3);
        }

        if (menuList[menuList.Count - 1].activeSelf && !InventoryUi.activeSelf)
        {
            InventoryUi.SetActive(true);
        }
        else if (!menuList[menuList.Count - 1].activeSelf && InventoryUi.activeSelf)
        {
            InventoryUi.SetActive(false);
        }
    }

    private void NextMenu()
    {
        menuList[curOptionNumber].SetActive(false);
        
        if (curOptionNumber == menuList.Count - 1)
        {
            menuList[0].SetActive(true);
        }
        else
        {
            menuList[curOptionNumber + 1].SetActive(true);
        }
    }

    private void PreMenu()
    {
        menuList[curOptionNumber].SetActive(false);
        
        if (curOptionNumber == 0)
        {
            menuList[menuList.Count - 1].SetActive(true);
            InventoryUi.SetActive(true);
        }
        else
        {
            menuList[curOptionNumber - 1].SetActive(true);
        }
    }
    
}

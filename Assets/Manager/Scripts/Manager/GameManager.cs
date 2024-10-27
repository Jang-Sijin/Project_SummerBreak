using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // [변수]
    //---------------------------------------------------------------------------------
    
    [Header("플레이어 설정")]
    public GameObject playerGameObject;
    public Transform loadPlayerTransform; // "주의: 디버그 출력용입니다."
    
    [Header("마우스 커서 설정")]
    [SerializeField] private float cursorDisappearTime;
    private float cursorCheckStartTime;
    private Vector3 beforeMousePosition;
    private bool isMoveMouse = true;
    [SerializeField] private Texture2D cursorDefault;
    [SerializeField] private Texture2D cursorClick;

    [Header("인게임 시간 정보")] 
    [SerializeField] private TimeController timeController;

    [Header("플레이어 UI[체력/스테미너/코인]")] public GameObject PlayerUI;
    
    [Header("사운드 슬라이드바 설정")]
    [SerializeField] private Slider bgSoundSlider;
    [SerializeField] private Slider effectSoundSlider;

    [Header("필드 아이템 리스트")] 
    public SaveItemList saveItemList;

    [Header("필드 상자상태 리스트")] 
    public SaveChestBoxList saveChestBoxList;
    
    //---------------------------------------------------------------------------------
    
    #region Game Manager 싱글톤 설정
    public static GameManager instance; // Game Manager을 싱글톤으로 관리
    private void Awake()
    {
        // Game Manager 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(Instance);
        } 
        else
        {
            // 이미 Game Manager가 존재할 때 오브젝트 파괴 
            Destroy(this.gameObject);  
        }
    }
    #endregion

    private void Start()
    {
        // 저장 데이터 불러오기
        InitLoadSaveData();
        
        // 마우스 이벤트 설정
        InitMouseEvent();

        // 사운드바 슬라이드 위치 설정
        bgSoundSlider.value = SoundManager.Instance.GetBgSoundVolumeValue();
        effectSoundSlider.value = SoundManager.Instance.GetSfxSoundVolumeValue();
    }

    // 인게임 시간 정지
    public void InGameTimeStop() => Time.timeScale = 0;
    // 인게임 시간 시작
    public void InGameTimeStart() => Time.timeScale = 1;

    // 게임 종료
    public void QuitGame() => Application.Quit();

    private void InitMouseEvent()
    {
        // 시작시 마우스 커서 기본으로 설정
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
        // 시작시 마우스 위치 현재 마우스 위치로 설정
        beforeMousePosition = Input.mousePosition;
        // 마우스 커서 이미지 할당 (Default:기본)
        StartCoroutine(SetMouseCursor());
    }

    // 마우스 설정
    private IEnumerator SetMouseCursor()
    {
        while (true)
        {
            // 마우스 이벤트 체크
            EventCheckMouse();
            CheckMouseMove();

            yield return null;
        }
    }

    private void EventCheckMouse()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭을 했을 때
        {
            // 마우스 커서 이미지 cursorClick로 바꿈
            Cursor.SetCursor(cursorClick, Vector2.zero, CursorMode.Auto);
            
            // 이벤트가 있으면 마우스의 움직임이 있다는 것.
            isMoveMouse = true;
        }
        if (Input.GetMouseButtonUp(0)) // 마우스 좌클릭을 뗏을 때
        {
            // 마우스 커서 이미지 cursorDefault 바꿈
            Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
            
            // 이벤트가 있으면 마우스의 움직임이 있다는 것.
            isMoveMouse = false;
        }
    }

    private void CheckMouseMove()
    {
        if (beforeMousePosition.Equals(Input.mousePosition) && isMoveMouse)
        {
            // 마우스가 움직이고 있지 않음
            isMoveMouse = false;
            
            // 시간 카운트 시작
            cursorCheckStartTime = Time.time;
        }
        else if(!beforeMousePosition.Equals(Input.mousePosition))
        {
            // Debug.Log($"[장시진] 이전 마우스 위치: {beforeMousePosition}, 현재 마우스 위치: {Input.mousePosition}");
            
            // 마우스가 움직이고 있음
            isMoveMouse = true;
            // 마우스의 위치가 이동되었으면 현재 마우스 위치를 받는다. 
            beforeMousePosition = Input.mousePosition;
        }

        // Time.time - cursorCheckStartTime -> 현재 시간과 마우스가 움직이지 않았던 시간을 빼면  
        if (!isMoveMouse && Time.time - cursorCheckStartTime > cursorDisappearTime)
        {
            // Debug.Log($"[장시진] {Time.time - cursorCheckStartTime}초 동안 마우스 이동이 없습니다. (커서 숨김)");
            
            // 마우스를 보이지 않도록 설정한다.
            Cursor.visible = false;
        }
        else if(isMoveMouse)
        {
            Cursor.visible = true;
            cursorCheckStartTime = 0.0f;
        }
    }
    
    public DateTime GetInGameTime()
    {
        return timeController.InGameTime();
    }

    public void SetInGameTime(int hour)
    {
        timeController.SetInGameTime(hour);
    }

    public void SetTimeMultiplier(int timeSpeed)
    {
        timeController.SetTimeMultiplier(timeSpeed);
    }
    
    private void InitLoadSaveData()
    {
        // 로드할 데이터가 없을 때
        if (!JsonManager.Instance.CheckSaveFile())
        {
            Debug.Log($"[장시진] 불러올 데이터가 없습니다. 새로운 게임을 시작합니다.");
            return;
        }

        // 다이얼로그 시스템 초기화
        DialogSystem.instance.Setup();

        print($"[장시진] 불러올 데이터가 있습니다.");
        SaveInfo saveinfo = JsonManager.Instance.LoadSaveFile();

        // 플레이어 데이터 로드
        playerGameObject.transform.position = saveinfo.position;
        playerGameObject.transform.rotation = Quaternion.Euler(saveinfo.rotation);
        playerGameObject.GetComponent<PlayerStatus>().currentHealth = saveinfo.hp;
        playerGameObject.GetComponent<PlayerStatus>().currentMaxstamina = saveinfo.maxStamina;
        playerGameObject.GetComponent<PlayerStatus>().currentStamina = saveinfo.currentStamina;
        InventorySystem.instance.LoadInventory(saveinfo.playerCoinCount, saveinfo.equipmentItem, saveinfo.inventoryItem);
        QuestSystem.instance.LoadQuestData(saveinfo.questProgressID, saveinfo.isProgressQuest);
        MapPiecesController.instance.LoadMap(saveinfo.landMarkEnableArray);
        saveItemList.LoadMapItemList(saveinfo.fieldItemList);
        saveChestBoxList.LoadMapChestBoxList(saveinfo.fieldChestBoxList);

        return;
    }
}
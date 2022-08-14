using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjDialogTrigger : MonoBehaviour
{
    [Header("ObjectDialogSheet의 내용을 불러옵니다.")]
    [Header("↓Excel DB Scriptable 오브젝트를 연결해주세요.[차후 Manager(싱글턴)으로 등록할 수 있도록 구조 변경 필요!]")]
    [SerializeField] private ExcelDB dialogDB; // Import Excel File
    [Header("↓출력을 원하는 다이얼로그 ID 번호를 입력해주세요.")] 
    [SerializeField] private int dialogID; // 대화 출력을 원하는 퀘스트ID 선택

    [Header("↓[Debug] 대화 목록 확인용 리스트")]
    [SerializeField] private List<ObjDialogDBEntity> useDialogList; // 선택한 Object DialogID의 대사 목록 리스트

    private void Awake()
    {
        // 테스트 체크 [디버그]
        useDialogList = this.dialogDB.ObjectDialogSheet.Where(excelDB => excelDB.DialogID == dialogID).ToList();
    }

    public void EnterPlayer()
    {
        SoundManager.Instance.PlaySFX("DialogStart");
        StartCoroutine("StartDialog");
    }

    private IEnumerator StartDialog()
    {
        // setQuestID에 해당되는 Dialog 리스트를 매개변수로 보낸다. // Linq
        yield return new WaitUntil(() => DialogSystem.instance.UpdateDialog(this.dialogDB.ObjectDialogSheet.Where(excelDB => excelDB.DialogID == dialogID).ToList()));
    }
}

using UnityEngine;

public class PlayerSpawnMove : MonoBehaviour
{
    //키입력
    [SerializeField]
    private bool numberPad1KeyDown = false;
    [SerializeField]
    private bool numberPad2KeyDown = false;
    [SerializeField]
    private bool numberPad3KeyDown = false;
    [SerializeField]
    private bool numberPad4KeyDown = false;
    [SerializeField]
    private bool numberPad5KeyDown = false;

    private PlayerStatus playerStatus;

    [SerializeField] 
    private GameObject landMarkSpawnerPoint1;
    [SerializeField] 
    private GameObject landMarkSpawnerPoint2;
    [SerializeField] 
    private GameObject landMarkSpawnerPoint3;
    [SerializeField] 
    private GameObject landMarkSpawnerPoint4;
    [SerializeField] 
    private GameObject landMarkSpawnerPoint5;
    // Start is called before the first frame update
    void Start()
    {
        playerStatus = GameManager.instance.playerGameObject.GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        
        if (playerStatus.GetDebugMod())
        {
            Interaction();
        }
    }

    private void GetInput()
    {
        numberPad1KeyDown = Input.GetButtonDown("NumberPad1");
        
        numberPad2KeyDown = Input.GetButtonDown("NumberPad2");
        
        numberPad3KeyDown = Input.GetButtonDown("NumberPad3");
        
        numberPad4KeyDown = Input.GetButtonDown("NumberPad4");
        
        numberPad5KeyDown = Input.GetButtonDown("NumberPad5");
    }

    private void Interaction()
    {
        if (numberPad1KeyDown)
        {
            this.transform.position = landMarkSpawnerPoint1.transform.position;
        }
        else if (numberPad2KeyDown)
        {
            this.transform.position = landMarkSpawnerPoint2.transform.position;
        }
        else if (numberPad3KeyDown)
        {
            this.transform.position = landMarkSpawnerPoint3.transform.position;
        }
        else if (numberPad4KeyDown)
        {
            this.transform.position = landMarkSpawnerPoint4.transform.position;
        }
    }
}

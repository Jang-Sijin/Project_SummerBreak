using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera fpsCamera;
    public Rigidbody fpsRigidBody;
    public bool IsInvertYaxis = false; // 상하 반전 유무
    public float movementSpeed = 5;
    private float PreviousMovementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false; // 마우스 포인터가 보이지 않도록 설정한다.
        Cursor.lockState = CursorLockMode.Locked; // 마우스 포인터가 가운데로 갱신하도록 한다.

        PreviousMovementSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X"); // 마우스 X축(움직임) 값을 가져온다.
        var mouseY = Input.GetAxis("Mouse Y"); // 마우스 y축(움직임) 값을 가져온다.

            // print(message:$"mouse X: {mouseX}, mouse Y: {mouseY}"); // 마우스 변경 값을 출력한다.

            // 좌, 우 화면 회전은 캐릭터 모델(몸체)의 카메라를 바꿔야한다.
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, mouseX, 0); // x, y, z축의 각도 값을 넣어준다. // 화면을 회전하게 해줄 수 있게함.


            fpsCamera.transform.eulerAngles =
                fpsCamera.transform.eulerAngles +
                new Vector3(mouseY * (IsInvertYaxis ? 1 : -1), 0, 0); // 조건식을 3항식으로 대체했음.

            var keyboardX = Input.GetAxis("Horizontal"); // ws, 좌 우 버튼을 누르면 값을 받음
            var keyboardY = Input.GetAxis("Vertical"); // sw, 위 아래 버튼을 누르면 값을 받음

            // 앞으로 나가아는 속도는 KeyboardX의 값으로 한다. // Left Shift를 누를 경우 달린다.
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = PreviousMovementSpeed * 1.3f;
                // print(movementSpeed);
            }
            else
            {
                movementSpeed = PreviousMovementSpeed;
                // print(movementSpeed);
            }

            fpsRigidBody.velocity = (transform.forward * (keyboardY * movementSpeed)) +
                                    (transform.right * (keyboardX * movementSpeed)) + 
                                    new Vector3(0, fpsRigidBody.velocity.y, 0);
    }
}

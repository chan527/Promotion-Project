using UnityEngine;
using UnityEngine.InputSystem;

public class CameraArmController : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float mouseSensitivityX;
    [SerializeField]
    private float mouseSensitivityY;

    // 현재 CameraArm 의 회전 각도를 저장하는 변수
    private float xRotation;
    private float yRotation;

    [SerializeField]
    private float scrollSensitivity;
    // 카메라의 z 위치 (카메라 거리) 
    private float cameraPosZ;

    [SerializeField]
    private float distance;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Transform cam;

    private void Start()
    {
        cam = transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;

        mouseSensitivityX = 0.1f;
        mouseSensitivityY = 0.1f;

        // 시작 시 x, y축 회전을 오일러 각도로 받아 초기화
        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;

        scrollSensitivity = 0.3f;
        cameraPosZ = cam.localPosition.z;
    }

    private void Update()
    {
        // 마우스의 상하 움직임은 x축 회전을 변화시키고 좌우 움직임은 y축 회전을 변화시킴
        float rotationDeltaX = Mouse.current.delta.ReadValue().y * mouseSensitivityY;
        float rotationDeltaY = Mouse.current.delta.ReadValue().x * mouseSensitivityX;

        // 현재 회전 각도를 계속 업데이트
        // x축 회전은 마우스 이동과 반대방향으로 업데이트
        xRotation -= rotationDeltaX;
        yRotation += rotationDeltaY;

        // x축 회전은 최대, 최소를 정함
        xRotation = Mathf.Clamp(xRotation, -15f, 80f);

        float scrollDelta = Mouse.current.scroll.ReadValue().y * scrollSensitivity;

        cameraPosZ = Mathf.Clamp(cameraPosZ + scrollDelta, -5f, -1.5f);

    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        transform.position = target.position + offset;

        cam.localPosition = new Vector3(cam.localPosition.x, cam.localPosition.y, cameraPosZ);
    }


}

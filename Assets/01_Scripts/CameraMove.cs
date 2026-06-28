using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float mouseSensitivityX;
    [SerializeField]
    private float mouseSensitivityY;

    private float xRotation;
    private float yRotation;

    [SerializeField]
    private float distance;

    [SerializeField]
    private Vector3 offset;

    private Transform camera;

    private void Start()
    {
        camera = transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;

        mouseSensitivityX = 0.5f;
        mouseSensitivityY = 0.2f;

        xRotation = transform.rotation.eulerAngles.x;
        yRotation = transform.rotation.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        float rotationDeltaX = Mouse.current.delta.value.y * mouseSensitivityY;
        float rotationDeltaY = Mouse.current.delta.value.x * mouseSensitivityX;

        xRotation += rotationDeltaX;
        yRotation += rotationDeltaY;

        xRotation = Mathf.Clamp(xRotation, -20f, 80f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}

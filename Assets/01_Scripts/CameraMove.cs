using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private Vector3 offset;

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}

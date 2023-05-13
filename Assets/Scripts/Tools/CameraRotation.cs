using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Transform target;

    private void Update()
    {
        Vector3 desiredPosition = Quaternion.Euler(0f, rotationSpeed * Time.deltaTime, 0f) * (transform.position - target.position) + target.position;
        transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = desiredPosition;
        transform.LookAt(new Vector3(target.transform.position.x, 1, target.transform.position.z));
    }
}

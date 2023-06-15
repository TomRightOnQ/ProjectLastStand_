using UnityEngine;

public class LightRotation : MonoBehaviour
{
    // The total angle to rotate in degrees (360 degrees)
    private const float totalRotationAngle = 360f;

    // The total time to complete the rotation in seconds (3 minutes = 180 seconds)
    private const float totalRotationTime = 180f;

    // The rotation speed in degrees per second
    private float rotationSpeed;

    private void Start()
    {
        // Calculate the rotation speed
        rotationSpeed = totalRotationAngle / totalRotationTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate how much to rotate during this frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Rotate the light
        transform.Rotate(Vector3.right * rotationThisFrame);
    }
}

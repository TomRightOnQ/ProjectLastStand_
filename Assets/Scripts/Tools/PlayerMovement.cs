using UnityEngine;
using Photon.Pun;

// Player camera and movements
public class PlayerMovement : MonoBehaviour
{
    private Camera _camera;
    private Players player;
    private const float CLOSET_FIRERANGE = 10.0f;
    private float globalTime = 0.0f;

    void Start()
    {
        player = GetComponent<Players>();
        Plane plane = new Plane(Vector3.up, transform.position);
        _camera = Camera.main;
        globalTime = 0.0f;
    }
    void FixedUpdate()
    {
        // increment the global time in each FixedUpdate()
        globalTime += Time.fixedDeltaTime;
    }

    void LateUpdate()
    {
        if (globalTime < 1.0f || !Application.isFocused)
        {
            return;
        }

        if (PhotonNetwork.IsConnected && !player.photonView.IsMine)
        {
            return;
        }

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(transform.position.x, 0.65f, transform.position.z));
        float distanceToPlane;
        Vector3 mousePosition = transform.position;

        if (plane.Raycast(ray, out distanceToPlane))
        {
            mousePosition = ray.GetPoint(distanceToPlane);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 directionToMouse = mousePosition - transform.position;
            float distanceToMouse = directionToMouse.magnitude;
            directionToMouse.Normalize();
            Vector3 firePosition = distanceToMouse > CLOSET_FIRERANGE ? mousePosition : transform.position + directionToMouse * CLOSET_FIRERANGE;
            player.fire(firePosition);
        }

        float horizontalInput = 0.0f;
        float verticalInput = 0.0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1.0f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1.0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1.0f;
        }

        // Rotate towards the mouse position
        transform.LookAt(new Vector3(mousePosition.x, transform.position.y, mousePosition.z));

        // Move based on the direction of the WASD keys
        Vector3 direction = new Vector3(horizontalInput, 0.0f, verticalInput);
        direction = Vector3.ClampMagnitude(direction, 1.0f);
        Vector3 movement = direction * player.Speed * 18.0f * Time.deltaTime;

        // Check for collision with the Base collider
        Vector3 newPosition = transform.position + direction * player.Speed * Time.deltaTime;
        Vector3 displacement = newPosition - transform.position;
        Vector3 directionNormalized = displacement.normalized;
        float distance = displacement.magnitude;

        int defaultLayer = LayerMask.NameToLayer("Default");
        float playerRadius = player.GetComponent<Collider>().bounds.extents.magnitude;
        float totalDistance = distance + playerRadius;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, directionNormalized, out hitInfo, totalDistance, 1 << defaultLayer))
        {
            if (hitInfo.collider.CompareTag("Base") || hitInfo.collider.CompareTag("Wall"))
            {
                movement = Vector3.zero;
            }
        }

        // Move the objects
        transform.position += movement;
        _camera.transform.position = new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z - 51.9f);
    }
}

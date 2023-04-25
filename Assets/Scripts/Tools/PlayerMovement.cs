using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player camera and movements
public class PlayerMovement : MonoBehaviour
{
    private Camera _camera;
    private Players player;

    void Start()
    {
        player = GetComponent<Players>();
        Plane plane = new Plane(Vector3.up, transform.position);
        _camera = Camera.main;
    }

    void Update()
    {
        /*
        if (!IsOwner) {
            return;
        }
        */
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

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distanceToPlane;
        Vector3 mousePosition = transform.position;

        if (plane.Raycast(ray, out distanceToPlane))
        {
            mousePosition = ray.GetPoint(distanceToPlane);
        }

        // Rotate towards the mouse position
        transform.LookAt(new Vector3(mousePosition.x, transform.position.y, mousePosition.z));

        // Move based on the direction of the WASD keys
        Vector3 direction = new Vector3(horizontalInput, 0.0f, verticalInput);
        direction = Vector3.ClampMagnitude(direction, 1.0f);
        Vector3 movement = direction * player.Speed * Time.deltaTime;
        transform.position += movement;

        _camera.transform.position = new Vector3(transform.position.x, _camera.transform.position.y, transform.position.z - 51.9f);
    }
}

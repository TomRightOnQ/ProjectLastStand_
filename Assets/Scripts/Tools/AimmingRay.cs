using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimmingRay : MonoBehaviour
{
    [SerializeField] private LineRenderer aimmingRay;

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            // If the ray hits something, update the positions of the LineRenderer
            aimmingRay.SetPosition(0, transform.position);
            aimmingRay.SetPosition(1, hit.point);
        }
        else
        {
            aimmingRay.SetPosition(0, transform.position);
            aimmingRay.SetPosition(1, transform.position + transform.forward * 100f);
        }
    }
}

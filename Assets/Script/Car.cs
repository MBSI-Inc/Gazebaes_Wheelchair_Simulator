using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed;
    public bool stopMoving = false;

    private float raycastDistance = 5.5f;
    private Vector3 raycastBoxSize = new Vector3(2f, 0.5f, 2.5f);


    void Update()
    {
        stopMoving = false;
        RaycastHit hit;

        // if (Physics.Raycast(transform.position + transform.forward * 4f, transform.forward, out hit, 2f))
        if (Physics.BoxCast(transform.position, raycastBoxSize, transform.forward, out hit, transform.rotation, raycastDistance))
        {
            if (hit.collider.transform.parent && hit.collider.transform.parent.GetComponent<Car>())
            {
                stopMoving = true;
            }
            else if (hit.collider.GetComponent<EyeGazeController>())
            {
                stopMoving = true;
            }
        }

        if (!stopMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the raycast in yellow
        Gizmos.color = Color.yellow;
        // Gizmos.DrawRay(transform.position + transform.forward * 4f, transform.forward * 2f);

        Gizmos.DrawWireCube(transform.position + transform.forward * raycastDistance, raycastBoxSize * 2);
    }
}

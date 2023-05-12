using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed;
    public bool carInFront = false;

    private float raycastDistance = 6f;
    private Vector3 raycastBoxSize = new Vector3(2f, 0.5f, 2f);


    void Update()
    {
        carInFront = false;
        RaycastHit hit;

        // if (Physics.Raycast(transform.position + transform.forward * 4f, transform.forward, out hit, 2f))
        if (Physics.BoxCast(transform.position, raycastBoxSize, transform.forward, out hit, transform.rotation, raycastDistance))
        {
            if (hit.collider.transform.parent && hit.collider.transform.parent.GetComponent<Car>())
            {
                carInFront = true;
            }
        }

        if (!carInFront)
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

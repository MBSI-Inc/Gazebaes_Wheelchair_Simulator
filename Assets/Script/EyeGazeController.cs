using UnityEngine;

public class EyeGazeController : MonoBehaviour
{
    public ConnectionsHandler connectionsHandler;
    public float moveSpeed = 0;
    private float currentSpeed = 0;
    public bool stopMoving = false;
    [SerializeField] private float direction = 0.0f;
    public bool useKeyboard;
    public Transform spawn;

    private void Update()
    {
        if (!stopMoving)
        {
            if (!useKeyboard)
            {
                SetDirection(connectionsHandler.getLatestDirection());
            }
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
        transform.Rotate(0f, direction * Time.deltaTime, 0f, Space.Self);

        // In final version, we would use brain signal to start. For now let
        // just use space.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stopMoving = !stopMoving;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawn.position;
            transform.rotation = Quaternion.Euler(0, 90f, 0);
        }
    }

    //Sets direction of wheelchair, 0 would keep going forward, negative values turn left, pos turns right
    public void SetDirection(float newDir)
    {
        direction = newDir;
        if (newDir == 0)
        {
            currentSpeed = moveSpeed;
        }
        else
        {
            currentSpeed = 0f;
        }
    }
}

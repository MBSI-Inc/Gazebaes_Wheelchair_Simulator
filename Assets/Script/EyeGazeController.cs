using UnityEngine;

public class EyeGazeController : MonoBehaviour
{
    public ConnectionsHandler connectionsHandler;
    public float targetSpeed = 10F;
    private float currentSpeed = 0;
    public bool isMoving = false;
    [SerializeField] private float direction = 0.0f;
    public bool useKeyboard;
    public Transform spawn;
    public int obstacleBumpCount = 0;

    private void Update()
    {
        if (!useKeyboard)
        {
            isMoving = connectionsHandler.getLatestMovement();
            if (isMoving)
            {
                SetDirection(connectionsHandler.getLatestDirection());
                SetTargetSpeed(connectionsHandler.getLatestTargetSpeed());
                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
        transform.Rotate(0f, direction * Time.deltaTime, 0f, Space.Self);

        // In final version, we would use brain signal to start. For now let
        // just use space.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Space bar pressed");
            isMoving = !isMoving;
            if (isMoving)
            {
                currentSpeed = targetSpeed;
            }
            else
            {
                currentSpeed = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawn.position;
            transform.rotation = Quaternion.Euler(0, 90f, 0);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {

            connectionsHandler.isMoving= !connectionsHandler.isMoving;
        }

    }

    //Sets direction of wheelchair, 0 would keep going forward, negative values turn left, pos turns right
    public void SetDirection(float newDir)
    {
        direction = newDir;
        if (newDir == 0)
        {
            currentSpeed = targetSpeed;
        }
        else
        {
            currentSpeed = 0.0f;
        }
    }
    public void SetTargetSpeed(float _targetSpeed)
    {
        targetSpeed = _targetSpeed;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.layer == 6)
        {
            obstacleBumpCount += 1;

        }
    }
}

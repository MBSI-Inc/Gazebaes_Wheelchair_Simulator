using UnityEngine;

public class EyeGazeController : MonoBehaviour
{
    public ConnectionsHandler connectionsHandler;
    public float maxSpeed = 24.0f;
    // If move speed is lower than this, drift wont decel
    public float minSpeedBeforeDriftDecel = 12f;
    public float accel = 1f;
    public float driftDecel = 1f;
    public float moveSpeed = 0;
    public bool isStopped = true;
    [SerializeField] private float direction = 0.0f;
    public bool useKeyboard;

    Vector3 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
    }


    void Update()
    {
        // Now need to check if we are getting a direction input or a stop input
        if (!useKeyboard)
        {
            if (connectionsHandler.getDataTypeTracker() == 0.0f)
            {
                SetDirection(connectionsHandler.getLatestDirection());
            }

            else if (connectionsHandler.getDataTypeTracker() == 1.0f)
            {
                SetStopOrGo(connectionsHandler.getLatestMovingSignal());
            }
            
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, direction * Time.deltaTime, 0f, Space.Self);
        if((transform.position - startingPosition).magnitude > 300)
        {
            transform.position = startingPosition;
        }
    }

    //Sets direction of wheelchair, 0 would keep going forward, negative values turn left, pos turns right
    public void SetDirection(float newDir)
    {
        direction = newDir;
        if (newDir == 0 && !isStopped)
        {
            moveSpeed += accel * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
        }
        else if (!isStopped)
        {
            if (moveSpeed > minSpeedBeforeDriftDecel)
                moveSpeed -= driftDecel * (moveSpeed / 10) * Time.deltaTime;
        }
    }

    // Sets the stop and go of the wheelchair, based on a 0/1 value
    public void SetStopOrGo(float isMoving)
    {
        if (isMoving == 0.0f)
        {
            moveSpeed = 0;
            isStopped = true;
            return;
        }

        isStopped = false;
    }

}

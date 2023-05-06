using UnityEngine;

public class EyeGazeRacingController : MonoBehaviour
{
    public ConnectionsHandler connectionsHandler;
    public float maxSpeed = 24.0f;
    // If move speed is lower than this, drift wont decel
    public float minSpeedBeforeDriftDecel = 12f;
    public float accel = 1f;
    public float driftDecel = 1f;
    public float moveSpeed = 0;
    [SerializeField] private float direction = 0.0f;
    public bool useKeyboard;

    Vector3 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
    }


    void Update()
    {
        if (!useKeyboard)
        {
            SetDirection(connectionsHandler.getLatestDirection());
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
        if (newDir == 0)
        {
            moveSpeed += accel * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);
        }
        else
        {
            if (moveSpeed > minSpeedBeforeDriftDecel)
                moveSpeed -= driftDecel * (moveSpeed / 10) * Time.deltaTime;
        }
    }

}

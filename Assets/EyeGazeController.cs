using UnityEngine;

public class EyeGazeController : MonoBehaviour
{
    public ConnectionsHandler connectionsHandler;
    public float normalSpeed = 10.0f;
    public float driftSpeed = 2.0f;
    public float moveSpeed;
    [SerializeField] private float direction = 0.0f;
    public bool useKeyboard;


    void Start()
    {
        moveSpeed = normalSpeed;
    }


    void Update()
    {
        if (!useKeyboard)
        {
            SetDirection(connectionsHandler.getLatestDirection());
        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, direction * Time.deltaTime, 0f, Space.Self);
    }

    //Sets direction of wheelchair, 0 would keep going forward, negative values turn left, pos turns right
    public void SetDirection(float newDir)
    {
        direction = newDir;
        if (newDir == 0)
        {
            moveSpeed = normalSpeed;
        }
        else
        {
            moveSpeed = driftSpeed;
        }
    }

}

using UnityEngine;

public class SteeringController : MonoBehaviour
{
    [SerializeField]
    private float direction = 0.0f;//value between -45~45
    [SerializeField]
    public float moveSpeed = 2.0f;


    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, direction * Time.deltaTime, 0f, Space.Self);
    }

    //Sets direction of wheelchair, 0 would keep going forward, negative values turn left, pos turns right
    public void SetDirection(float newDir)
    {
        direction = newDir;
    }

}

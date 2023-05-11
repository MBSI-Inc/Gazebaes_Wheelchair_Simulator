using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed;


    void Start()
    {
    }

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}

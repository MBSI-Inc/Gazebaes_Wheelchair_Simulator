using UnityEngine;

public class CarTeleportZone : MonoBehaviour
{
    public Vector3 change;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<Car>())
        {
            Transform car = other.transform.parent.transform;
            Vector3 position = new Vector3(car.position.x, car.position.y, car.position.z) + change;
            car.position = position;
        }
    }
}

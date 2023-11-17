using UnityEngine;

public class TramZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        EyeGazeController player = other.gameObject.GetComponent<EyeGazeController>();
        if (player)
        {
            player.isMoving = false;
            GameManager.instance.FinishSimulation();
        }
    }
}

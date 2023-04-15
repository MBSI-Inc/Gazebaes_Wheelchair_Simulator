using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ConnectionsHandler connectionsHandler;
    [SerializeField]
    GameObject player;
    SteeringController sc;

    // Start is called before the first frame update
    void Start()
    {
        connectionsHandler = this.GetComponent<ConnectionsHandler>();
        sc = player.GetComponent<SteeringController>();
    }

    // Update is called once per frame
    void Update()
    {
        float newDir = connectionsHandler.getLatestDirection();
        sc.SetDirection(newDir);
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}

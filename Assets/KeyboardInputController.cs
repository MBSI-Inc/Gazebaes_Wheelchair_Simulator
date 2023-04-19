using UnityEngine;

/// <summary>
/// For mocking.
/// </summary>
public class KeyboardInputController : MonoBehaviour
{
    public GameObject player;
    private EyeGazeController eyeGazeController;
    public float turnRate = 25f;

    void Start()
    {
        eyeGazeController = player.GetComponent<EyeGazeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            eyeGazeController.SetDirection(-turnRate);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            eyeGazeController.SetDirection(turnRate);
        }
        else
        {
            eyeGazeController.SetDirection(0);
        }
    }
}

using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject finishText;


    // Singleton pattern
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void FinishSimulation()
    {
        finishText.SetActive(true);
        Debug.Log("Obstacles bumped " + GameObject.Find("Player").GetComponent<EyeGazeController>().obstacleBumpCount);
    }

}

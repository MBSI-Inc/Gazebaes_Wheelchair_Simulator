using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public EyeGazeController controller;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        speedText.text = $"{(controller.moveSpeed * 3):0} km/h";
    }
}

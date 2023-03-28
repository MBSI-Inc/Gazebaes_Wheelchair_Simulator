using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    SteeringController sc;
    [SerializeField]
    float turnRate = 45f;
    float defaultMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        sc = player.GetComponent<SteeringController>();
        defaultMoveSpeed = sc.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float newDir = 0f;
        if(Input.GetKey("left")){
            //newDir = Mathf.Clamp(newDir-turnRate, -45, 45);
            newDir = -turnRate;
        }
        if(Input.GetKey("right")){
            newDir = turnRate;
        }
        if(Input.GetKey("up")){
            sc.moveSpeed = defaultMoveSpeed*2;
        }else{
            sc.moveSpeed = defaultMoveSpeed;
        }
        sc.SetDirection(newDir);
    }
}

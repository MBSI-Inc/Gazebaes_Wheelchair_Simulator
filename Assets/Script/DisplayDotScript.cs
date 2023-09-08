using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDotScript : MonoBehaviour
{
    [SerializeField] private ConnectionsHandler handler;
    private RectTransform picture;
    private float width = 1440f;
    private float height = 720f;
    
    // Start is called before the first frame update
    void Start()
    {
        picture = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // float newX = (handler.lastReceivedDirection * width) + height;
        float newX = handler.lastReceivedDirection;
        picture.anchoredPosition = new Vector2(newX, 0);
        // Debug.Log(picture.anchoredPosition);
    }
}

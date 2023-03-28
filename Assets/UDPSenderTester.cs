using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;


public class UDPSenderTester : MonoBehaviour
{

    [SerializeField]
    float direction = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        SendInfo();
    }

    void SendInfo()
    {
        byte[] data = Encoding.ASCII.GetBytes(direction.ToString());
        string ipAddress = "127.0.0.1";
        int sendPort = 8051;
        try
        {
            using (var client = new UdpClient())
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), sendPort);
                client.Connect(ep);
                client.Send(data, data.Length);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}

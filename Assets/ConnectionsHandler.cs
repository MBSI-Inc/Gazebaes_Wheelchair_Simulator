using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;


public class ConnectionsHandler : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    IPEndPoint endpoint;
    public int port = 8051;

    public float lastReceivedDirection = 0.0f;

    private void Awake()
    {
        // status
        print("Sending to 127.0.0.1 : " + port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");
        client = new UdpClient(port);
        endpoint = new IPEndPoint(IPAddress.Any, 0);
    }

    private void Start()
    {
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // receive thread
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                byte[] data = client.Receive(ref endpoint);
                string text = Encoding.UTF8.GetString(data);
                Debug.Log(">> UDP listener received data: " + text);

                float newDir;
                if (float.TryParse(text, out newDir))
                {
                    lastReceivedDirection = newDir;
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }


    public float getLatestDirection()
    {
        return lastReceivedDirection;
    }

    private void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();
        client.Close();
    }
}

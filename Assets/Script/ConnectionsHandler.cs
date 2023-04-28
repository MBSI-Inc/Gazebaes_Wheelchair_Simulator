using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;


public class ConnectionsHandler : MonoBehaviour
{
    Thread receiveThread;
    Thread frameSenderThread;
    // udpclient object
    UdpClient client;
    IPEndPoint endpoint;

    public int port = 8051; // define > init
    public readonly float IS_DIRECTION = 0.0f;
    public readonly float IS_MOVING = 1.0f;
    public float dataTypeTracker = 0.0f;

    [SerializeField]
    ScreenshotFetcher screenshotFetcher;
    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!
    public float lastReceivedDirection = 0.0f;
    public float lastReceivedMovingSignal = 0.0f;

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
        frameSenderThread = new Thread(()=>TCPServer.Connect(screenshotFetcher));
        frameSenderThread.Start();

    }

    // receive thread
    private void ReceiveData()
    {
        while (true)
        {
            // Simple way of getting both stop signal and direction
            // is to have a value that switches between 1 and 0,
            // and each value indicates a specific data taken
            try
            {
                byte[] data = client.Receive(ref endpoint);
                string text = Encoding.UTF8.GetString(data);
                Debug.Log(">> UDP listener received data: " + text);

                float newVal;
                if (float.TryParse(text, out newVal))
                {
                    if (dataTypeTracker == IS_DIRECTION)
                    {
                        lastReceivedDirection = newVal;
                    }

                    else
                    {
                        lastReceivedMovingSignal = newVal;
                    }
                }

                dataTypeTracker = Math.Abs(dataTypeTracker-1.0f);
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

    public float getLatestMovingSignal()
    {
        return lastReceivedMovingSignal;
    }

    public float getDataTypeTracker()
    {
        return dataTypeTracker;
    }

    private void OnDisable()
    {
        if (receiveThread != null)
            receiveThread.Abort();
        client.Close();
        if (frameSenderThread != null)
            frameSenderThread.Abort();
    }
}

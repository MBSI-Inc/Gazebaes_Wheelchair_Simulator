using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;

public class ConnectionsHandler : MonoBehaviour
{
    private Thread gazeReceiveThread;
    private Thread frameSenderThread;
    // udpclient object
    private UdpClient gazeClient;
    private UdpClient brainClient;

    private IPEndPoint endpoint;
    public bool isMoving = false;
    public int brainCommandPort = 8053;
    public int framePort = 8052;
    public int gazeTrackCommandPort = 8051; // define > init

    [SerializeField]
    private ScreenshotFetcher screenshotFetcher;

    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!
    private float targetTurnRate = 0.0f;
    private float targetSpeed = 10.0f;
    public bool[] userInputTrigger;

    private bool gazeReceiveThreadIsRunning = false;
    private bool frameSenderThreadIsRunning = false;

    private void Awake()
    {
        // status
        print("Listening for movement command on to 127.0.0.1 : " + gazeTrackCommandPort);
        gazeClient = new UdpClient(gazeTrackCommandPort);
        brainClient = new UdpClient(brainCommandPort);
        endpoint = new IPEndPoint(IPAddress.Any, 0);
    }

    private void Start()
    {
        gazeReceiveThreadIsRunning = true;
        gazeReceiveThread = new Thread(ReceiveData);
        gazeReceiveThread.IsBackground = true;
        gazeReceiveThread.Start();

        frameSenderThreadIsRunning = true;
        frameSenderThread = new Thread(ConnectScreenshotFetcher);
        frameSenderThread.IsBackground = true;
        frameSenderThread.Start();
        userInputTrigger = new bool[5];
    }

    // receive thread
    private void ReceiveData()
    {
        var timer = new System.Diagnostics.Stopwatch();
        timer.Start();
        while (true)
        {

            try
            {
                byte[] data = gazeClient.Receive(ref endpoint);
                string text = Encoding.UTF8.GetString(data);
                Debug.Log(">> UDP listener received data: " + text);
                String command = text.Split(" ")[0];
                String value = text.Split(" ")[1];
                if (command.Equals("c"))
                {
                    if (value.Equals("move"))
                    {
                        if ((timer.ElapsedMilliseconds > 125))
                        {
                            isMoving = !isMoving;
                            timer.Restart();
                        }
                    }

                    if (value.Equals("input_0"))
                    {
                        userInputTrigger[0] = true;
                    }
                }
                else if (command.Equals("d"))
                {
                    //Value should be in the format of [speed,turnrate]
                    String[] values = value.Substring(1, value.Length - 2).Split(",");
                    float _targetSpeed, _targetTurnRate;
                    if (float.TryParse(values[0], out _targetSpeed) && float.TryParse(values[1], out _targetTurnRate))
                    {
                        targetTurnRate = _targetTurnRate;
                        targetSpeed = _targetSpeed;
                    }
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    // frameSender thread
    private void ConnectScreenshotFetcher()
    {
        while (true)
        {
            print("restarting server");
            TcpListener server = null;
            try
            {

                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, framePort);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    print("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    print("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        print(String.Format("Received: {0}", data));

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] frameBytes = screenshotFetcher.GetCameraViewByteArray();
                        Int32 frameLength = frameBytes.Length;
                        byte[] lengthHeader = BitConverter.GetBytes(frameLength);

                        byte[] msg = Combine(lengthHeader, frameBytes);
                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);

                        //print(String.Format("Sent byte array of length: {0}", msg.Length));

                        //Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                    stream.Close();
                    print("Client and stream closed");
                }
            }
            catch (Exception e)
            {
                print(String.Format("!SocketException: {0}", e));
            }
            finally
            {
                print("Finally block, returning to while loop");
                // Stop listening for new clients.
                //server.Stop();
            }
        }
    }

    public static byte[] Combine(byte[] first, byte[] second)
    {
        byte[] bytes = new byte[first.Length + second.Length];
        Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
        Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
        return bytes;
    }

    public float getLatestDirection()
    {
        return targetTurnRate;
    }

    public float getLatestTargetSpeed()
    {
        return targetSpeed;
    }

    public bool getLatestMovement()
    {
        return isMoving;
    }
    public bool getInputTrigger(int i = 0)
    {
        if (userInputTrigger[i])
        {
            userInputTrigger[i] = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDisable()
    {
        if (gazeReceiveThread != null)
        {
            gazeReceiveThreadIsRunning = false;
            //gazeReceiveThread.Join();
            gazeReceiveThread.Abort();
        }
        if (frameSenderThread != null)
        {
            frameSenderThreadIsRunning = false;
            //frameSenderThread.Join();
            frameSenderThread.Abort();
        }

        gazeClient.Close();
    }
}

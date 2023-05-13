using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System;

public class ConnectionsHandler : MonoBehaviour
{
    private Thread receiveThread;
    private Thread frameSenderThread;
    // udpclient object
    private UdpClient client;
    private IPEndPoint endpoint;
    private ManualResetEvent stopEvent = new ManualResetEvent(false);

    public int port = 8051; // define > init

    [SerializeField]
    private ScreenshotFetcher screenshotFetcher;

    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!
    public float lastReceivedDirection = 0.0f;

    private bool receiveThreadIsRunning = false;
    private bool frameSenderThreadIsRunning = false;

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
        receiveThreadIsRunning = true;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        frameSenderThreadIsRunning = true;
        frameSenderThread = new Thread(new ThreadStart(ConnectScreenshotFetcher));
        frameSenderThread.IsBackground = true;
        frameSenderThread.Start();
    }

    // receive thread
    private void ReceiveData()
    {
        while (receiveThreadIsRunning)
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

    // frameSender thread
    private void ConnectScreenshotFetcher()
    {
        while (frameSenderThreadIsRunning)
        {
            print("restarting server");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 8052;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

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
                        print(String.Format("Sent byte array of length: {0}", msg.Length));
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
        return lastReceivedDirection;
    }

    private void OnDisable()
    {
        if (receiveThread != null)
        {
            receiveThreadIsRunning = false;
            receiveThread.Join();
        }
        if (frameSenderThread != null)
        {
            frameSenderThreadIsRunning = false;
            frameSenderThread.Join();
        }

        stopEvent.Set();
        client.Close();
    }
}

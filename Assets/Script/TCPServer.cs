using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using UnityEngine;


class TCPServer: MonoBehaviour
{
        
    public static void Connect(ScreenshotFetcher screenshotFetcher)
    {
        while (true)
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

}


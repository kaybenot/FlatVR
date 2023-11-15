using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class OpentrackData
{
    public double x;
    public double y;
    public double z;
    public double yaw;
    public double pitch;
    public double roll;
}

public class UDPReceiver : MonoBehaviour
{
    public static Action<OpentrackData> OnDataReceived;

    private UdpClient client;
    private Thread thread;

    private void Awake()
    {
        thread = new Thread(new ThreadStart(ReceiveData));
        thread.IsBackground = true;
        thread.Start();
    }
    
    private void ReceiveData()
    {
        Debug.Log("Thread started!");
        client = new UdpClient(4242);

        while (true)
        {
            try
            {
                var anyIP = new IPEndPoint(IPAddress.Any, 0);
                var data = client.Receive(ref anyIP);

                OpentrackData oData = new()
                {
                    x = BitConverter.ToDouble(data, 0),
                    y = BitConverter.ToDouble(data, 8),
                    z = BitConverter.ToDouble(data, 16),
                    yaw = BitConverter.ToDouble(data, 24),
                    pitch = BitConverter.ToDouble(data, 32),
                    roll = BitConverter.ToDouble(data, 40)
                };

                Debug.Log($"X: {oData.x}, Y: {oData.y}, Z: {oData.z}");

                OnDataReceived?.Invoke(oData);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                throw;
            }
        }
    }

    private void OnApplicationQuit()
    {
        thread.Abort();
        client.Close();
    }
}

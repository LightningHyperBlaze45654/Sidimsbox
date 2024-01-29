using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UnityClient : MonoBehaviour {
    public string serverIP = "127.0.0.1";
    public int serverPort = 25001;
    public Button Button;
    private bool isRecording = false;

    private TcpClient client;
    private NetworkStream stream;

    private void Start() {
        client = new TcpClient();
        client.Connect(serverIP, serverPort);
        stream = client.GetStream();

        Button.onClick.AddListener(StartAndStopRecord);


        // Start a background thread for receiving messages
        System.Threading.Thread receiveThread = new System.Threading.Thread(ReceiveData);
        receiveThread.Start();
    }

    private void OnDestroy() {
        client.Close();
    }

    private void StartAndStopRecord() {
        if (isRecording == false) {
            SendData("StartRecord");
            isRecording = true;
        } else {
            SendData("StopReocrd");
            isRecording = false;
        }
    }



    private void SendData(string data) {
        byte[] buffer = Encoding.UTF8.GetBytes(data);
        stream.Write(buffer, 0, buffer.Length);
        Debug.Log($"Sent data: {data}");
    }

    private void ReceiveData() {
        while (true) {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            if (receivedData != null && receivedData != "") {
                Debug.Log($"Received data: {receivedData}");
            }
        }
    }
}

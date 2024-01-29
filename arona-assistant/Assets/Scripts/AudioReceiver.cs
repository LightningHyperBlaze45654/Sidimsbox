using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class AudioReceiver : MonoBehaviour {
    public AudioSource audioSource;

    private TcpListener server;
    private TcpClient client;
    private NetworkStream stream;
    private BinaryReader reader;

    private const int SampleRate = 22050;
    private const int Channels = 1;
    private const int BitsPerSample = 32;

    private byte[] receivedAudioData;

    private bool play = false;

    private void Start() {
        Debug.Log("Time.timeScale: " + Time.timeScale);
        StartServer();
    }

    private void Update() {
        if (play) {
            audioSource.Play();
            play = false;
        }
    }

    private void StartServer() {
        server = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 12345);
        server.Start();

        // Accept client connection
        client = server.AcceptTcpClient();
        stream = client.GetStream();
        reader = new BinaryReader(stream);

        // Start receiving audio data
        StartCoroutine(ReceiveAudioData());
    }

    private System.Collections.IEnumerator ReceiveAudioData() {
        using (MemoryStream memoryStream = new MemoryStream()) {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0) {
                memoryStream.Write(buffer, 0, bytesRead);
            }

            receivedAudioData = memoryStream.ToArray();
        }

        // Create and play the AudioClip
        CreateAndPlayAudioClip();

        // Clean up
        reader.Close();
        stream.Close();
        client.Close();
        server.Stop();

        yield return null;
    }

    const int SAMPLE_SIZE = sizeof(Int32);
    const int WAV_HEADER_SIZE = 44;

    private void CreateAndPlayAudioClip() {
        if (receivedAudioData != null && receivedAudioData.Length > 0) {
            var sampleCount = (receivedAudioData.Length - WAV_HEADER_SIZE) / SAMPLE_SIZE;
            AudioClip clip = AudioClip.Create("ReceivedAudio", sampleCount, Channels, SampleRate, false);

            float[] samples = new float[sampleCount];

            for (int i = 0; i < samples.Length; i++) {
                samples[i] = (float)BitConverter.ToSingle(receivedAudioData, i * SAMPLE_SIZE) / Int16.MaxValue;
            }

            clip.SetData(samples, 0);
            audioSource.clip = clip;

            play = true;
        }
    }
}

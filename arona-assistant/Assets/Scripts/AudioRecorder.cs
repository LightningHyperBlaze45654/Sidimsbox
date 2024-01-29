using UnityEngine;
using System.IO;
using Samples.Whisper;
using UnityEngine.UI;

public class AudioRecorder : MonoBehaviour {
    AudioClip recordedClip;
    AudioSource audioSource;
    public static bool recording = false;
    public Button recordButton;


    void Start() {
        Button btn = recordButton.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void Update() { 
        if (recording) {
            // Record audio
            int sampleRate = 44100; // You can adjust this based on your needs
            int recordingTime = 40; // Set the recording time in seconds

            recordedClip = Microphone.Start(null, false, recordingTime, sampleRate);
        } else if (Microphone.IsRecording(null) || !recording) {
            // Stop recording and save the audio to a .wav file
            Microphone.End(null);

            string filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
            SaveWav(filePath, recordedClip);

            // Send the file via socket (implement your socket logic here)
            SendAudioFile(filePath);
        }
    }
    void SaveWav(string filePath, AudioClip clip) {
        int hz = clip.frequency;
        int channels = clip.channels;
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fileStream)) {
            // Write the WAV header
            writer.Write("RIFF".ToCharArray());
            writer.Write(36 + samples.Length * 2);
            writer.Write("WAVE".ToCharArray());
            writer.Write("fmt ".ToCharArray());
            writer.Write(16);
            writer.Write((ushort)1); // PCM format
            writer.Write((ushort)channels);
            writer.Write(hz);
            writer.Write(hz * channels * 2); // Average bytes per second
            writer.Write((ushort)(channels * 2)); // Block align
            writer.Write((ushort)16); // Bits per sample
            writer.Write("data".ToCharArray());
            writer.Write(samples.Length * 2);

            // Write audio data
            foreach (var sample in samples) {
                writer.Write((short)(sample * 32767));
            }
        }
    }
    private void OnClick() {
        if (recording) {
            recording = false;

        } else {
            recording = true;
        }
    }
    void SendAudioFile(string filePath) {
        // Implement your socket logic to send the file to the server
        // You can use System.Net.Sockets.Socket or any other networking library
    }

}

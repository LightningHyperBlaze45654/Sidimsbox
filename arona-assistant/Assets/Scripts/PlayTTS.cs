using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTTS : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip clip;
    // Start is called before the first frame update
    void Start() {
        //PlayWavFile("C:/Users/user/Desktop/AronaAssistant/arona_backend/output.wav"); // Replace with your initial WAV file path
    }

    // Call this method whenever you want to change the WAV file and play it
    public void PlayWavFile(string wavFilePath) {
        StartCoroutine(LoadAndPlayWav(wavFilePath));
        StopCoroutine(LoadAndPlayWav(wavFilePath));
    }

    private IEnumerator LoadAndPlayWav(string wavFilePath) {
        using (WWW www = new WWW("file://" + wavFilePath)) {
            yield return www;

            if (string.IsNullOrEmpty(www.error)) {
                audioSource.clip = www.GetAudioClip();
                audioSource.Play();
            } else {
                Debug.LogError("Error loading WAV file: " + www.error);
            }
        }
    }
}

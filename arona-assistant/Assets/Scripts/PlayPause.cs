using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayPause : MonoBehaviour
{
    public AudioSource BGMSource;
    public GameObject soundControlButton;
    public Sprite audioPauseSprite;
    public Sprite audioPlaySprite;
    private bool isPaused = true;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = soundControlButton.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick() {
        if (isPaused) { 
            BGMSource.Play();
            soundControlButton.GetComponent<Image>().sprite = audioPlaySprite;
            isPaused = false;
        } else {
            BGMSource.Pause();
            soundControlButton.GetComponent<Image>().sprite = audioPauseSprite;
            isPaused = true;
        }
    }


}

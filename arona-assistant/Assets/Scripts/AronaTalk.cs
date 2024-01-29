using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using static AnimationManager;

public class AronaTalk : MonoBehaviour {
    
    public GameObject DialougeBox;
    private TextMeshProUGUI Dialouge;
    public AnimationManager animationManager;
    public AudioSource _audioSource;
    public AudioClip[] DialougeGreet;
    public AudioClip[] DialougeChat;
    public AudioClip[] DialougeService;
    public Collider2D area;
    private AnimationReferenceAsset[] AnimClip;

    private CanvasGroup DialougeGroup;

    [Range(0f, 1f)]
    public float animationMix;
    [Range(9, 42)]
    public int selectedAnimationNum;
    bool IsDialouge = false;
    int DialougeState = 0;
    
    
    IEnumerator DialougeBoxShowHide(float ClipTime, float FadeInOutSpeed, int _DialougeState) {
        IsDialouge = true;
        DialougeGroup = DialougeBox.GetComponent<CanvasGroup>();
            for (float i = 0; i <= 1; i += Time.deltaTime * FadeInOutSpeed) {
            // set color with i as alpha
                DialougeGroup.alpha = i;
                yield return null;
            }
            DialougeGroup.alpha = 1f;
        
            yield return new WaitForSeconds(ClipTime + 0.2f);
            DialougeGroup.alpha = 0f;
            for (float i = 1; i >= 0; i -= Time.deltaTime * FadeInOutSpeed) {
                // set color with i as alpha
                DialougeGroup.alpha = i;
                yield return null;
            }
            DialougeGroup.alpha = 0f;
        DialougeState = _DialougeState;
        IsDialouge = false;
    }
    public enum ServiceMasterBranch {
        OpenApp, Call, Message, KakaoTalk, Search
    } //TODO: think which service do i want to implement


    
    // Start is called before the first frame update
    void Start() {
        Dialouge = DialougeBox.GetComponentInChildren<TextMeshProUGUI>() ;
        AnimClip = animationManager.AnimClip;
    }

    // Update is called once per frame
    void FixedUpdate() {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height)));
        if (Input.GetMouseButton(0)) {
            if ((area.OverlapPoint(mousePos))) {
                if (!IsDialouge) {
                    if (DialougeState == 0) {
                        int DialougeNum = Random.Range(0, 2);
                        switch (DialougeNum) {
                            case 0:
                                _audioSource.clip = DialougeGreet[DialougeNum];
                                Dialouge.text = "선생님! \n 기다리고 있었습니다!";
                                break;
                            case 1:
                                _audioSource.clip = DialougeGreet[DialougeNum];
                                Dialouge.text = "자아, 업무를 시작할 시간이에요!";
                                break;
                            case 2:
                                _audioSource.clip = DialougeGreet[DialougeNum];
                                Dialouge.text = "어떤 업무를 하실 건가요 \n 선생님?";
                                break;
                        }
                        _audioSource.Play();
                        StartCoroutine(DialougeBoxShowHide(_audioSource.clip.length, 2, 1));
                    } else if (DialougeState == 1) {
                        int DialougeNum = Random.Range(0, 6);
                        switch (DialougeNum) {
                            case 0:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = "여기서는 선생님의 다양한 업무를\n진행할 수 있어요!";
                                break;
                            case 1:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = " 선생님!\n원하시는 업무를 선택해 주세요.\n제가 옆에서 돕겠습니다!";
                                break;
                            case 2:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = "해결해 주셔야 할 업무들이에요.\r\n어른이란 힘들겠네요.";
                                break;
                            case 3:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = "해결해야 할 일들이 잔뜩!\n그치만 힘내는 거예요!";
                                break;
                            case 4:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = "가끔씩은 몸 생각도 해야죠.\n선생님의 건강이 걱정된다구요!";
                                break;
                            case 5:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = "우와⋯.\r\n일이 엄청나게 많이 있네요.";
                                break;
                            case 6:
                                _audioSource.clip = DialougeChat[DialougeNum];
                                Dialouge.text = "자, 힘내는 거예요!\r\n선생님!";
                                break;
                        }
                        _audioSource.Play();
                        StartCoroutine(DialougeBoxShowHide(_audioSource.clip.length, 2, 1));
                    }
                }
            }
        }
    }

}
    

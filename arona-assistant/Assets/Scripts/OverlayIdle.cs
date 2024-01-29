using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayIdle : MonoBehaviour {
    public Button Button;
    public AnimationManager animationManager;
    public AnimationReferenceAsset[] AnimClip = new AnimationReferenceAsset[42];
    [Range(0f, 1f)]
    public float animationMix;
    [Range(9, 42)]
    public int selectedAnimationNum;

    /*
     *!! IMPORTANT !! When using animationManager to overlay images to idle animation, use track [0] since its the same as idle animation's track.
     * Example: _AsyncAnimation(0,AnimClip[15],false,1f,animationMix);
    9 : default
    10 : default
    11: default-mouthopeninterested
    12: headpat
    13: surprised
    14: focused
    15: worried
    16: contempt
    17: stare
    18: stare-eyeballing
    19: shy
    20: love
    21: excited
    22: snooze-default
    23: mataku
    24: sweating
    25: surprised-shy
    26: shy-confused(spinny eyes)
    27: shy-confused(><)
    28: shocked/scared
    29: defaulthappy
    30: engaged-love(hungry?)
    31: engaged-defaulthalo
    32: snooze-deep
    33: snooze-thinking
    34: engaged-greenhalo
    35: disgust-mild
    36: disgust
    37: shock(o.o)
    38: shock-spinny
    39: shock-spinnywithmouth
    40: default-mouthopen
    41: default-happyeyeclosed
    42: default-afk
    */
    // Start is called before the first frame update
    void Start() {
        Button btn = Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
//        animationManager._AsyncAnimation(1, AnimClip[9], false, 1f, animationMix);
//        WARNING!! ALWAYS USE _ASYNCANIMATION'S ANIMTRACK AS 1, IF YOU USE ANOTHER NUMBER, YOU WILL GET THESE BUGS :
//        1. Idle animation movement not working, only Pet/Look Working, 2. Pet/Look Animation Bugging, not overlaying it properly.

    }
     void OnClick() {
        animationManager._AsyncAnimation(0, AnimClip[selectedAnimationNum], false, 1f, animationMix);
    }

}

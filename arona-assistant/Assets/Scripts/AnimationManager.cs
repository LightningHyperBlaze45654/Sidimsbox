using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AnimationManager : MonoBehaviour
{
    public AnimationReferenceAsset[] AnimClip = new AnimationReferenceAsset[42]; // 0: Idle, 1: Pet_A, 2: Pet_M, 3: PetEnd_A, 4: PetEnd_M, 5: Look_01_A/M, 6: Look_01_M, 7: LookEnd_01_A, 8: LookEnd_01_M
    public Collider2D area;
    public SkeletonAnimation skeletonAnimation;
    public GameObject touchPoint;
    public GameObject eyePoint;

    [Range(0f, 1f)]
    public float animationMix;
    [Range(0f, 0.05f)]
    public float touchPointBoneAlpha;

    [Range(0f, 1f)]
    public float eyePointVerticalBound;
    [Range(0f, 2f)]
    public float eyePointHorizontalBound;

    public enum AnimState
    {
        Idle, Pet, Look
    }

    Transform touchPointTransform;
    Transform eyePointTransform;
    SkeletonUtilityBone touchPointBone;
    SkeletonUtilityBone eyePointBone;

    AnimState _AnimState;
    AnimState _PrevState;
    string[] CurrentAnimation = new string[30];

    Vector3 velocity = Vector3.zero;
    bool currentlyOnClick = false;
    bool currentlyOnTrack = false;

    // Start is called before the first frame update
    void Start()
    {
        touchPointTransform = touchPoint.GetComponent<Transform>();
        eyePointTransform = eyePoint.GetComponent<Transform>();
        touchPointBone = touchPoint.GetComponent<SkeletonUtilityBone>();
        eyePointBone = eyePoint.GetComponent<SkeletonUtilityBone>();
        for (int i=0;i<4;i++) {
            _AsyncAnimation(i,AnimClip[0],true,1f,animationMix);
        }
        touchPointBone.overrideAlpha = touchPointBoneAlpha;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector2(Mathf.Clamp(Input.mousePosition.x,0,Screen.width), Mathf.Clamp(Input.mousePosition.y,0,Screen.height)));
        if (Input.GetMouseButton(0)) {
            if ((area.OverlapPoint(mousePos) || currentlyOnClick) && !currentlyOnTrack) {
                currentlyOnClick = true;
            } else {
                currentlyOnTrack = true;
            }
            if (currentlyOnClick) {
                Vector3 mouseLocalPos = touchPointTransform.InverseTransformPoint(mousePos);
                touchPointTransform.localPosition = Vector3.SmoothDamp(touchPointTransform.localPosition, new Vector3(mouseLocalPos.x, mouseLocalPos.y, touchPointTransform.position.z), ref velocity, 0.1f);
                _AnimState = AnimState.Pet;
            }
            if (currentlyOnTrack) {
                Vector3 mouseLocalPos = eyePointTransform.InverseTransformPoint(mousePos);
                eyePointTransform.localPosition = Vector3.SmoothDamp(eyePointTransform.localPosition, new Vector3(Mathf.Clamp(mouseLocalPos.x,-eyePointVerticalBound,eyePointVerticalBound), Mathf.Clamp(mouseLocalPos.y,-eyePointHorizontalBound,eyePointHorizontalBound), eyePointTransform.localPosition.z), ref velocity, 0.2f);
                _AnimState = AnimState.Look;
            }
            _PrevState = _AnimState;
        } else {
            currentlyOnTrack = false;
            currentlyOnClick = false;
            if(_AnimState != AnimState.Idle) {
                _AnimState = AnimState.Idle;
            }
        }
        SetCurrentAnimation(_AnimState);
    }

    public void _AsyncAnimation(int animTrack, AnimationReferenceAsset animClip, bool loop, float timeScale, float mixDuration) 
    {
        if(animClip.name.Equals(CurrentAnimation[animTrack])) {
            return;
        }
        var setAnim = skeletonAnimation.state.SetAnimation(animTrack, animClip, loop);
        setAnim.TimeScale = timeScale;
        setAnim.MixDuration = mixDuration;
        CurrentAnimation[animTrack] = animClip.name;
    }

    public void SetCurrentAnimation(AnimState _state)
    {

        switch (_state) {
            case AnimState.Idle:
                switch (_PrevState) {
                    case AnimState.Pet:
                        touchPointTransform.localPosition = Vector3.SmoothDamp(touchPointTransform.localPosition, Vector3.zero, ref velocity, 0.1f);
                        
                        _AsyncAnimation(2,AnimClip[3],false,1f,animationMix);
                        _AsyncAnimation(3,AnimClip[4],false,1f,animationMix);
                        break;
                    case AnimState.Look:
                        eyePointTransform.localPosition = Vector3.SmoothDamp(eyePointTransform.localPosition, Vector3.zero, ref velocity, 0.1f);
                        _AsyncAnimation(2,AnimClip[7],false,1f,animationMix);
                        _AsyncAnimation(3,AnimClip[8],false,1f,animationMix);
                        break;
                    default:
                        break;
                }
                break;
            case AnimState.Pet:
                _AsyncAnimation(2,AnimClip[1],true,1f,animationMix);
                _AsyncAnimation(3,AnimClip[2],true,1f,animationMix);
                break;
            case AnimState.Look:
                _AsyncAnimation(2,AnimClip[5],true,1f,animationMix);
                _AsyncAnimation(3,AnimClip[6],true,1f,animationMix);
                break;
        }
    }
}

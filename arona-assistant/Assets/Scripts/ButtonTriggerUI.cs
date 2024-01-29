using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonTriggerUI : MonoBehaviour
{
    public Canvas canvas;
    public Button Button;
    public Animator _animator;
    private bool isOpen = false;
    void Start()
    {
        Button btn = Button.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        print("hi");
       if (isOpen) {
            _animator.SetTrigger("closeui");
            isOpen = false;
        } else {
            _animator.SetTrigger("openui");
            isOpen = true;
        }
    }
}

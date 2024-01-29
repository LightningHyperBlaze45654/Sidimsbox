using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    float targetAspect = 4f/3f;
    float initOrthographicSize;
    
    // Start is called before the first frame update
    void Start()
    {
        initOrthographicSize = Camera.main.orthographicSize;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate() {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        if (targetAspect < screenAspect) {
            Camera.main.orthographicSize = initOrthographicSize * (targetAspect / Camera.main.aspect);
        } else {
            Camera.main.orthographicSize = initOrthographicSize;
        }
    }
}

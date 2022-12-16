using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] GameObject EyeMaskUp;
    [SerializeField] GameObject EyeMaskDown;
    [SerializeField] Blur BlurEffect;

    public event EventNoParam OnGetUpEvent;
    public event EventNoParam OnPassOutEvent;
    public event EventNoParam EndSceneEvent;

    public void OnGetUp()
    {
        // disable masks
        EyeMaskUp.SetActive(false);
        EyeMaskDown.SetActive(false);
        BlurEffect.enabled = false;

        if(OnGetUpEvent != null) OnGetUpEvent();
    }
    
    public void OnPassOut()
    {
        // enable masks
        EyeMaskUp.SetActive(true);
        EyeMaskDown.SetActive(true);
        BlurEffect.enabled = true;

        if(OnPassOutEvent != null) OnPassOutEvent();
    }

    public void EndScene()
    {
        if(EndSceneEvent != null) EndSceneEvent();
    }
}

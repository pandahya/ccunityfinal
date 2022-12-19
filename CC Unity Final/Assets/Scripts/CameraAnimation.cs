using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] GameObject EyeMaskUp;
    [SerializeField] GameObject EyeMaskDown;
    [SerializeField] Blur BlurEffect;

    public void OnGetUp()
    {
        // disable masks
        EyeMaskUp.SetActive(false);
        EyeMaskDown.SetActive(false);
        BlurEffect.enabled = false;
    }
    
    public void OnPassOut()
    {
        // enable masks
        EyeMaskUp.SetActive(true);
        EyeMaskDown.SetActive(true);
        BlurEffect.enabled = true;
    }
}

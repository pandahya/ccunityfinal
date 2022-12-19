using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostFX_Manager : MonoBehaviour
{
    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    LensDistortion m_LensDistortion;
    DepthOfField m_DepthOfField;
    private void Start()
    {
        m_Volume = GetComponent<PostProcessVolume>();

        // Get postFX profiles
        m_Volume.profile.TryGetSettings(out m_Vignette);
        m_Volume.profile.TryGetSettings(out m_LensDistortion);
        m_Volume.profile.TryGetSettings(out m_DepthOfField);
        // m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        // m_LensDistortion = ScriptableObject.CreateInstance<LensDistortion>();
    }
    public void EnableEffects(bool val)
    {
        m_Vignette.active = val;
        m_LensDistortion.active = val;
        m_DepthOfField.active = val;
    }
    // void Update()
    // {
    //     m_Vignette.intensity.value = 0.4f + Mathf.Sin(Time.realtimeSinceStartup) * 0.2f;
    // }
    // destroy the volume and the attached profile
    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}

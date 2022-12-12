using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    // assign the renderers here
    [SerializeField]
    private List<Renderer> renderers;

    [SerializeField]
    private Color highlightColor = Color.white; // highlight color, white in default

    [SerializeField]
    private List<Material> materials; // cach all the materials of the object
    
    // get all the materials from each renderer
    private void Awake()
    {
        renderers.Add(this.GetComponent<MeshRenderer>());
        // get all the materials
        materials = new List<Material>();
        foreach(var renderer in renderers)
        {
            // get all the instantiated materials of this object.
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool val)
    {
        if(val)
        {
            foreach(var material in materials)
            {
                // enable the EMISSION
                material.EnableKeyword("_EMISSION");
                // before set the color
                material.SetColor("_EmissionColor", highlightColor);
            }
        }
        else
        {
            foreach(var material in materials)
            {
                // disable the EMISSION
                material.DisableKeyword("_EMISSION");
            }
        }
    }
}

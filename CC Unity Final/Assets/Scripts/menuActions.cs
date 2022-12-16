using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuActions : MonoBehaviour
{
    //public GameObject insButton;
    //public GameObject startButton;
    public GameObject insPanel;
    public GameObject fader;

    public float fadeSpeed;
    bool startPressed; 
    float t;
    Color faderColor;

    void Start()
    {
        
    }

    void Update()
    {
        t += Time.deltaTime * fadeSpeed;
        fader.GetComponent<Image>().color = faderColor;

        if (startPressed)
        {
            fader.SetActive(true);
            faderColor = new Color(faderColor.r, faderColor.g, faderColor.b, Mathf.Lerp(faderColor.a, 1, t));

            if (faderColor.a >= .99f)
            {
                SceneManager.LoadScene("Scene00");
            }
        }
    }

    public void showInstructions()
    {
        insPanel.SetActive(true);
    }
    public void hideInstructions()
    {
        insPanel.SetActive(false);
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
    public void ButtonStart()
    {
        startPressed = true;
    }
}

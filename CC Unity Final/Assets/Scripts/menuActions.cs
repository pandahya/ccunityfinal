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
    bool startPressed = true;
    bool isFadeIn = true;
    float t;
    Color faderColor = Color.black;

    void Update()
    {
        t += Time.deltaTime * fadeSpeed;
        fader.GetComponent<Image>().color = faderColor;
        
        if (startPressed)
        {
            fader.SetActive(true);
            if(isFadeIn)
                faderColor = new Color(faderColor.r, faderColor.g, faderColor.b, Mathf.Lerp(faderColor.a, 0, t));
            else
                faderColor = new Color(faderColor.r, faderColor.g, faderColor.b, Mathf.Lerp(faderColor.a, 1, t));

            if(faderColor.a <= .01f && isFadeIn)
            {
                fader.SetActive(false);
                startPressed = false;
                isFadeIn = false;
            }
            else if (faderColor.a >= .99f && !isFadeIn)
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
        isFadeIn = false;
    }
}

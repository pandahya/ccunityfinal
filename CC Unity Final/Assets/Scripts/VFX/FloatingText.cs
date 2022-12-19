using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    Text content;
    string state = "Dormant";
    float stayTime = 0, fadingRate;

    // Start is called before the first frame update
    void Start()
    {
        content = this.GetComponent<Text>();
        // clear the contents
        content.text = "";
        content.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = content.color.a;
        switch(state)
        {
            case "FadeIn":
                if(alpha < 1.0f)
                    content.color = new Color(255, 255, 255, alpha += fadingRate * Time.deltaTime);
                else state = "Stay";
                break;
            case "FadeOut":
                if(alpha > 0)
                    content.color = new Color(255, 255, 255, alpha -= fadingRate * Time.deltaTime);
                else state = "Dormant";
                break;
            case "Stay":
                if(stayTime > 0) stayTime -= Time.deltaTime;
                else state = "FadeOut";
                break;
            default:
                break;
        }
    }
    
    public void CreateFloatingText(string text, float stay = 2.0f, float rate = 1.0f)
    {
        while(state == "Dormant") // invalid creation when the floating text is working
        {
            content.text = text; // assign the text value
            stayTime = stay; // reset staying time
            fadingRate = rate;
            state = "FadeIn"; // start fade-in
        }
    }

    public string GetState()
    {
        return state;
    }
}

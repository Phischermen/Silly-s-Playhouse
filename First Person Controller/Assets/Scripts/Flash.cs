using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public TextFader prompt_controller;
    private Text ui_text;
    private float alpha;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        ui_text = GetComponent<Text>();
        alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(prompt_controller.state == TextFader.prompt_state.fading_out)
        {
            alpha = Mathf.Min(prompt_controller.alpha, alpha);
        }
        else
        {
            if (t < 0f)
            {
                alpha = Mathf.Lerp(alpha, 0, 0.1f);
                ui_text.color = new Color(1, 0.71237f, 0, alpha);
            }
            else
            {
                t -= Time.deltaTime;
            }
        }
        ui_text.color = new Color(1, 0.71237f, 0, alpha);
    }
    public void flash_text(string message,float time)
    {
        ui_text.text = message;
        t = time;
        alpha = 1;
    }
}

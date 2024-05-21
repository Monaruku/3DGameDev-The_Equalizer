using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryBackgroundController : MonoBehaviour
{
    public bool isFirst = true;
    public bool isSwitched;
    public Image background1;
    public Image background2;
    public Animator animator;

    public void switchImage(Sprite sprite)
    {
        if (isFirst)
        {
            animator.SetTrigger("SwitchStart");
            isFirst = false;
        }
        else
        {
            if (!isSwitched)
            {
                background2.sprite = sprite;
                animator.SetTrigger("SwitchFirst");
            }
            else
            {
                background1.sprite = sprite;
                animator.SetTrigger("SwitchSecond");
            }
            isSwitched = !isSwitched;
        }
        
    }

    public void setImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            background1.sprite = sprite;
        }
        else
        {
            background2.sprite = sprite;
        }
    }


    void Start()
    {
        isFirst = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

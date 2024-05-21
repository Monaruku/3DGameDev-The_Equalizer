using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public Animator animator;
    public GameObject panel;


    void Start()
    {

    }


    public void OnButtonPress()
    {
        animator.SetTrigger("FadeOut");
        //change scene
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class ClickRestrictedButton : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private AudioSource audioSource;
    private Button button;
    private bool isButtonClickable = true;
    private float clickCooldown = 0.7f;
    public bool canClick;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        canClick = true;
    }

    void Update()
    {
        // Check if the cooldown has passed, and if so, make the button clickable again
        if (!isButtonClickable && canClick)
        {
            clickCooldown -= Time.deltaTime;
            if (clickCooldown <= 0f)
            {
                isButtonClickable = true;
                button.interactable = true;
                clickCooldown = 0.7f;
            }
        }
    }

    void OnButtonClick()
    {
        if (isButtonClickable)
        {
            // Play the sound effect
            if (buttonClickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(buttonClickSound);
            }
            isButtonClickable = false;
            button.interactable = false;
        }
    }
}

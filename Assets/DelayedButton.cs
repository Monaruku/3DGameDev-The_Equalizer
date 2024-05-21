using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayedButtonAppearance : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    void Start()
    {
        // Start the coroutine to enable buttons after 3 seconds
        StartCoroutine(EnableButtonsAfterDelay(1f));
    }

    IEnumerator EnableButtonsAfterDelay(float delay)
    {
        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        button1.interactable = true;
        button2.interactable = true;
        button3.interactable = true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour
{
    public GameObject firstPart;
    public GameObject secondPart;
    public GameObject thirdPart;
    public GameObject fourthPart;
    public GameObject tutorialObj;
    public GameObject eventSystem;

    public bool canProgress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firstPart.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse0) && canProgress)
        {
            canProgress = false;
            firstPart.SetActive(false);
            secondPart.SetActive(true);
            StartCoroutine(progressTimer());
        }
        if (secondPart.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse0) && canProgress)
        {
            canProgress = false;
            secondPart.SetActive(false);
            thirdPart.SetActive(true);
            StartCoroutine(progressTimer());
        }
        if (thirdPart.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse0) && canProgress)
        {
            canProgress = false;
            thirdPart.SetActive(false);
            fourthPart.SetActive(true);
            StartCoroutine(progressTimer());
        }
        if (fourthPart.activeInHierarchy && Input.GetKeyDown(KeyCode.Mouse0) && canProgress)
        {
            fourthPart.SetActive(false);
            tutorialObj.SetActive(false);
            eventSystem.GetComponent<EventSystem>().enabled = true;
        }
    }

    public void startTutorial()
    {
        canProgress = false;
        eventSystem.GetComponent<EventSystem>().enabled = false;
        firstPart.SetActive(true);
        StartCoroutine(progressTimer());
    }

    IEnumerator progressTimer()
    {
        yield return new WaitForSeconds(3);
        canProgress = true;
    }
}

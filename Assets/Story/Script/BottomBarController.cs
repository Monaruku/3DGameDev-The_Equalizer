using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BottomBarController : MonoBehaviour
{
    public TextMeshProUGUI barText;

    private int sentenceIndex = -1;
    private StoryScene currentScene;
    private State state = State.COMPLETED;
    

    private enum State
    {
        PLAYING, COMPLETED
    }

    public void Update()
    {
        if(isCompleted())
        {
            GameObject.Find("BottomBar").GetComponentInChildren<RawImage>().enabled = true;
        }
        else
        {
            GameObject.Find("BottomBar").GetComponentInChildren<RawImage>().enabled = false;
        }
    }

    public void playScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        playNextSentence();
    }

    public void playNextSentence()
    {
        barText.text = "";
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
    }

    public bool isCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool isLastSentence()
    {
        return sentenceIndex +1 == currentScene.sentences.Count;
    }

    public void playFullSentence()
    {
        barText.text = currentScene.sentences[sentenceIndex].text;
        state = State.COMPLETED;
        StopCoroutine(TypeText(currentScene.sentences[sentenceIndex].text));
    }
    private IEnumerator TypeText(string text)
    {
        state = State.PLAYING;
        int wordIndex = 0;

        while(state != State.COMPLETED)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(0.05f);

            if(++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }
}

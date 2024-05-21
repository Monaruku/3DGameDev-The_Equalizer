using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeController : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    public StoryBackgroundController storyBackground;

    // Start is called before the first frame update
    void Start()
    {
        bottomBar.playScene(currentScene);
        storyBackground.switchImage(currentScene.background);
    }

    // Update is called once per frame
    void Update()
    {
        if (!SettingMenu.settingMenuOn)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
            {
                if (bottomBar.isCompleted())
                {
                    if (bottomBar.isLastSentence())
                    {
                        if (currentScene.nextScene == null) //after last scene
                        {
                            UIManager.NarrativeDone = true;
                        }
                        else
                        {
                            currentScene = currentScene.nextScene;
                            bottomBar.playScene(currentScene);
                            storyBackground.switchImage(currentScene.background);
                        }
                    }
                    else
                    {
                        bottomBar.playNextSentence();
                    }


                }
                else
                {
                    bottomBar.playFullSentence();
                }
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class UIManager : MonoBehaviour
{
    public string levelToLoad;
    public AudioSource buttonHoverAudio;
    public AudioSource buttonClickedAudio;
    public GameObject loadingScreen;
    public Image loadingBarFill;

    public static bool CreditAnimationDone = false;
    public static bool NarrativeDone = false;
    public static bool PlayerDead = false;

    // Start is called before the first frame update

    void Start()
    {
        Cursor.lockState = false ? CursorLockMode.Locked : CursorLockMode.None;

    }

    // Update is called once per frame
    void Update()
    {
        if (CreditAnimationDone)
        {
            levelToLoad = "GameMenu";
            CreditAnimationDone = false;
            StartCoroutine(ChangeSceneWithLoading());
        }

        if (NarrativeDone)
        {
            levelToLoad = "FirstStage";
            NarrativeDone = false;
            StartCoroutine(ChangeSceneWithLoading());
        }

        if (PlayerDead)
        {
            levelToLoad = "Another Scene";
            PlayerDead = false;
            StartCoroutine(ChangeSceneWithLoading());
        }
    }

    public void playBtnSound()
    {
        buttonClickedAudio.Play();
    }

    public void playBtnHoverSound()
    {
        buttonHoverAudio.Play();
    }


    public void PlayBtnCLicked()
    {
        levelToLoad = "Cutscene";
        playBtnSound();
        StartCoroutine(WaitChangeScene());
        
    }

    public void SettingsBtnCLicked()
    {
        playBtnSound();
        SettingMenu.settingMenuOn = true;
    }

    public void ExitBtnCLicked()
    {
        levelToLoad = "Exit";
        playBtnSound();
        StartCoroutine(WaitQuit());

    }
    public void CreditBtnClicked()
    {
        levelToLoad = "Another Scene";
        playBtnSound();
        StartCoroutine(WaitChangeScene());

    }

    public void SettingBackBtnClicked()
    {
        playBtnSound();
        SettingMenu.settingMenuOn = false;
    }

    public void ReturnToLobbyBtnClicked()
    {
       
        levelToLoad = "Lobby";
        playBtnSound();
        StartCoroutine(WaitChangeScene());
    }

    public void ReturnToMainMenuBtnCLicked()
    {
        levelToLoad = "GameMenu";
        playBtnSound();
        StartCoroutine(WaitChangeScene());
    }

    public void ContinueBtnCLicked()
    {
        playBtnSound();
    }

    IEnumerator WaitQuit()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Exit!!!");
        Application.Quit();
    }

    IEnumerator WaitChangeScene()
    {
        SettingMenu.settingMenuOn = false;
        yield return new WaitForSeconds(0.5f);
        if(SceneManager.GetActiveScene().name != levelToLoad)
        {
            StartCoroutine(ChangeSceneWithLoading());
        }
    }
     
    IEnumerator ChangeSceneWithLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBarFill.fillAmount = progressValue;

            
            yield return null;
        }

    }
    
}

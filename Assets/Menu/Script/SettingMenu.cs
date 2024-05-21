using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public VolumeProfile volumeProfile;
    public Dropdown resolutionDropDown;
    public Dropdown graphicDropdown;
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Button fullScreenButton;
    Resolution[] resolutions;
    public static bool settingMenuOn = false;
    public bool isFullScreen;

    private ColorAdjustments colorAdjustment;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("SettingMenu").GetComponent<Canvas>().enabled = settingMenuOn;

        if (PlayerPrefs.HasKey("fullScreen"))
        {
            loadFullScreen();
        }
        else
        {
            isFullScreen = Screen.fullScreen;
        }

        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> resolutionOptions = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio + "hz";
            resolutionOptions.Add(option);
            Debug.Log(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropDown.AddOptions(resolutionOptions);

        if (PlayerPrefs.HasKey("resolution"))
        {
            loadResolution();
        }
        else
        {
            resolutionDropDown.value = currentResolutionIndex;
            resolutionDropDown.RefreshShownValue();
        }


        if (PlayerPrefs.HasKey("quality"))
        {
            loadQuality();
        }
        else
        {
            graphicDropdown.value = QualitySettings.GetQualityLevel();
            graphicDropdown.RefreshShownValue();
        }


        if (PlayerPrefs.HasKey("masterVolume"))
        {
            loadVolume();
        }

        if (PlayerPrefs.HasKey("brightness"))
        {
            loadBrightness();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            settingMenuOn = !settingMenuOn;
        }

        GameObject.FindWithTag("SettingMenu").GetComponent<Canvas>().enabled = settingMenuOn;

        if (SceneManager.GetActiveScene().name == "GameMenu")
        {
            GameObject.FindWithTag("GameMenu").GetComponent<Canvas>().enabled = !settingMenuOn;
        }

        if (isFullScreen)
        {
            fullScreenButton.GetComponentInChildren<Text>().text = "On";
        }
        else
        {
            fullScreenButton.GetComponentInChildren<Text>().text = "Off";
        }

        if (settingMenuOn)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", (Mathf.Log10(volume) * 20));
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    private void loadVolume()
    {
        float volume = PlayerPrefs.GetFloat("masterVolume");
        volumeSlider.value = volume;
        setVolume(volume);
    }

    public void setBrightness(float brightness)
    {
        volumeProfile.TryGet(out colorAdjustment);
        {
            colorAdjustment.postExposure.value = brightness;
        }

        PlayerPrefs.SetFloat("brightness", brightness);
    }

    public void loadBrightness()
    {
        float brightness = PlayerPrefs.GetFloat("brightness");
        brightnessSlider.value = brightness;
        setBrightness(brightness);
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    private void loadQuality()
    {
        int quality = PlayerPrefs.GetInt("quality");
        QualitySettings.SetQualityLevel(quality);
        graphicDropdown.value = QualitySettings.GetQualityLevel();
        graphicDropdown.RefreshShownValue();
    }

    public void setFullScreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        if (isFullScreen)
        {
            PlayerPrefs.SetInt("fullScreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullScreen", 0);
        }
    }

    private void loadFullScreen()
    {
        int fullScreen = PlayerPrefs.GetInt("fullScreen");
        if (fullScreen == 0)
        {
            isFullScreen = false;
        }
        else
        {
            isFullScreen = true;
        }
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }

    private void loadResolution()
    {
        int resolutionIndex = PlayerPrefs.GetInt("resolution");
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropDown.value = resolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

}

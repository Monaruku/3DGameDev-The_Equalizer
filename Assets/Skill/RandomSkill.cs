using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillData
{
    public int skillID;
    public Sprite skillImage;
}

public class RandomSkill : MonoBehaviour
{
    public List<Button> randomButtons;
    public Transform targetTransform;
    public List<SkillData> skillList = new List<SkillData>();
    public AudioClip buttonClickSound; 

    private List<Image> buttonImages = new List<Image>();
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (Button randomButton in randomButtons)
        {
            randomButton.onClick.AddListener(() => OnRandomButtonClick(randomButton));
            buttonImages.Add(randomButton.GetComponent<Image>());
        }

        UpdateButtonImages();
    }

    void OnRandomButtonClick(Button clickedButton)
    {
        if (skillList.Count > 0)
        {
            if (buttonClickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(buttonClickSound);
            }

            int randomIndex = Random.Range(0, skillList.Count);
            SkillData selectedSkill = skillList[randomIndex];

            int buttonIndex = randomButtons.IndexOf(clickedButton);
            DisplaySkill(selectedSkill, buttonImages[buttonIndex]);

            skillList.RemoveAt(randomIndex);
        }
    }

    void DisplaySkill(SkillData selectedSkill, Image buttonImage)
    {
        buttonImage.sprite = selectedSkill.skillImage;
        Debug.Log("Selected Skill ID: " + selectedSkill.skillID);
    }

    void UpdateButtonImages()
    {
        foreach (Image buttonImage in buttonImages)
        {
            if (skillList.Count > 0)
            {
                int randomIndex = Random.Range(0, skillList.Count);
                buttonImage.sprite = skillList[randomIndex].skillImage;
            }
        }
    }
}



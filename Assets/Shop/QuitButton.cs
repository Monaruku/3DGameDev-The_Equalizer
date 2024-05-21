using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    public Button quitButton;
    public AudioClip buttonPressSound; 
    private AudioSource audioSource;
    public GameObject _player;

    private string playerTag = "Player";

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        quitButton.onClick.AddListener(Quit);
        _player = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Quit()
    {
        if (buttonPressSound != null && audioSource != null)
        {
            audioSource.Play();
            audioSource.PlayOneShot(buttonPressSound);
        }
        _player.GetComponent<StarterAssets.ThirdPersonController>().disableShop();
        transform.parent.gameObject.transform.parent.gameObject.SetActive(false);

    }
}

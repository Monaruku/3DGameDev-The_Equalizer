using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPerson : MonoBehaviour
{

    public GameObject shopUI;
    public GameObject interactText;
    private bool canTrigger;
    public GameObject _player;
    public Button[] buttons;

    private string playerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        canTrigger = false;
        _player = GameObject.FindGameObjectWithTag(playerTag);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canTrigger)
        {
            shopUI.SetActive(true);
            foreach (var item in buttons)
            {
                item.interactable = true;
            }
            _player.GetComponent<StarterAssets.ThirdPersonController>().enableShop();
            canTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            interactText.SetActive(true);
            canTrigger = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        interactText.SetActive(false);
        canTrigger = false;
    }
}

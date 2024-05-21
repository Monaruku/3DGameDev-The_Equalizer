using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpGuy : MonoBehaviour
{
    public bool helpGuyFirstTime;
    public bool canInteract;
    public GameObject interactText;
    public StarterAssets.ThirdPersonController _player;
    public ShopManager _shop;
    public GameObject _interactUI;
    public TextMeshProUGUI _text;

    private float typeSpeed;
    private bool typing;
    private bool canProceed;
    private bool final;


    private string playerTag = "Player";

    private string FirstSentence = "Hey pipsqueak, I heard it's your first time coming into here.";
    private string SecondSentence = "Here's 1000 coins for you. You see that guy over there right?";
    private string ThirdSentence = "He's the merchant around here, spend your coins there and get some stuff for yourself.";
    private string FourthSentence = "It's my treat, don't worry about it. HAHAHAHAHA";
    // Start is called before the first frame update
    void Start()
    {
        helpGuyFirstTime = true;
        typing = false;
        canProceed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && helpGuyFirstTime && canInteract)
        {
            _shop.coins += 1000;
            _player.enableShop();
            helpGuyFirstTime = false;
            interactText.SetActive(false);
            _interactUI.SetActive(true);
            StartCoroutine(typeFull());
        }

        if (Input.GetMouseButtonDown(0) && typing)
        {
            typeSpeed = 0;
        }

        if (Input.GetMouseButtonDown(0) && !typing)
        {
            canProceed = true;
        }

        if (Input.GetMouseButtonDown(0) && final)
        {
            if (_player != null)
            {
                _player.disableShop();
                _interactUI.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && helpGuyFirstTime)
        {
            interactText.SetActive(true);
            canInteract = true;
            _player = other.gameObject.GetComponent<StarterAssets.ThirdPersonController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactText.SetActive(false);
        canInteract = false;
        _player = null;
    }

    IEnumerator typeFull()
    {
        StartCoroutine(type(FirstSentence));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => canProceed == true);
        StartCoroutine(type(SecondSentence));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => canProceed == true);
        StartCoroutine(type(ThirdSentence));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => canProceed == true);
        StartCoroutine(type(FourthSentence));
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => canProceed == true);
        final = true;
    }

    IEnumerator type(string s)
    {
        typing = true;
        canProceed = false;
        typeSpeed = 0.1f;
        _text.text = "";
        foreach (char c in s.ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        typing = false;
    }
}

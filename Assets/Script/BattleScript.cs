using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class BattleScript : MonoBehaviour
{
    public GameObject battleSystems;
    public GameObject battleCamera;
    public GameObject mainCamera;
    public GameObject globalLight;

    public GameObject player;
    public Transform playerBattlePos;

    public GameObject[] goblinWaypoint;
    public GameObject goblin;
    public Transform minionBattlePos1;
    public Transform minionBattlePos2;
    public GameObject minionSpotLight1;
    public GameObject minionSpotLight2;
    public GameObject minionHealthbar1;
    public GameObject minionHealthbar2;
    public GameObject onFieldGoblin1;
    public GameObject onFieldGoblin2;
    public static bool goblinFirst;
    public static bool goblinNext;
    public static bool readyToEnd;
    public static bool hasRun;
    public static bool endRun;
    public List<int> waypoint;
    public int waypointNum;
    public GameObject goblinPatrol;

    public GameObject castUI;
    public GameObject castCard;
    public GameObject battleUI;
    public bool inCastUI;
    public bool inGuardUI;
    public bool inTransition;

    public int enemyToSpawn;
    public static bool enemyPos1;
    public static bool enemyPos2;
    public static int currentCardID;
    public GameObject targetCard;
    public static int attackPos;
    public static bool inBattle;
    public bool onEndOnce;
    public PlayerDeck _deck;
    public bool canCancel;
    public static int totalEncounter;
    public GameObject _tutorial;


    public GameObject fadeToBlackScreen;

    public TurnSystem _turn;
    public ShopManager _shop;
    public Vector3 playerLastPos;
    public bool inBossBattle;
    public bool bossRun;
    public GameObject bossPrefab;
    public GameObject onFieldBoss;
    public Transform bossBattlePos;
    public GameObject bossHealthBar;

    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public GameObject joystickGame;
    public GameObject noManaText;

    public string minionPositionString = "MinionPosition";
    public string cameraCastAniString = "CameraCast";
    public string cameraDefaultAniString = "CameraDefault";
    public string enemyPatrolString = "EnemyPatrol";

    //Cheat Stuff
    public GameObject cheatUI;
    public TextMeshProUGUI cheatAtkDisplay;
    public TextMeshProUGUI cheatMoneyDisplay;
    public bool playerCursorHidden;

    public AudioClip[] BGM;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyPos2 = true;
        enemyPos1 = true;
        currentCardID = 0;
        attackPos = 0;
        inTransition = false;
        goblinNext = false;
        endRun = false;
        bossRun = false;
        inBattle = false;
        onEndOnce = false;
        playerCursorHidden = true;
        totalEncounter = 0;
        foreach (var item in BGM)
        {
            item.LoadAudioData();
        }
        if (Application.platform == RuntimePlatform.Android)        //Mobile Joystick
        {
            joystickGame.SetActive(true);
        }
        _audio = this.GetComponent<AudioSource>();
        _audio.clip = BGM[0];
        _audio.Play();

        //spawn patrol enemy
        waypoint = new List<int>(new int[5]);
        for (int i = 0; i < 5; i++)
        {
            waypointNum = Random.Range(0, 11);

            while (waypoint.Contains(waypointNum))
            {
                waypointNum = Random.Range(0, 11);
            }
            waypoint[i] = waypointNum;
        }
        foreach (var item in waypoint)
        {
            Instantiate(goblinPatrol, goblinWaypoint[item].transform.position, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            PlayerPrefs.SetInt("FirstTime", 0);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            cheatUI.SetActive(!cheatUI.activeInHierarchy);
            if (!inBattle && !inBossBattle)
            {
                if (cheatUI.activeInHierarchy)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().enableShop();
                }
                else
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().disableShop();
                }
            }
        }
        if (enemyToSpawn > 0)
        {
            if (!enemyPos1)
            {
                onFieldGoblin1 = Instantiate(goblin, (minionBattlePos1.transform.position + Vector3.forward * 4), Quaternion.identity) as GameObject;
                onFieldGoblin1.GetComponent<EnemyController>().moveCharacter(minionBattlePos1.transform.position);
                enemyToSpawn -= 1;
                enemyPos1 = true;
            }
            if (!enemyPos2)
            {
                onFieldGoblin2 = Instantiate(goblin, (minionBattlePos2.transform.position + Vector3.forward * 4), Quaternion.identity);
                onFieldGoblin2.GetComponent<EnemyController>().moveCharacter(minionBattlePos2.transform.position);
                enemyToSpawn -= 1;
                enemyPos2 = true;
            }
        }
        if (inBossBattle && !bossRun)
        {
            onFieldBoss = Instantiate(bossPrefab, (bossBattlePos.transform.position + Vector3.forward * 4), Quaternion.identity);
            onFieldBoss.GetComponent<BossController>().moveCharacter(bossBattlePos.transform.position);
            bossRun = true;
        }
        if (enemyToSpawn <= 0 && !enemyPos1 && !enemyPos2 && !onEndOnce && onFieldBoss == null)
        {
            StartCoroutine(endBattle());
            onEndOnce = true;
        }
        if (inCastUI)
        {
            if (Input.GetMouseButtonDown(1) && !inTransition && canCancel)
            {
                exitCastUI();
            }
        }
        if (inGuardUI)
        {
            if (Input.GetMouseButtonDown(1) && canCancel)
            {
                exitGuardUI();
            }
        }
        if (readyToEnd && !endRun)
        {
            _turn.EndOpponentTurn();
            goblinNext = false;
            goblinFirst = false;
            readyToEnd = false;
            endRun = true;
        }
        if (_turn.isEnemyTurn && !hasRun)
        {
            if (!inBossBattle)
            {
                if (!goblinFirst)
                {
                    if (onFieldGoblin1 != null)
                    {
                        onFieldGoblin1.GetComponent<EnemyController>().onTurn(minionBattlePos1.position, 1);
                    }
                    else
                    {
                        goblinNext = true;
                    }
                }
                if (goblinNext)
                {
                    if (onFieldGoblin2 != null)
                    {
                        onFieldGoblin2.GetComponent<EnemyController>().onTurn(minionBattlePos2.position, 2);
                    }
                }
                hasRun = true;
            }
            else
            {
                onFieldBoss.GetComponent<BossController>().onTurn(bossBattlePos.position, 0);
                hasRun = true;
            }
        }

        if (inBattle)
        {
            if (onFieldGoblin1 != null)
            {
                minionHealthbar1.GetComponent<Slider>().value = onFieldGoblin1.GetComponent<EnemyController>().health;
            }
            if (onFieldGoblin2 != null)
            {
                minionHealthbar2.GetComponent<Slider>().value = onFieldGoblin2.GetComponent<EnemyController>().health;
            }
        }

        //Setting menu unlock cursor
        if (SettingMenu.settingMenuOn)
        {
            player.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
            player.GetComponent<StarterAssets.StarterAssetsInputs>().SetCursorState(false);
        }
        else
        {
            player.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
            //player.GetComponent<StarterAssets.StarterAssetsInputs>().SetCursorState(true);
        }

        if (bossHealthBar.activeInHierarchy)
        {
            if (onFieldBoss != null)
            {
                bossHealthBar.GetComponent<Slider>().value = onFieldBoss.GetComponent<BossController>().health;
                bossHealthBar.GetComponent<Slider>().maxValue = onFieldBoss.GetComponent<BossController>().maxHealth;
            }
        }
    }

    ////// Cheat Stuff ///////
    public void cheatEnemySpawn()
    {
        enemyToSpawn = 0;
    }

    public void cheatDamage()
    {
        player.GetComponent<StarterAssets.ThirdPersonController>().attackModifier += 100;
        cheatAtkDisplay.text = "Current Attack : " + player.GetComponent<StarterAssets.ThirdPersonController>().attackModifier;
    }

    public void cheatAddPotion()
    {
        player.GetComponent<StarterAssets.ThirdPersonController>().potionAmt += 1;
    }

    public void cheatMoney()
    {
        _shop.coins += 10000;
        cheatMoneyDisplay.text = "Current Money : " + _shop.coins;
    }

    public void cheatRemovePatrol()
    {
        GameObject[] pos = GameObject.FindGameObjectsWithTag(enemyPatrolString);
        foreach (var item in pos)
        {
            Destroy(item.gameObject);
        }
    }

    public void cheatKillMe()
    {
        player.GetComponent<StarterAssets.ThirdPersonController>().health = 2;
    }

    public void startDefeatScreen()
    {
        defeatScreen.SetActive(true);
        player.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
        player.GetComponent<StarterAssets.StarterAssetsInputs>().SetCursorState(false);
    }

    public void startVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
    public void exitCastUI()
    {
        if (inBattle && inCastUI)
        {
            InputSystem.DisableDevice(Mouse.current);
            GameObject[] pos = GameObject.FindGameObjectsWithTag(minionPositionString);
            foreach (var item in pos)
            {
                item.GetComponent<MinionPositionScript>().enableScript = false;
                item.GetComponent<MinionPositionScript>().spotLight.SetActive(false);
            }
            bossBattlePos.GetComponent<MinionPositionScript>().enableScript = false;
            bossBattlePos.GetComponent<MinionPositionScript>().spotLight.SetActive(false);
            battleUI.SetActive(true);
            castUI.SetActive(false);
            castUI.GetComponent<Canvas>().enabled = false;
            //castCard.SetActive(false);
            globalLight.SetActive(true);
            currentCardID = 0;
            inCastUI = false;
            StartCoroutine(exitCastCamera());
        }
    }

    public void exitGuardUI()
    {
        playerBattlePos.GetComponent<MinionPositionScript>().enableScript = false;
        playerBattlePos.GetComponent<MinionPositionScript>().spotLight.SetActive(false);
        battleUI.SetActive(true);
        castUI.SetActive(false);
        castUI.GetComponent<Canvas>().enabled = false;
        //castCard.SetActive(false);
        globalLight.SetActive(true);
        currentCardID = 0;
        inGuardUI = false;
    }

    ////// Scripts to start battle here ////////
    public void onBattleStart(bool minion)
    {
        if (PlayerPrefs.GetInt("FirstTime", 0) == 0)
        {
            _tutorial.GetComponent<Tutorial>().startTutorial();  
        }
        if (minion)
        {
            battleSystems.SetActive(true);
            mainCamera.SetActive(false);
            playerLastPos = player.transform.position;
            _turn.maxMana = _shop.maxMana;
            TurnSystem.currentMana = _shop.maxMana;
            player.GetComponent<StarterAssets.ThirdPersonController>().onBattleStart(playerBattlePos.position);
            enemyToSpawn = Random.Range(1, 7);
            enemyPos1 = false;
            enemyPos2 = false;
            inBattle = true;
        }
        else
        {
            battleSystems.SetActive(true);
            mainCamera.SetActive(false);
            playerLastPos = player.transform.position;
            _turn.maxMana = _shop.maxMana;
            TurnSystem.currentMana = _shop.maxMana;
            player.GetComponent<StarterAssets.ThirdPersonController>().onBattleStart(playerBattlePos.position);
            bossHealthBar.SetActive(true);
            inBattle = true;
        }
        if (Application.platform == RuntimePlatform.Android)        //Mobile Joystick
        {
            joystickGame.SetActive(false);
        }
        resetGame();
        totalEncounter++;
        PlayerPrefs.SetInt("FirstTime", 1);
        _audio.clip = BGM[1];
        _audio.Play();
        onEndOnce = false;
    }

    public void startBattle(bool minion)
    {
        if (minion)
        {
            player.GetComponent<StarterAssets.ThirdPersonController>()._playerInput.enabled = false;
            player.GetComponent<StarterAssets.ThirdPersonController>()._controller.enabled = false;
            inBossBattle = false;
        }
        else
        {
            player.GetComponent<StarterAssets.ThirdPersonController>()._playerInput.enabled = false;
            player.GetComponent<StarterAssets.ThirdPersonController>()._controller.enabled = false;
            inBossBattle = true;
        }
    }

    public IEnumerator fadeToBlackStart(bool minion)
    {
        fadeToBlackScreen.GetComponent<Animator>().Play("Base Layer.FadeIn", 0, 0);
        yield return new WaitWhile(() => fadeToBlackScreen.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        startBattle(minion);
        yield return new WaitForSeconds(0.5f);
        onBattleStart(minion);
        fadeToBlackScreen.GetComponent<Animator>().Play("Base Layer.FadeOut", 0, 0);
    }

    public void callStartBattle(bool minion)
    {
        StartCoroutine(fadeToBlackStart(minion));
    }

    public void onBattleEnd()
    {
        battleSystems.SetActive(false);
        mainCamera.SetActive(true);
        player.GetComponent<StarterAssets.ThirdPersonController>().onBattleExit(playerLastPos);
        enemyToSpawn = 0;
        if (onFieldGoblin1 != null)
        {
            Destroy(onFieldGoblin1);
        }
        if (onFieldGoblin2 != null)
        {
            Destroy(onFieldGoblin2);
        }
        if (Application.platform == RuntimePlatform.Android)  //Mobile Joystick
        {
            joystickGame.SetActive(true);
        }
        _shop.GetComponent<ShopManager>().coins += Random.Range(1500, 2001);
        _audio.clip = BGM[0];
        _audio.Play();
        inBattle = false;
    }

    public void mouseOverAction(int pos)
    {
        GameObject[] minionPos = GameObject.FindGameObjectsWithTag(minionPositionString);
        foreach (var item in minionPos)
        {
            item.GetComponent<MinionPositionScript>().enableScript = false;
        }
        if (currentCardID == 1)
        {
            if (pos == 1)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().moveToTarget(minionBattlePos1.position, currentCardID);
                    inTransition = true;
                    globalLight.SetActive(true);
                    TurnSystem.currentMana -= 2;
                    Destroy(targetCard);
                }
                else if (pos == 2)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().moveToTarget(minionBattlePos2.position, currentCardID);
                    inTransition = true;
                    globalLight.SetActive(true);
                    TurnSystem.currentMana -= 2;
                    Destroy(targetCard);
                }
                else if (pos == 0)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().moveToTarget(bossBattlePos.position + Vector3.forward * -0.5f, currentCardID);
                    inTransition = true;
                    globalLight.SetActive(true);
                    TurnSystem.currentMana -= 2;
                    Destroy(targetCard);
                }
                attackPos = pos;
            }
        if (currentCardID == 2)
        {
                player.GetComponent<StarterAssets.ThirdPersonController>().addShield();
                TurnSystem.currentMana -= 1;
                Destroy(targetCard);
                exitGuardUI();
        }
        if (currentCardID == 3)
        {
                //Special attack
                if (pos == 1)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().moveToTarget(minionBattlePos1.position, currentCardID);
                    inTransition = true;
                    globalLight.SetActive(true);
                    TurnSystem.currentMana -= 3;
                    Destroy(targetCard);
                }
                else if (pos == 2)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().moveToTarget(minionBattlePos2.position, currentCardID);
                    inTransition = true;
                    globalLight.SetActive(true);
                    TurnSystem.currentMana -= 3;
                    Destroy(targetCard);
                }
                else if (pos == 0)
                {
                    player.GetComponent<StarterAssets.ThirdPersonController>().moveToTarget(bossBattlePos.position + Vector3.forward * -0.5f, currentCardID);
                    inTransition = true;
                    globalLight.SetActive(true);
                    TurnSystem.currentMana -= 2;
                    Destroy(targetCard);
                }
                attackPos = pos;
        }
    }

    public void showEnemyHealthBar(int pos)
    {
        if (!inBossBattle)
        {
            if (pos == 1)
            {
                minionHealthbar1.SetActive(true);
                //minionHealthbar1.GetComponent<Slider>().value = onFieldGoblin1.GetComponent<EnemyController>().health;
            }
            if (pos == 2)
            {
                minionHealthbar2.SetActive(true);
                //minionHealthbar2.GetComponent<Slider>().value = onFieldGoblin2.GetComponent<EnemyController>().health;
            }
        }
    }

    public void unshowEnemyHealthBar(int pos)
    {
        if (!inBossBattle)
        {
            if (pos == 1)
            {
                minionHealthbar1.SetActive(false);
            }
            if (pos == 2)
            {
                minionHealthbar2.SetActive(false);
            }
        }
    }

    public void changeCanCancel(bool result)
    {
        canCancel = result;
    }
    
    public void enterCastUI(int displayID, GameObject card)
    {
        if (!inBossBattle)
        {
            GameObject[] pos = GameObject.FindGameObjectsWithTag(minionPositionString);
            foreach (var item in pos)
            {
                item.GetComponent<MinionPositionScript>().enableScript = true;
            }
        }
        else
        {
            bossBattlePos.GetComponent<MinionPositionScript>().enableScript = true;
        }
        battleUI.SetActive(false);
        castUI.SetActive(true);
        castUI.GetComponent<Canvas>().enabled = true;
        //castCard.SetActive(true);
        castCard.GetComponent<CastCard>().initCastCard(card.GetComponent<DisplayCard>());
        currentCardID = displayID;
        globalLight.SetActive(false);
        canCancel = false;
        battleCamera.GetComponent<Animator>().Play(cameraCastAniString);
        targetCard = card;
        inCastUI = true;
    }

    public void enterGuardUI(int displayID, GameObject card)
    {
        playerBattlePos.GetComponent<MinionPositionScript>().enableScript = true;
        battleUI.SetActive(false);
        castUI.SetActive(true);
        castUI.GetComponent<Canvas>().enabled = true;
        //castCard.SetActive(true);
        castCard.GetComponent<CastCard>().initCastCard(card.GetComponent<DisplayCard>());
        currentCardID = displayID;
        canCancel = true;
        globalLight.SetActive(false);
        targetCard = card;
        inGuardUI = true;
    }

    void resetGame()
    {
        _deck.resetDeck();
    }

    public void showNoMana()
    {
        StartCoroutine(noMana());
    }

    public IEnumerator endBattle()
    {
        exitCastUI();
        //do something here
        yield return new WaitForSeconds(2);
        onBattleEnd();
    }

    IEnumerator exitCastCamera()
    {
        yield return new WaitUntil(() => battleCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        battleCamera.GetComponent<Animator>().Play(cameraDefaultAniString);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => battleCamera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        InputSystem.EnableDevice(Mouse.current);
    }

    IEnumerator noMana()
    {
        noManaText.SetActive(true);
        yield return new WaitForSeconds(1);
        noManaText.SetActive(false);
    }
}

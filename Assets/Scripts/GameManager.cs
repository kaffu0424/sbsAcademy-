using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float moveSpeed;
    public int exp,lv,lvMax,coin, currentStageNumber;
    public int[] expMax;
    public int[] hpMax;
    public int[] attackPower;

    public float attackPowerTime;
    float currentAttackPowerTime=0;
    int originalAttackPower;

    public GameObject pauseUI,gameOverUI,stageClearUI;
    public GameObject rewardBoxClose, rewardBoxOpen;

    public Slider expBar,hpBar;
    public TextMeshProUGUI lvText,coinText;
    public TextMeshProUGUI rewardText;
    MapGenerator mapG;
    Player player;

    public int bodyMapIndex, bodyMapSeed, bodyCoin;
    public Vector3 bodyPosition;
    public GameObject body;

    private void Awake()
    {
        instance = this;
        player = GameObject.Find("Player").GetComponent<Player>();
        player.hpMax = hpMax[lv];
        //PlayerPrefs.DeleteAll();
    }

    private void Start()
    {
        mapG = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        currentStageNumber = mapG.mapIndex;
        originalAttackPower = attackPower[lv];

        LoadBodyData();
        exp = PlayerPrefs.GetInt("EXP",0);    //저장된 경험치 값 가져 오기
        lv = PlayerPrefs.GetInt("LV",1);     //저장된 레벨 값 가져오기
        GetExp(0);
        UpdateLv();
        UpdateCoin(PlayerPrefs.GetInt("COIN",0));
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        stageClearUI.SetActive(false);
        rewardBoxClose.SetActive(false);
        rewardBoxOpen.SetActive(false);        
    }

    private void Update()
    {
        if(currentAttackPowerTime>0)
        {
            currentAttackPowerTime -= Time.deltaTime;
        }
        else
        {
            attackPower[lv] = originalAttackPower;
        }
    }

    public void SaveBody(int index,int seed, int coin, Vector3 point)
    {
        bodyMapIndex = index;
        bodyMapSeed = coin;
        bodyCoin = coin;
        bodyPosition=point;

        PlayerPrefs.SetInt("BODY_MAP_INDEX", index);
        PlayerPrefs.SetInt("BODY_MAP_SEED", seed);
        PlayerPrefs.SetInt("BODY_COIN", coin);
        PlayerPrefs.SetFloat("BODYX", point.x);
        PlayerPrefs.SetFloat("BODYY", point.y);
        PlayerPrefs.SetFloat("BODYZ", point.z);

        CheckMapBody();
    }

    void LoadBodyData()
    {
        bodyMapIndex = PlayerPrefs.GetInt("BODY_MAP_INDEX",0);
        bodyMapSeed = PlayerPrefs.GetInt("BODY_MAP_SEED", 0);
        bodyCoin = PlayerPrefs.GetInt("BODY_COIN", 0);
        bodyPosition = new Vector3(PlayerPrefs.GetFloat("BODYX", 0), PlayerPrefs.GetFloat("BODYY", 0), PlayerPrefs.GetFloat("BODYZ", 0));
    }

    public void CheckMapBody()
    {
        if (mapG.mapIndex == bodyMapIndex && mapG.maps[bodyMapIndex].seed == bodyMapSeed)
        {
            GameObject Body = Instantiate(body, bodyPosition, Quaternion.identity) as GameObject;
            Body.GetComponent<Body>().coin = GameManager.instance.coin;
        }
    }

    public void GetExp(int num)
    {
        //Debug.Log("경험치 획득 "+num);
        exp += num;
        if(exp>=expMax[lv])
        {
            exp = 0;
            lv++;
            UpdateLv();
            player.hpMax = hpMax[lv];
            player.RecoverHP();
        }
        expBar.value = (float)exp / (float)expMax[lv];
        PlayerPrefs.SetInt("EXP", exp);
    }

    public void UpdateHP(float hp, float hpMax)
    {
        hpBar.value = hp / hpMax;
    }

    public void UpdateLv()
    {
        lvText.text = lv.ToString();
        PlayerPrefs.SetInt("LV", lv);
    }

    public void UpdateCoin(int num)
    {
        coin += num;
        coinText.text = coin.ToString();
        PlayerPrefs.SetInt("COIN", coin);
    }

    public void Reset()
    {
        exp = 0;
        lv = 1;
        coin = 0;
        GetExp(0);
        UpdateLv();
        UpdateCoin(0);
        LoadBodyData();
    }

    public void OnClickPause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnClickContinue()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void OnClickRetry()
    {
        Time.timeScale = 1;
        player.Reset();
        pauseUI.SetActive(false);
        mapG.mapIndex = currentStageNumber;

    }

    public void OnClickNextStage()
    {
        Time.timeScale = 1;
        mapG.mapIndex++;
        mapG.GenerateMap();
        GameObject.Find("SpawnManager").GetComponent<SpawnManager>().Reset();
        player.ResetForNextStage();
        stageClearUI.SetActive(false);

        CheckMapBody();
    }

    public void OnClickHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("01_main");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void StageClear()
    {
        Time.timeScale = 0;
        rewardText.text = "";
        rewardBoxClose.SetActive(true);
        rewardBoxOpen.SetActive(false);
        stageClearUI.SetActive(true);
    }

    public void OnClickSkill01()
    {
        originalAttackPower = attackPower[lv];
        attackPower[lv] *= 2;
        currentAttackPowerTime = attackPowerTime;
    }

}

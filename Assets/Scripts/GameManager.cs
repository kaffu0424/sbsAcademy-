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
    public int exp,lv,lvMax,coin;
    public int[] expMax;

    public GameObject pauseUI;

    public Slider expBar,hpBar;
    public TextMeshProUGUI lvText,coinText;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        exp = PlayerPrefs.GetInt("EXP",0);    //저장된 경험치 값 가져 오기
        lv = PlayerPrefs.GetInt("LV",1);     //저장된 레벨 값 가져오기
        GetExp(0);
        UpdateLv();
        UpdateCoin(PlayerPrefs.GetInt("COIN",0));
        pauseUI.SetActive(false);
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

    public void OnClickHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("01_main");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

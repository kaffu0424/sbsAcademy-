using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardBox : MonoBehaviour
{
    public int defaultCoin = 50;
    int randomCoin;
    public GameObject closeBox, openBox;
    public TextMeshProUGUI rewardText;
    MapGenerator mapG;

    private void Start()
    {
        closeBox.SetActive(true);
        openBox.SetActive(false);
        rewardText.text = "";
        mapG = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    }

    private void OnMouseDown()
    {
        closeBox.SetActive(false);
        openBox.SetActive(true);
        
        if(mapG.mapIndex>0)
        {
            randomCoin = Random.Range(defaultCoin * (mapG.mapIndex - 1), defaultCoin * (mapG.mapIndex + 1));
        }
        else
        {
            randomCoin = Random.Range(defaultCoin * 0, defaultCoin * 1);
        }
        rewardText.text = "COIN +" + randomCoin;
        GameManager.instance.UpdateCoin(randomCoin);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Slider loadingSlider;
    public float loadingSpeed;

    private void Start()
    {
        loadingSlider.value = 0;
    }

    private void Update()
    {
        loadingSlider.value += Time.deltaTime * loadingSpeed;

        if(loadingSlider.value>=1)
        {
            SceneManager.LoadScene("01_main");
        }
    }
}

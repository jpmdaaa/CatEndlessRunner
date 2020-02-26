using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UlManager : MonoBehaviour
{

    public Image[] lifeHearts;
    public Text coinText;
    public Text scoreText;
    public Text timeText;
    public GameObject gameOverPanel;
    public GameObject timeOBJ;
    float currentTime = 0f;
    float startingTime = 3f;

    private void Start()
    {
        timeOBJ.SetActive(true);
        currentTime = startingTime;
    }
    private void Update()
    {
     
        if(currentTime>0)
        {
            currentTime -= 1 * Time.deltaTime;
        }
        else
        {
            timeOBJ.SetActive(false);
        }
        timeText.text = currentTime.ToString("0");
    }



    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if(lives>i)
            {
                lifeHearts[i].color = Color.white;
            }
            else
            {
                lifeHearts[i].color = Color.black;
            }
        }
    }

    public void UpdateCoins(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString()+ "m";
    }

}

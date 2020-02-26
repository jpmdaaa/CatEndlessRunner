using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Text[] missionDescription, missionReward, missionProgress;
    public GameObject[] rewardButton;
    public Text coinsText;
    public GameObject missionsObj;
    public GameObject buttonMissions;
    public Text costText;
    public GameObject[] characters;
    private int characterIndex;


    public void ShowAchievementsUI()
    {
        PlayServices.ShowAchivements();
    }

    public void StartRun()
    {
        if (GameManeger.gm.characterCost[characterIndex]<=GameManeger.gm.coins)
        {
            GameManeger.gm.coins -= GameManeger.gm.characterCost[characterIndex];
            GameManeger.gm.characterCost[characterIndex] = 0;
            GameManeger.gm.Save();
            GameManeger.gm.StartRun(characterIndex);
        }
     
    }
    void Start()
    {
        SetMission();
        characterIndex = 1;
        UpdateCoins(GameManeger.gm.coins);

    }

    void Update()
    {
      
    }


    public void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }

    public void SetMission()
    {
        for (int i = 0; i < 3; i++)
        {
            MissionBase mission = GameManeger.gm.GetMission(i);
            missionDescription[i].text = mission.GetMissionDescription();
            missionReward[i].text = "Recompensa: " + mission.reward;
            missionProgress[i].text = mission.progress + mission.currentProgress +" / "+ mission.max;

            if(mission.GetMissionComplete())
            {
                rewardButton[i].SetActive(true);
                
            }
        }

        GameManeger.gm.Save();

    }


    public void GetRewards(int missionIndex)
    {
        GameManeger.gm.coins += GameManeger.gm.GetMission(missionIndex).reward;
        UpdateCoins(GameManeger.gm.coins);
        rewardButton[missionIndex].SetActive(false);
        GameManeger.gm.GenereteMission(missionIndex);
    }

    public void ButtonMissionsOn()
    {
        missionsObj.SetActive(true);
        buttonMissions.SetActive(false);
    }


    public void ButtonMissionsOff()
    {
        missionsObj.SetActive(false);
        buttonMissions.SetActive(true);
    }

    public void ChangeCharacter(int index)
    {
        characterIndex += index;
        if(characterIndex>=characters.Length)
        {
            characterIndex = 0;
        }
        else if(characterIndex<0)
        {
            characterIndex = characters.Length - 1;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (i == characterIndex)
            {
                characters[i].SetActive(true);
            }
            else
                characters[i].SetActive(false);
        }

        string cost = "";

        if(GameManeger.gm.characterCost[characterIndex]!=0)
        {
            cost = GameManeger.gm.characterCost[characterIndex].ToString();

        }
        costText.text = cost;

    }


    public void ShowLeaderbordUI()
    {
        PlayServices.ShowLeaderBoard();
    }

}

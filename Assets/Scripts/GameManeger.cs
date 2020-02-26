using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Random = UnityEngine.Random;

[Serializable]
public class PlayerData
{
    public int coins;
    public int[] max;
    public int[] progress;
    public int[] currentprogress;
    public int[] reward;
    public string[] missionType;
    public int[] characterCost;
}

public class GameManeger : MonoBehaviour
{
    public static GameManeger gm;
    public int coins;
    public int[] characterCost;
    public int characterIndex;
    public MissionBase[] missions;
    private string filePath;

    private void Awake()
    {
        if(gm==null)
        {
            gm = this;
        }
        else if(gm!=this)
        {
            Destroy(gameObject);
        }
       
        DontDestroyOnLoad(gameObject);
        filePath = Application.persistentDataPath + "/playerInfo.dat";

        missions = new MissionBase[3];

        if(File.Exists(filePath))
        {
            Load();
        }
        else
        {

            for (int i = 0; i < missions.Length; i++)
            {
                GameObject newMission = new GameObject("Mission" + i);
                newMission.transform.SetParent(transform);
                MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.FishesSingleRun };
                int ramdomType = Random.RandomRange(0, missionType.Length);

                if (ramdomType == (int)MissionType.SingleRun)
                {
                    missions[i] = newMission.AddComponent<SingleERun>();

                }
                else if (ramdomType == (int)MissionType.TotalMeter)
                {

                    missions[i] = newMission.AddComponent<TotalMeters>();

                }
                else if (ramdomType == (int)MissionType.FishesSingleRun)
                {

                    missions[i] = newMission.AddComponent<FishesSingleRun>();

                }
                missions[i].Created();

            }
        }


    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);


        PlayerData data = new PlayerData();
        data.coins = coins;
        data.max = new int[3];
        data.progress = new int[3];
        data.currentprogress= new int[3];
        data.reward= new int[3];
        data.missionType= new string[3];
        data.characterCost = new int[characterCost.Length];

        for (int i = 0; i < 3; i++)
        {
            data.max[i] = missions[i].max;
            data.progress[i] = missions[i].progress;
            data.currentprogress[i] = missions[i].currentProgress;
            data.reward[i] = missions[i].reward;
            data.missionType[i] = missions[i].missionType.ToString();
        }
        for (int i = 0; i < data.characterCost.Length; i++)
        {
            data.characterCost[i] = characterCost[i];
        } 

        bf.Serialize(file, data);
        file.Close();

    }
    void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);
        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();
        coins = data.coins;

        for (int i = 0; i < 3; i++)
        {

            GameObject newMission = new GameObject("Mission" + i);
            newMission.transform.SetParent(transform);
            if (data.missionType[i] == MissionType.SingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<SingleERun>();
                missions[i].missionType = MissionType.SingleRun;
            }
            else if (data.missionType[i] == MissionType.TotalMeter.ToString()) 
            {
                missions[i] = newMission.AddComponent<TotalMeters>();
                missions[i].missionType = MissionType.TotalMeter;
            }
            else if (data.missionType[i] == MissionType.FishesSingleRun.ToString())
            {
                missions[i] = newMission.AddComponent<FishesSingleRun>();
                missions[i].missionType = MissionType.FishesSingleRun;
            }
            missions[i].max = data.max[i];
            missions[i].progress = data.progress[i];
            missions[i].currentProgress = data.currentprogress[i];
            missions[i].reward = data.reward[i];

        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartRun(int charIndex)
    {
        characterIndex = charIndex;
        SceneManager.LoadScene("Game");

    }

    public void EndRun()
    {
        SceneManager.LoadScene("Menu"); 
    }

    public MissionBase GetMission(int index)
    {
        return missions[index];
    }

    public void StartMissions()
    {
        for (int i = 0; i < 3; i++)
        {
            missions[i].RunStart();
        }
    }




    public void GenereteMission(int i)
    {
        Destroy(missions[i].gameObject);

        GameObject newMission = new GameObject("Mission" + i);
        newMission.transform.SetParent(transform);
        MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter, MissionType.FishesSingleRun };
        int ramdomType = Random.RandomRange(0, missionType.Length);

        if (ramdomType == (int)MissionType.SingleRun)
        {
            missions[i] = newMission.AddComponent<SingleERun>();

        }
        else if (ramdomType == (int)MissionType.TotalMeter)
        {

            missions[i] = newMission.AddComponent<TotalMeters>();

        }
        else if (ramdomType == (int)MissionType.FishesSingleRun)
        {

            missions[i] = newMission.AddComponent<FishesSingleRun>();

        }
        missions[i].Created();

        FindObjectOfType<Menu>().SetMission();



    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class PlayServices : MonoBehaviour
{
   
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();


        Social.localUser.Authenticate(succes => { });

    }

  
    void Update()
    {
        
    }


    public static void UnlockAchiviment(string id)
    {
        Social.ReportProgress(id, 100, success => { });

    }

    public static void IncrementeAchivement(string id, int steps)
    {
        PlayGamesPlatform.Instance.IncrementAchievement(id, steps, succes => { });
    }

    public static void ShowAchivements()
    {
        Social.ShowAchievementsUI();
    }

    public static void PostScore(long score,string leaderBoard)
    {
        Social.ReportScore(score, leaderBoard, (succes => { }));
    }


    public static void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }

    public static long GetPlayerScore(string leaderboard)
    {
        long score = 0;

        PlayGamesPlatform.Instance.LoadScores(leaderboard, LeaderboardStart.PlayerCentered, 1, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime, (LeaderboardScoreData data) => {score=data.PlayerScore.value;});

        return score;
    }

}

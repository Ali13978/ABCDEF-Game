using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsController : MonoBehaviour
{
    [SerializeField] private LeaderboardEntry[] leaderboardEntries;
    [SerializeField] private List<Leaderboardentity> entries;
    [SerializeField] private List<Button> levelBtns;
    [SerializeField] private object scoresResponse;

    private void Start()
    {

        foreach (Button i in levelBtns)
        {
            i.onClick.AddListener(() =>
            {
                string leaderboardID = "level" + (levelBtns.IndexOf(i) + 1) + "Leaderboard";

                UpdateLeaderboard(leaderboardID);
            });
        }
    }


    public async Task<string> GetScores(string LeaderboardId)
    {
        try
        {
            scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);

            string jsonData = JsonConvert.SerializeObject(scoresResponse);
            Debug.Log(jsonData);
            return jsonData;
        }
        catch (UnityException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    public async Task<string> GetPlayerScore(string LeaderboardId)
    {
        try
        {
            var scoreResponse =
                await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
            string jsonData = JsonConvert.SerializeObject(scoreResponse);
            Debug.Log(jsonData);

            return jsonData;
        }
        catch (HttpNotFoundException ex)
        {
            // Catch and handle the HttpNotFoundException
            Debug.LogError("HttpNotFoundException: " + ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            // Catch any other exceptions if needed
            Debug.LogError("Exception: " + ex.Message);
            return null;
        }
    }

    public async void UpdateLeaderboard(string leaderboardId)
    {
        string jsonData = await GetScores(leaderboardId);

        FillEntriesFromJSON(jsonData);

        for (int i = 0; i < leaderboardEntries.Length - 1; i++)
        {
            if (i < entries.Count)
            {
                leaderboardEntries[i].UpdateLeaderbordEntry("Rank# " + entries[i].rank + 1,
                    entries[i].playerName,
                    entries[i].score.ToString() + "sec");
            }

            else
            {
                leaderboardEntries[i].UpdateLeaderbordEntry("Rank# " + "Nill",
                    "Nill",
                    "Nill");
            }
        }


        string playerJsonData = await GetPlayerScore(leaderboardId);

        if (!string.IsNullOrEmpty(playerJsonData))
        {
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(playerJsonData);
            Debug.Log("PlayerScore string: " + playerJsonData);

            if (playerData != null)
            {
                leaderboardEntries[leaderboardEntries.Length - 1].UpdateLeaderbordEntry("Your Rank# " + (playerData.rank + 1),
                    playerData.playerName,
                    playerData.score + "sec");
            }
            else
            {
                leaderboardEntries[leaderboardEntries.Length - 1].UpdateLeaderbordEntry("Your Rank# " + "Nill",
                    "Nill",
                    "Nill");
            }
        }
        else
        {
            // Handle the case when the JSON data is empty or null
            leaderboardEntries[leaderboardEntries.Length - 1].UpdateLeaderbordEntry("Your Rank# " + "Nill",
                "Nill",
                "Nill");
        }
    }

    private void FillEntriesFromJSON(string jsonData)
    {
        LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(jsonData);
        entries = data.results;
    }
}


[System.Serializable]
class LeaderboardData
{
    public int limit;
    public int total;
    public List<Leaderboardentity> results;
}

[System.Serializable]
class Leaderboardentity
{
    public string playerId;
    public string playerName;
    public int rank;
    public float score;
}

[System.Serializable]
class PlayerData
{
    public string playerId;
    public string playerName;
    public int rank;
    public float score;
    public string updatedTime;
}

public class HttpNotFoundException : Exception
{
    public HttpNotFoundException() : base("HTTP/1.1 404 Not Found") { }
}

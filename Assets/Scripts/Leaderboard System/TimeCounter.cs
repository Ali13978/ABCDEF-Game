using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private int Levelnumber;

    private float _Time = 0f;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    private void Update()
    {
        _Time += Time.deltaTime;
    }

    public async void StopTimerAndSendToLeaderboard()
    {
        float finishTime = _Time;
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync("level" + Levelnumber +"Leaderboard", finishTime);
    }
}

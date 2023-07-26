using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject goalEffect;
    [SerializeField] TimeCounter timeCounter;
    [SerializeField] float timeToWin;
    float timeOnGoal = 0f;
    bool playerOnGoal = false;

    private void Update()
    {
        if (playerOnGoal)
        {
            timeOnGoal += Time.deltaTime;
            if (timeOnGoal >= timeToWin)
            {
                LoadMenuScene();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeCounter.StopTimerAndSendToLeaderboard();
            playerOnGoal = true;
            goalEffect.SetActive(true);
        }
    }


    private void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}

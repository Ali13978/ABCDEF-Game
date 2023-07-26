using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Authentication;

public class UIController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GoogleLoginAuthentication googleLoginAuthentication;

    [Header("Menu Pannel")]
    [SerializeField] private GameObject menuPannel;
    [SerializeField] private RawImage userImage;
    [SerializeField] private TMP_Text userNameText;

    [Header("Login Pannel")]
    [SerializeField] private GameObject loginPannel;
    [SerializeField] private Button loginBtn;

    [Header("Leaderboard pannel")]
    [SerializeField] private GameObject leaderboardPannel;
    [SerializeField] private Button enterLeaderboardBtm;
    [SerializeField] private Button exitLeaderboardBtm;


    private void Start()
    {
        if (googleLoginAuthentication.IsPlayerSignedin())
        {
            LoadProfile();
            EnableMenuPannel();
        }
        else
            EnableLoginPannel();

        loginBtn.onClick.AddListener(async () => {
            await googleLoginAuthentication.SignInWithGoogleAsync();
        });

        enterLeaderboardBtm.onClick.AddListener(() => {
            EnableLeaderboardPannel();
        });

        exitLeaderboardBtm.onClick.AddListener(() => {
            EnableMenuPannel();
        });

        AuthenticationService.Instance.SignedIn += () => {
            LoadProfile();
            EnableMenuPannel();
        };
    }

    private void TurnOffAlllPannels()
    {
        menuPannel.SetActive(false);
        loginPannel.SetActive(false);
        leaderboardPannel.SetActive(false);
    }

    private void EnableMenuPannel()
    {
        TurnOffAlllPannels();

        menuPannel.SetActive(true);
    }

    private async void LoadProfile()
    {
        string ptofilePicurl = PlayerPrefs.GetString("ProfilePicURL");
        string userName = PlayerPrefs.GetString("ProfileUserName");

        userNameText.text = "Name: " + userName;

        if (ptofilePicurl == null)
            return;

        userImage.texture = await GetRemoteTexture(ptofilePicurl);
    }

    private void EnableLeaderboardPannel()
    {
        TurnOffAlllPannels();
        leaderboardPannel.SetActive(true);
    }

    private void EnableLoginPannel()
    {
        TurnOffAlllPannels();
        loginPannel.SetActive(true);
    }


    #region GetImageFromURL
    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            //if (www.isNetworkError || www.isHttpError)
            if (www.result != UnityWebRequest.Result.Success)// for Unity >= 2020.1
            {
                // log error:
#if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
#endif

                // nothing to return on error:
                return null;
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }
    #endregion

}

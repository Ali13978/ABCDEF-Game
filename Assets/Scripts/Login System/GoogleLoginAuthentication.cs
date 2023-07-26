using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Izhguzin.GoogleIdentity;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using RequestFailedException = Izhguzin.GoogleIdentity.RequestFailedException;

public class GoogleLoginAuthentication : MonoBehaviour
{
    private TokenResponse _googleTokenResponse;
    private UserCredential userCredential;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    private async void Start()
    {
        await InitServicesAsync();
    }

    private async Task InitServicesAsync()
    {
        GoogleAuthOptions.Builder optionsBuilder = new GoogleAuthOptions.Builder ();
        optionsBuilder.SetCredentials("784627625314-kp3um1pjg3ai9meqb6voagq26vvcfkhs.apps.googleusercontent.com", "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")
            .SetScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile)
            .SetListeningTcpPorts(new[] { 5000, 5001, 5002, 5003, 5005 });

        try
        {
            await GoogleIdentityService.InitializeAsync(optionsBuilder.Build());
        }
        catch (AuthorizationFailedException exception)
        {
            Debug.LogException(exception);
        }
        catch (ServicesInitializationException)
        {
            Debug.LogError("Critical error, unable to log in anonymously.");
            throw;
        }
    }

    private async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException exception) when (exception.ErrorCode ==
                                                        AuthenticationErrorCodes.InvalidSessionToken)
        {
            await SignInAnonymouslyAsync();
        }
    }

    public bool IsPlayerSignedin()
    {
        return AuthenticationService.Instance.IsSignedIn;
    }

    public async Task SignInWithGoogleAsync()
    {
        if (IsPlayerSignedin()) return;

        try
        {
            _googleTokenResponse = await GoogleIdentityService.Instance.AuthorizeAsync();
            userCredential = _googleTokenResponse.GetUserCredential();

            string PlayerName = userCredential.Name;
            string profilePictureurl = userCredential.PictureUrl;

            string playerName = userCredential.Name.Replace(' ', '_');

            PlayerPrefs.SetString("ProfilePicURL", profilePictureurl);
            PlayerPrefs.SetString("ProfileUserName", PlayerName);

            await AuthenticationService.Instance.SignInWithGoogleAsync(_googleTokenResponse.IdToken); 
            
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }
        // Google Identity Exception
        catch (AuthorizationFailedException exception)
        {
            Debug.LogException(exception);
        }
        // Unity Authentication Exception
        catch (AuthenticationException exception) when (exception.ErrorCode ==
                                                        AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            AuthenticationService.Instance.SignOut(true);
            await AuthenticationService.Instance.SignInWithGoogleAsync(_googleTokenResponse.IdToken);
        }
    }
}
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }
    
    [SerializeField] private InputField emailField;
    [SerializeField] private InputField passwordField;
    [SerializeField] private Button signInButton;

    public static FirebaseApp FirebaseApp;
    public static FirebaseAuth FirebaseAuth;
    public static AuthResult User;

    public void Start()
    {
        signInButton.interactable = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;

            if (result != DependencyStatus.Available)
            {
                Debug.LogError(result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;

                FirebaseApp = FirebaseApp.DefaultInstance;
                FirebaseAuth = FirebaseAuth.DefaultInstance;
            }

            signInButton.interactable = IsFirebaseReady;
        }
            );
    }

    public void SignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
            return;

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        FirebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
        {
            Debug.Log($"Sign in status : {task.Status}");

            IsSignInOnProgress = false;
            signInButton.interactable = true;

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Sign-in canceled");
            }
            else
            {
                User = task.Result;
                Debug.Log("login");
                SceneManager.LoadScene("Lobby");
            }
        }
            );
    }

}

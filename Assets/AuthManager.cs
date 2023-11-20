using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }
    
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button signInButton;

    public static FirebaseApp _firebaseApp;
    public static FirebaseAuth _firebaseAuth;
    public static FirebaseUser _user;

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

                _firebaseApp = FirebaseApp.DefaultInstance;
                _firebaseAuth = FirebaseAuth.DefaultInstance;
            }

            signInButton.interactable = IsFirebaseReady;
        }
            );
    }

    public void SignIn()
    {
        if (!IsFirebaseReady || IsSignInOnProgress || _user != null)
            return;

        IsSignInOnProgress = true;
        signInButton.interactable = false;

        _firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
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
                _user = task.Result.User;
                Debug.Log(_user.Email);
                SceneManager.LoadScene("Lobby");
            }
        }
            );
    }

}

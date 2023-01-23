using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class MenuAndSave : MonoBehaviour
{
    public TextMeshProUGUI status;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button playButton;

    FirebaseAuth auth;


    //string name;
    // Start is called before the first frame update
    void Start()
    {
        playButton.interactable = false;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;
        });
    }

    //void NameChanged()
    //{
    //    PlayerPrefs.SetString("PlayerName", nameInput.text);
    //}


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignInButton()
    {
        SignIn(emailInput.text, passwordInput.text);
    }

    public void SignOutButton()
    {
        SignOut();
    }

    public void RegisterButton()
    {
        RegisterNewUser(emailInput.text, passwordInput.text);
    }

    public void SignInUser1()
    {
        string user1 = "User1@test.user";
        string password = "12345Abcde";
        SignIn(user1, password);
    }

    public void SignInUser2()
    {
        string user1 = "User2@test.user";
        string password = "12345Abcde";
        SignIn(user1, password);
    }

    private void RegisterNewUser(string email, string password)
    {
        Debug.Log("Starting Registration");
        status.text = "Starting Registration";
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User Registerd: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                status.text = "User Registerd: " +
                  newUser.DisplayName;
            }
        });
    }

    private void SignIn(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);

                status.text = "User signed in successfully: " +
                  newUser.DisplayName;

                playButton.interactable = true;
            }
        });
    }

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out");
        status.text = "User signed out";

        playButton.interactable = false;
    }



}

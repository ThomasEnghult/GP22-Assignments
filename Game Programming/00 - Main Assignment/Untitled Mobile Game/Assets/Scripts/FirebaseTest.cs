using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;

[Serializable]
public class SavePosition
{
    public Vector3 position;
    public string name;
}

public class FirebaseTest : MonoBehaviour
{
    FirebaseAuth auth;
    TMP_InputField inputField;
    SavePosition savePosition;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;

            if(auth.CurrentUser == null)
                AnonymousSignIn();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            savePosition.position = transform.position;
            string jsonString = JsonUtility.ToJson(savePosition);
            DataTest(auth.CurrentUser.UserId, jsonString);
        }

        if (Input.GetKeyDown(KeyCode.D))
            DataTest(auth.CurrentUser.UserId, UnityEngine.Random.Range(0, 100).ToString());

        if (Input.GetKeyDown(KeyCode.L))
            LoadFromFirebase();

        if (Input.GetKeyDown(KeyCode.S))
            SaveToFirebase(inputField.text);
    }

    private void AnonymousSignIn()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            }
        });
    }

    private void DataTest(string userID, string data)
    {
        Debug.Log("Trying to write data...");
        var database = FirebaseDatabase.DefaultInstance;
        database.RootReference.Child("users").Child(userID).SetRawJsonValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
            else
                Debug.Log("DataTestWrite: Complete");
        });
    }

    private void SaveToFirebase(string data)
    {
        var db = FirebaseDatabase.DefaultInstance;
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //puts the json data in the "users/userId" part of the database.
        db.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(data);
    }

    private void LoadFromFirebase()
    {
        var database = FirebaseDatabase.DefaultInstance;
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        database.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }

            //here we get the result from our database.
            DataSnapshot snap = task.Result;

            //And send the json data to a function that can update our game.
            //LoadState(snap.GetRawJsonValue());
            Debug.Log(snap.GetRawJsonValue());

            savePosition = JsonUtility.FromJson<SavePosition>(snap.GetRawJsonValue());
            transform.position = savePosition.position;
        });
    }

}
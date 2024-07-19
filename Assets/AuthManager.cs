using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI messageText;

    void Start()
    {
        Debug.Log("Starting AuthManager...");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void RegisterUser()
    {
        Debug.Log("RegisterUser method called.");
        if (emailInputField == null || passwordInputField == null || messageText == null)
        {
            Debug.LogError("One or more InputFields/Text is not set in the Inspector.");
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("RegisterUser encountered an error: " + task.Exception);
                messageText.text = "Registration failed.";
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            messageText.text = "Registration successful!";
        });
    }

    public void LoginUser()
    {
        Debug.Log("LoginUser method called.");
        if (emailInputField == null || passwordInputField == null || messageText == null)
        {
            Debug.LogError("One or more InputFields/Text is not set in the Inspector.");
            return;
        }

        string email = emailInputField.text;
        string password = passwordInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("LoginUser encountered an error: " + task.Exception);
                messageText.text = "Login failed.";
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            messageText.text = "Login successful!";
            SceneManager.LoadScene("MainMenu");
        });
    }
}

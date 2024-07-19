using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    private FirebaseAuth auth;

    public TextMeshProUGUI userEmailText;
    public Button profileButton; // Button used as profile picture
    public GameObject dropdownMenu; // Dropdown menu with logout and change profile buttons
    public Button logoutButton;
    public Button signUpButton; // Sign up button
    public Sprite defaultAvatar;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null)
        {
            DisplayUserInfo();
            SetButtonsState(true);
        }
        else
        {
            SetDefaultUserInfo();
            SetButtonsState(false);
        }

        profileButton.onClick.AddListener(ToggleDropdownMenu);
        logoutButton.onClick.AddListener(Logout);
        signUpButton.onClick.AddListener(SignUp);
    }

    public void DisplayUserInfo()
    {
        if (auth.CurrentUser != null)
        {
            string userEmail = auth.CurrentUser.Email;
            userEmailText.text = GetTruncatedEmail(userEmail);
        }
    }

    private string GetTruncatedEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return "Guest";
        }

        if (email.Length > 7)
        {
            return email.Substring(0, 7) + "...";
        }

        return email;
    }

    private void SetDefaultUserInfo()
    {
        userEmailText.text = "Guest"; // Display "Guest" or any default text
        profileButton.image.sprite = defaultAvatar; // Set default avatar image
    }

    private void ToggleDropdownMenu()
    {
        dropdownMenu.SetActive(!dropdownMenu.activeSelf);
    }

    private void Logout()
    {
        auth.SignOut();
        ClearUserData();
    }

    private void ClearUserData()
    {
        // Clear user-related UI elements
        SetDefaultUserInfo();
        SetButtonsState(false);

        // Redirect to login screen
     
    }

    private void SignUp()
    {
        // Redirect to sign-up screen
        SceneManager.LoadScene("UserRegistration"); // Ensure you have a scene named "UserRegistration"
    }

    private void SetButtonsState(bool isLoggedIn)
    {
        logoutButton.gameObject.SetActive(isLoggedIn);
        signUpButton.gameObject.SetActive(!isLoggedIn);
    }
}

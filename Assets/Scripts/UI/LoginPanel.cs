using Appointix.ApplicationLayer;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
	[Header("User Input Fields")]
	[SerializeField] private TMP_InputField emailField;
	[SerializeField] private TMP_InputField passwordField;
	[SerializeField] private TMP_Dropdown userTypeSelection;

	[Header("Error Displays")]
	[SerializeField] private TextMeshProUGUI emailErrorDisplay;
	[SerializeField] private TextMeshProUGUI passwordErrorDisplay;

	[Header("Other Objs")]
	[SerializeField] private GameObject registerPanel;

	public void AttemptLogin()
	{
		bool shouldAttempt = true;
		string email = emailField.text.Trim();
		string password = passwordField.text.Trim();

		emailErrorDisplay.text = "";
		passwordErrorDisplay.text = "";

		if(string.IsNullOrEmpty(email))
		{
			EmailInvalid("Campo email vuoto");
			shouldAttempt = false;
		}

		if (string.IsNullOrEmpty(password))
		{
			PasswordInvalid("Campo password vuoto");
			shouldAttempt = false;
		}

		if (!shouldAttempt) return;

		// AppContext attempt to login
		// AppContext needs to tell us two things:
		// 1. Is the inserted email registered to an account?
		// 2. Is the inserted password correct for the inserted email?
		// If both are true, login was a success and we should show the correct display (doctor or patient view)
	}

	private void EmailInvalid(string msg)
	{
		Debug.Log(msg);
		emailErrorDisplay.text = msg;
	}

	private void PasswordInvalid(string msg)
	{
		Debug.Log(msg);
		passwordErrorDisplay.text = msg;
	}

	private void OnLoginSuccess()
	{
		// straight up change scene, reduce load by having only the correct UI for either doctor or patient
	}

	private void OnLoginFailure()
	{
		// kill the user with a thousand hammers
	}

	public void SwitchToRegisterPanel()
	{
		registerPanel.SetActive(true);
		this.gameObject.SetActive(false);
	}
}

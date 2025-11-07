using Appointix.ApplicationLayer;
using TMPro;
using UnityEngine;

public class RegisterPanel : MonoBehaviour
{
	[Header("User Input Fields")]
	[SerializeField] private TMP_InputField nameField;
	[SerializeField] private TMP_InputField surnameField;
	[SerializeField] private TMP_InputField phoneField;
	[SerializeField] private TMP_InputField emailField;
	[SerializeField] private TMP_InputField passwordField;

	[Header("Error Displays")]
	[SerializeField] private TextMeshProUGUI nameErrorDisplay;
	[SerializeField] private TextMeshProUGUI surnameErrorDisplay;
	[SerializeField] private TextMeshProUGUI phoneErrorDisplay;
	[SerializeField] private TextMeshProUGUI emailErrorDisplay;
	[SerializeField] private TextMeshProUGUI passwordErrorDisplay;

	[Header("Other Objs")]
	[SerializeField] private GameObject loginPanel;

	private void Start()
	{
		ClearAllErrorDisplays();
	}

	public void AttemptRegister()
	{
		ClearAllErrorDisplays();
		string name = nameField.text.Trim();
		string surname = surnameField.text.Trim();
		string phone = phoneField.text.Trim();
		string email = emailField.text.Trim();
		string password = passwordField.text.Trim();

		bool shouldAttempt = true;

		if(string.IsNullOrEmpty(name))
		{
			FieldError("Campo Nome Vuoto", nameErrorDisplay);
			shouldAttempt = false;
		}

		if (string.IsNullOrEmpty(surname))
		{
			FieldError("Campo Cognome Vuoto", surnameErrorDisplay);
			shouldAttempt = false;
		}

		if (string.IsNullOrEmpty(phone))
		{
			FieldError("Campo Telefono Vuoto", phoneErrorDisplay);
			shouldAttempt = false;
		}

		if (string.IsNullOrEmpty(email))
		{
			FieldError("Campo Email Vuoto", emailErrorDisplay);
			shouldAttempt = false;
		}

		if(string.IsNullOrEmpty(password))
		{
			FieldError("Campo Password Vuoto", passwordErrorDisplay);
			shouldAttempt = false;
		}

		// we alraedy checked that there's no point sending this data to the endpoint, and we alraedy populated all error field
		if (!shouldAttempt) return;

		// 1. try to find the email in the DB to prevent a new account with same email
		// 2.
		AppContext.Instance.RepositoryManager.CreatePatient(name, surname, email, password, phone);
	}

	private void ClearAllErrorDisplays()
	{
		FieldError("", nameErrorDisplay);
		FieldError("", surnameErrorDisplay);
		FieldError("", phoneErrorDisplay);
		FieldError("", emailErrorDisplay);
		FieldError("", passwordErrorDisplay);
	}

	private void FieldError(string msg, TextMeshProUGUI errorDisplay)
	{
		errorDisplay.text = msg;
	}

	public void SwitchToLoginPanel()
	{
		loginPanel.SetActive(true);
		gameObject.SetActive(false);
	}

}

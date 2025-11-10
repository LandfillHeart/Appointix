using System;
using Appointix.ApplicationLayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour
{
    [Header("User Input Fields")]
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField surnameField;
    [SerializeField] private TMP_InputField phoneField;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;

    [Header("Extra Fields (solo per medico)")]
    [SerializeField] private TMP_InputField specializzazioneField;
    [SerializeField] private TMP_InputField cittaField;
	[SerializeField] private TMP_InputField durataVisitaField;
	[SerializeField] private TextMeshProUGUI specializzazioneText;
	[SerializeField] private TextMeshProUGUI cittaText;
	[SerializeField] private TextMeshProUGUI durataVisitaText;


    [Header("Error Displays")]
    [SerializeField] private TextMeshProUGUI nameErrorDisplay;
    [SerializeField] private TextMeshProUGUI surnameErrorDisplay;
    [SerializeField] private TextMeshProUGUI phoneErrorDisplay;
    [SerializeField] private TextMeshProUGUI emailErrorDisplay;
    [SerializeField] private TextMeshProUGUI passwordErrorDisplay;

    [Header("Other Objs")]
    [SerializeField] private GameObject loginPanel;
	[SerializeField] private TMP_Dropdown roleDropdown; 
	
    private void Start()
    {
        ClearAllErrorDisplays();
        roleDropdown.onValueChanged.AddListener(OnRoleChanged);
        OnRoleChanged(roleDropdown.value);
    }

    private void OnRoleChanged(int value)
    {
        bool isMedico = roleDropdown.options[value].text == "MEDICO";

        specializzazioneField.gameObject.SetActive(isMedico);
        cittaField.gameObject.SetActive(isMedico);
		durataVisitaField.gameObject.SetActive(isMedico);
		specializzazioneText.gameObject.SetActive(isMedico);
		cittaText.gameObject.SetActive(isMedico);
		durataVisitaText.gameObject.SetActive(isMedico);
    }

    public void AttemptRegister()
    {
        ClearAllErrorDisplays();

        string name = nameField.text.Trim();
        string surname = surnameField.text.Trim();
        string phone = phoneField.text.Trim();
        string email = emailField.text.Trim();
        string password = passwordField.text.Trim();
		string specializzazione = specializzazioneField.text.Trim();
		string citta = cittaField.text.Trim();
		string durataVisita = durataVisitaField.text.Trim();

        bool shouldAttempt = true;

        if (string.IsNullOrEmpty(name))
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

		if (string.IsNullOrEmpty(password))
		{
			FieldError("Campo Password Vuoto", passwordErrorDisplay);
			shouldAttempt = false;
		}
		if (roleDropdown.options[roleDropdown.value].text == "MEDICO")
		{
			if (string.IsNullOrEmpty(specializzazione))
			{
				FieldError("Campo Specializzazione Vuoto", specializzazioneText);
				shouldAttempt = false;
			}
			if (string.IsNullOrEmpty(citta))
			{
				FieldError("Campo Citta Vuoto", cittaText);
				shouldAttempt = false;
			}
			if (string.IsNullOrEmpty(durataVisita))
			{
				FieldError("Campo Durata Visita Vuoto", durataVisitaText);
				shouldAttempt = false;
			}
			if (!shouldAttempt) return;
            Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager.CreateDoctor(name, surname, specializzazione, email, password, phone, citta, int.Parse(durataVisita),"Lun,Mar,Mer,Gio,Ven",TimeSpan.Zero,TimeSpan.Zero);
		}
        else
        {
            if (!shouldAttempt) return;
            Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager.CreatePatient(name, surname, email, password, phone);
    
        }

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

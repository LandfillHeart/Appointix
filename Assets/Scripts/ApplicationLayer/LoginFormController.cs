using Appointix.ApplicationLayer; // Per AppContext
using Appointix.Domain; // Per Patient/Doctor
using TMPro; // Per TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginFormController : MonoBehaviour
{
    [Header("UI Fields")]
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_Dropdown roleDropdown; // Nell'immagine, questo mostra "MEDICO"
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Panels")]
    [SerializeField] private GameObject loginPanel; // Questo pannello (per nasconderlo)
    [SerializeField] private GameObject doctorDashboardPanel; // Il pannello da mostrare per il dottore
    [SerializeField] private GameObject patientDashboardPanel; // Il pannello da mostrare per il paziente

    private void Start()
    {
        // Iscriviti al bottone
        loginButton.onClick.AddListener(AttemptLogin);

        // Iscriviti agli eventi di AppContext
        AppContext.Instance.OnPatientLoginSuccess += HandlePatientLogin;
        AppContext.Instance.OnDoctorLoginSuccess += HandleDoctorLogin;
        AppContext.Instance.OnLoginFailed += HandleLoginError;

        // Pulisci eventuali messaggi di errore precedenti
        errorText.text = "";
    }

    private void AttemptLogin()
    {
        string email = emailField.text;
        string password = passwordField.text;

        // Leggi il ruolo dal dropdown
        // Assumiamo che "MEDICO" sia opzione 0 e "PAZIENTE" sia opzione 1
        string selectedText = roleDropdown.options[roleDropdown.value].text;
        string ruolo = (selectedText == "MEDICO") ? "D" : "P";

        // Validazione semplice
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorText.text = "Email e Password non possono essere vuoti.";
            return;
        }

        // Disabilita il bottone e mostra un messaggio di attesa
        loginButton.interactable = false;
        errorText.text = "Login in corso...";

        // Avvia il tentativo di login tramite AppContext
        AppContext.Instance.TryLogin(email, password, ruolo);
    }

    private void HandlePatientLogin(Patient patient)
    {
        SceneManager.LoadScene("PatientScene");
        Debug.Log($"Login UI: Paziente {patient.nome} loggato!");
        errorText.text = ""; // Pulisci errore

        // Nascondi login e mostra pannello paziente
        loginPanel.SetActive(false);
        //patientDashboardPanel.SetActive(true);

        // Riattiva il bottone nel caso l'utente torni indietro
        loginButton.interactable = true;
    }
    

    private void HandleDoctorLogin(Doctor doctor)
    {
        Debug.Log($"Login UI: Dottore {doctor.nome} loggato!");
        errorText.text = ""; // Pulisci errore

        // Nascondi login e mostra pannello dottore
        loginPanel.SetActive(false);
        doctorDashboardPanel.SetActive(true);
        
        loginButton.interactable = true;
    }

    private void HandleLoginError(string error)
    {
        Debug.LogWarning($"Login UI: Fallito - {error}");
        
        // Mostra l'errore all'utente
        errorText.text = $"Login fallito. {error}";
        loginButton.interactable = true; // Riattiva il bottone
    }

    private void OnDestroy()
    {
        // Disiscriviti sempre dagli eventi statici!
        if (AppContext.Instance != null)
        {
            AppContext.Instance.OnPatientLoginSuccess -= HandlePatientLogin;
            AppContext.Instance.OnDoctorLoginSuccess -= HandleDoctorLogin;
            AppContext.Instance.OnLoginFailed -= HandleLoginError;
        }
    }
}
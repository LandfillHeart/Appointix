using System;
using UnityEngine;
using Appointix.Domain.Interfaces; // Assicurati di importare le tue interfacce
using Appointix.Domain;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement; // Assicurati di importare i tuoi modelli (Patient, Doctor)

namespace Appointix.ApplicationLayer
{       
    public class AppContext : MonoBehaviour
    {
        public int userID;
        #region Singleton
        private static AppContext instance;
        public static AppContext Instance => instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Aggiunto per persistenza
                return;
            }
            Destroy(gameObject);
        }
        #endregion

        [SerializeField] public TextAsset patientsJson;

        #region Events (per la UI)
        /// <summary>
        /// Evento pubblico a cui la UI si iscrive.
        /// Notifica che il Repository Manager (Reale o Mock) è pronto.
        /// </summary>
        public event Action<IRepositoryManager> OnRepositoryManagerSet;

        /// <summary>
        /// Evento pubblico. Notifica la UI di un login Paziente riuscito.
        /// </summary>
        public event Action<Patient> OnPatientLoginSuccess;

        /// <summary>
        /// Evento pubblico. Notifica la UI di un login Dottore riuscito.
        /// </summary>
        public event Action<Doctor> OnDoctorLoginSuccess;

        /// <summary>
        /// Evento pubblico. Notifica la UI di un login fallito.
        /// </summary>
        public event Action<string> OnLoginFailed;
        #endregion

        /// <summary>
        /// Il Repository (Reale o Mock) attualmente in uso dall'applicazione.
        /// </summary>
        public IRepositoryManager RepositoryManager { get; private set; }

        #region Stato Utente Loggato
        /// <summary>
        /// Contiene i dati del Paziente loggato, se il ruolo è 'P'.
        /// </summary>
        public Patient LoggedInPatient { get; private set; }

        /// <summary>
        /// Contiene i dati del Dottore loggato, se il ruolo è 'D'.
        /// </summary>
        public Doctor LoggedInDoctor { get; private set; }

        /// <summary>
        /// Il ruolo ('P' o 'D') dell'utente attualmente loggato.
        /// </summary>
        public string LoggedInRole { get; private set; }
        #endregion

        /// <summary>
        /// Tenta di avviare la connessione al database in modo asincrono.
        /// Il risultato della connessione arriverà al metodo HandleConnectionTestResult.
        /// </summary>
        /// <returns>True se il tentativo è stato avviato, False se è impossibile avviarlo.</returns>
        public bool TryConnectionToDB()
        {
            Debug.Log("AppContext: Attempting DB connection test...");

            if (EndpointConnectionManager.Instance == null)
            {
                Debug.LogError("EndpointConnectionManager non trovato nella scena. Impossibile avviare il test.");
                return false;
            }

            // Avviamo la coroutine di test sul Manager e le passiamo
            // il nostro metodo "HandleConnectionTestResult" come callback.
            StartCoroutine(
            EndpointConnectionManager.Instance.TestConnection_DB(HandleConnectionTestResult)
            );

            // Restituisce 'true' per indicare che il test è stato avviato.
            return true;
    }

        /// <summary>
        /// Questo metodo viene invocato dal Connection Manager quando il test è finito.
        /// È qui che il RepositoryManager viene impostato.
        /// </summary>
        /// <param name="success">True se la connessione ha avuto successo, false altrimenti.</param>
        private void HandleConnectionTestResult(bool success)
     {
            if (success)
            {
                // CONNESSO! Usa il vero Repository che parla con l'API.
                Debug.Log("AppContext: Connection successful. Using Database Repository (EndpointConnectionManager).");
                RepositoryManager = EndpointConnectionManager.Instance;
            }
            else
            {
                // NON CONNESSO! Usa il finto Repository in memoria (Mock).
                Debug.LogWarning("AppContext: Connection failed. Using In-Memory (Mock) Repository.");

                RepositoryManager = InMemoryRepositoryManager.Instance; 
            }

            // --- Iscrizione agli eventi del Repository scelto ---
            // Indipendentemente da quale Repository è stato scelto, ci iscriviamo
            // ai suoi eventi di login per "intercettarli".
            if (RepositoryManager != null)
            {
                RepositoryManager.OnPatientLoginSuccess += HandlePatientLogin;
                RepositoryManager.OnDoctorLoginSuccess += HandleDoctorLogin;
                RepositoryManager.OnLoginFailed += HandleLoginFailed;
            }
            // ---------------------------------------------------

            // Notifica al resto dell'applicazione (es. SelectRepoOnStartup) 
            // che un Repository è stato impostato
            OnRepositoryManagerSet?.Invoke(RepositoryManager);
        }

        /// <summary>
        /// Metodo pubblico chiamato dalla UI (LoginFormController) per tentare il login.
        /// </summary>
        public void TryLogin(string email, string password, string ruolo)
        {
            if (RepositoryManager == null)
            {
                Debug.LogError("RepositoryManager non ancora impostato.");
                OnLoginFailed?.Invoke("Sistema non pronto. Riprova.");
                return;
            }
            
            // Pulisci i dati vecchi prima di un nuovo tentativo
            LoggedInPatient = null;
            LoggedInDoctor = null;
            LoggedInRole = "";

            // Inoltra la richiesta al Repository (Reale o Mock)
            RepositoryManager.Login(email, password, ruolo);
        }


        #region Handler Eventi Repository
        // Questi metodi "ascoltano" il Repository e "inoltrano"
        // l'evento alla UI (tramite gli eventi pubblici di AppContext).

        private void HandlePatientLogin(Patient patient)
        {
            SceneManager.LoadScene("PatientScene");
            Debug.Log($"AppContext: Paziente {patient.nome} loggato.");
            LoggedInPatient = patient;
            LoggedInRole = "P";
            userID = patient.id;

            // Notifica la UI
            OnPatientLoginSuccess?.Invoke(patient);
        }

        private void HandleDoctorLogin(Doctor doctor)
        {
            SceneManager.LoadScene("DoctorScene");
            Debug.Log($"AppContext: Dottore {doctor.nome} loggato.");
            LoggedInDoctor = doctor;
            LoggedInRole = "D";
            userID = doctor.id;

            // Notifica la UI
            OnDoctorLoginSuccess?.Invoke(doctor);
        }

        private void HandleLoginFailed(string error)
        {
            Debug.LogWarning($"AppContext: Login fallito: {error}");
            
            // Notifica la UI
            OnLoginFailed?.Invoke(error);
        }
        #endregion

        /// <summary>
        /// È buona norma disiscriversi dagli eventi quando l'oggetto viene distrutto
        /// per evitare "memory leak".
        /// </summary>
        private void OnDestroy()
        {
            if (RepositoryManager != null)
            {
                RepositoryManager.OnPatientLoginSuccess -= HandlePatientLogin;
                RepositoryManager.OnDoctorLoginSuccess -= HandleDoctorLogin;
                RepositoryManager.OnLoginFailed -= HandleLoginFailed;
            }
        }
    }
}
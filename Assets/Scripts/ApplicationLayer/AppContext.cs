using System;
using UnityEngine;
// Assicurati che le interfacce e le altre classi siano importate
// using Appointix.Domain.Interfaces; 

namespace Appointix.ApplicationLayer
{
    public class AppContext : MonoBehaviour
    {
        public Patient LoggedInPatient { get; private set; }
        public Doctor LoggedInDoctor { get; private set; }
        public string LoggedInRole { get; private set; }

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

        #region Events
        public event Action<IRepositoryManager> OnRepositoryManagerSet;
        public event Action<Patient> OnPatientLoginSuccess;
        public event Action<Doctor> OnDoctorLoginSuccess;
        public event Action<string> OnLoginFailed;
        #endregion

        public IRepositoryManager RepositoryManager { get; private set; }
		public bool connectedToDB;

		public int userID;

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
            // NON significa che la connessione sia riuscita.
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

                // Assegna semplicemente l'istanza.
                // Il caricamento avverrà "pigramente" (lazy) alla prima chiamata CRUD.
                RepositoryManager = InMemoryRepositoryManager.Instance;
            }
            RepositoryManager.OnPatientLoginSuccess += HandlePatientLogin;
            RepositoryManager.OnDoctorLoginSuccess += HandleDoctorLogin;
            RepositoryManager.OnLoginFailed += HandleLoginFailed;

<<<<<<< Updated upstream
			connectedToDB = success;

			// Notifica al resto dell'applicazione che un Repository è stato impostato
			OnRepositoryManagerSet?.Invoke(RepositoryManager);
		}

		public void Login(string email, string password, string role)
		{
			
		}
=======
            // Notifica al resto dell'applicazione che un Repository è stato impostato
            OnRepositoryManagerSet?.Invoke(RepositoryManager);
        }

        /// <summary>
        /// Tenta di effettuare il login tramite il RepositoryManager.
        /// </summary>
        public void TryLogin(string email, string password, string ruolo)
        {
            if (RepositoryManager == null)
            {
                Debug.LogError("RepositoryManager non ancora impostato.");
                OnLoginFailed?.Invoke("Sistema non pronto. Riprova.");
                return;
            }

            LoggedInPatient = null;
            LoggedInDoctor = null;
            LoggedInRole = "";

            RepositoryManager.Login(email, password, ruolo);
        }
        
        private void HandlePatientLogin(Patient patient)
        {
            Debug.Log($"AppContext: Paziente {patient.nome} loggato.");
            LoggedInPatient = patient;
            LoggedInRole = "P";
            OnPatientLoginSuccess?.Invoke(patient);
        }

        private void HandleDoctorLogin(Doctor doctor)
        {
            Debug.Log($"AppContext: Dottore {doctor.nome} loggato.");
            LoggedInDoctor = doctor;
            LoggedInRole = "D";
            OnDoctorLoginSuccess?.Invoke(doctor);
        }

        private void HandleLoginFailed(string error)
        {
            Debug.LogWarning($"AppContext: Login fallito: {error}");
            OnLoginFailed?.Invoke(error);
        }

        private void OnDestroy()
        {
            if (RepositoryManager != null)
            {
                RepositoryManager.OnPatientLoginSuccess -= HandlePatientLogin;
                RepositoryManager.OnDoctorLoginSuccess -= HandleDoctorLogin;
                RepositoryManager.OnLoginFailed -= HandleLoginFailed;
            }
        }
>>>>>>> Stashed changes
    }
}
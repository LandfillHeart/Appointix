using Appointix.Domain;
using Appointix.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Appointix.ApplicationLayer
{
    /// <summary>
    /// Gestore Singleton che comunica con l'endpoint API per interagire con il database.
    /// Implementa IRepositoryManager per consentire l'astrazione dei dati (es. DB vs Mock).
    /// </summary>
    public class EndpointConnectionManager : MonoBehaviour, IRepositoryManager
    {
        #region Singleton
        /// <summary>
        /// L'istanza statica privata del Singleton.
        /// </summary>
        private static EndpointConnectionManager instance;
        /// <summary>
        /// Ottiene l'istanza pubblica (Singleton) del manager.
        /// </summary>
        public static EndpointConnectionManager Instance => instance;

        /// <summary>
        /// Metodo Awake di Unity per inizializzare il pattern Singleton.
        /// Assicura che esista una sola istanza di questo manager.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Aggiunto per persistenza tra scene
                return;
            }
            Destroy(gameObject);
        }
        #endregion

        #region IRepositoryManager - Events
        /// <summary>
        /// Evento invocato dopo il caricamento di una lista di pazienti.
        /// </summary>
        public event Action<List<Patient>> OnPatientsLoaded;
        /// <summary>
        /// Evento invocato dopo il caricamento di una lista di dottori.
        /// </summary>
        public event Action<List<Doctor>> OnDoctorsLoaded;
        /// <summary>
        /// Evento invocato dopo il caricamento di una lista di appuntamenti.
        /// </summary>
        public event Action<List<Appointment>> OnAppointmentsLoaded;

        /// <summary>
        /// Evento invocato dopo la creazione di un nuovo paziente sul database.
        /// </summary>
        public event Action<Patient> OnPatientCreated;
        /// <summary>
        /// Evento invocato dopo la creazione di un nuovo dottore sul database.
        /// </summary>
        public event Action<Doctor> OnDoctorCreated;
        /// <summary>
        /// Evento invocato dopo la creazione di un nuovo appuntamento sul database.
        /// </summary>
        public event Action<Appointment> OnAppointmentsCreated;

        /// <summary>
        /// Evento invocato dopo l'eliminazione di un paziente dal database.
        /// </summary>
        public event Action OnPatientDeleted;
        /// <summary>
        /// Evento invocato dopo l'eliminazione di un dottore dal database.
        /// </summary>
        public event Action OnDoctorDeleted;
        /// <summary>
        /// Evento invocato dopo l'eliminazione di un appuntamento dal database.
        /// </summary>
        public event Action OnAppointmentDeleted;

        /// <summary>
        /// Evento invocato dopo l'aggiornamento di un paziente dal database.
        /// </summary>
        public event Action<Patient> OnPatientsUpdate;
        /// <summary>
        /// Evento invocato dopo l'aggiornamento di un dottore dal database.
        /// </summary>
		public event Action<Doctor> OnDoctorsUpdate;
        /// <summary>
        /// Evento invocato dopo l'aggiornamento di un appuntamento dal database.
        /// </summary>
        public event Action<Appointment> OnAppointmentsUpdate;
        
        #endregion

        /// <summary>
        /// L'URI di base per tutte le chiamate API (es. "http://localhost:3000/api").
        /// </summary>
        private static string baseUri = "http://localhost:3000/api";

        #region IRepositoryManager - CRUD Functions

        /// <summary>
        /// Avvia la creazione di un nuovo appuntamento nel database.
        /// </summary>
        /// <param name="fk_doctorID">ID del dottore.</param>
        /// <param name="fk_clientID">ID del paziente (cliente).</param>
        /// <param name="startDate">Data e ora di inizio dell'appuntamento.</param>
        public void CreateAppointment(int fk_doctorID, int fk_clientID, DateTime startDate)
        {
            Appointment newAppointment = new Appointment
            {
                idPaziente = fk_doctorID,
                idDottore = fk_clientID,
                inizioApp = startDate
            };
            StartCoroutine(CreateAppointment_DB(newAppointment));
        }

        /// <summary>
        /// Avvia la creazione di un nuovo dottore nel database.
        /// </summary>
        public void CreateDoctor(string name, string surname, string specialization, string email, string password, string phoneNumber, string city, int appointmentDurationInMinutes, string weekDaysAvailable, TimeSpan inHours, TimeSpan fnHours)
        {
            Doctor newDoctor = new Doctor
            {
                nome = name,
                cognome = surname,
                specializzazione = specialization,
                email = email,
                telefono = phoneNumber,
                citta = city,
                durata = appointmentDurationInMinutes,
                giorniDisponibili = weekDaysAvailable,
                orarioInizio = inHours,
				orarioFine = fnHours
            };
            StartCoroutine(CreateDoctor_DB(newDoctor, password));
        }

        /// <summary>
        /// Avvia la creazione di un nuovo paziente nel database.
        /// </summary>
        public void CreatePatient(string name, string surname, string email, string password, string phoneNumber)
        {
            Patient newPatient = new Patient
            {
                nome = name,
                cognome = surname,
                email = email,
                telefono = phoneNumber
            };
            StartCoroutine(CreatePatient_DB(newPatient, password));
        }

        /// <summary>
        /// Avvia la lettura di tutti gli appuntamenti per un paziente specifico.
        /// </summary>
        /// <param name="clientID">ID del paziente (cliente).</param>
        public void ReadAllByClient(int clientID)
        {
            StartCoroutine(ReadAllByClient_DB(clientID));
        }

        /// <summary>
        /// Avvia la lettura di tutti gli appuntamenti per un dottore specifico.
        /// </summary>
        /// <param name="doctorID">ID del dottore.</param>
        public void ReadAllByDoctor(int doctorID)
        {
            StartCoroutine(ReadAllByDoctor_DB(doctorID));
        }

        /// <summary>
        /// Avvia la lettura di un appuntamento specifico tramite il suo ID.
        /// </summary>
        /// <param name="appointmentID">ID dell'appuntamento.</param>
        public void ReadByAppointmentID(int appointmentID)
        {
            StartCoroutine(ReadByAppointmentID_DB(appointmentID));
        }

        /// <summary>
        /// Avvia la lettura di un dottore specifico tramite il suo ID.
        /// </summary>
        /// <param name="id">ID del dottore.</param>
        public void ReadDoctor(int id)
        {
            StartCoroutine(ReadDoctor_DB(id));
        }

        /// <summary>
        /// Avvia la lettura di un paziente specifico tramite il suo ID.
        /// </summary>
        /// <param name="id">ID del paziente.</param>
        public void ReadPatient(int id)
        {
            StartCoroutine(ReadPatient_DB(id));
        }

        /// <summary>
        /// Avvia l'aggiornamento dei dati di un dottore nel database.
        /// </summary>
        /// <param name="id">ID del dottore da aggiornare.</param>
        /// <param name="newData">Oggetto Doctor contenente i nuovi dati.</param>
        public void UpdateDoctor(int id, Doctor newData)
        {
            StartCoroutine(UpdateDoctor_DB(id, newData));
        }

        /// <summary>
        /// Avvia l'aggiornamento dei dati di un paziente nel database.
        /// </summary>
        /// <param name="id">ID del paziente da aggiornare.</param>
        /// <param name="newData">Oggetto Patient contenente i nuovi dati.</param>
        public void UpdatePatient(int id, Patient newData)
        {
            StartCoroutine(UpdatePatient_DB(id, newData));
        }

        /// <summary>
        /// Avvia l'eliminazione di un appuntamento dal database.
        /// </summary>
        /// <param name="appointmentID">ID dell'appuntamento da eliminare.</param>
        public void DeleteAppointment(int appointmentID)
        {
            StartCoroutine(DeleteAppointment_DB(appointmentID));
        }

        /// <summary>
        /// Avvia l'eliminazione di un dottore dal database.
        /// </summary>
        /// <param name="id">ID del dottore da eliminare.</param>
        public void DeleteDoctor(int id)
        {
            StartCoroutine(DeleteDoctor_DB(id));
        }

        /// <summary>
        /// Avvia l'eliminazione di un paziente dal database.
        /// </summary>
        /// <param name="id">ID del paziente da eliminare.</param>
        public void DeletePatient(int id)
        {
            StartCoroutine(DeletePatient_DB(id));
        }
        #endregion

        #region Unity Web Requests

        /// <summary>
        /// Metodo helper per creare richieste POST o PUT con dati JSON.
        /// </summary>
        /// <param name="uri">L'URI completo dell'endpoint.</param>
        /// <param name="method">Il metodo HTTP (es. "POST" o "PUT").</param>
        /// <param name="jsonData">I dati in formato stringa JSON da inviare nel body.</param>
        /// <returns>Un UnityWebRequest pronto per l'invio.</returns>
        private UnityWebRequest CreateJsonRequest(string uri, string method, string jsonData)
        {
            UnityWebRequest request = new UnityWebRequest(uri, method);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            return request;
        }

        // --- CREATE ---
        /// <summary>
        /// Coroutina che esegue la richiesta POST per creare un nuovo appuntamento.
        /// </summary>
        /// <param name="newAppointment">L'oggetto appuntamento da serializzare e inviare.</param>
        private IEnumerator CreateAppointment_DB(Appointment newAppointment)
        {
            string uri = $"{baseUri}/prenotazioni";
            string jsonData = JsonHelper.ToJson(newAppointment);

            using (UnityWebRequest request = CreateJsonRequest(uri, "POST", jsonData))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Creating Appointment: {request.error}");
                }
                else
                {
                    Debug.Log("Appointment Created!");
                    Appointment createdAppointment = JsonUtility.FromJson<Appointment>(request.downloadHandler.text);
                    OnAppointmentsCreated?.Invoke(createdAppointment);
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta POST per creare un nuovo dottore.
        /// </summary>
        /// <param name="newDoctor">L'oggetto dottore da serializzare e inviare.</param>
        private IEnumerator CreateDoctor_DB(Doctor newDoctor, string password)
        {
            string uri = $"{baseUri}/dottori";
            

			RegisterUser newUser = new RegisterUser()
			{
				nome = newDoctor.nome,
				cognome = newDoctor.cognome,
				username = newDoctor.email,
				password = password,
				email = newDoctor.email,
				ruolo = "D",
				telefono = newDoctor.telefono,
				citta = newDoctor.citta,
				specializzazione = newDoctor.specializzazione,
			};

			string jsonData = JsonUtility.ToJson(newUser);
			using (UnityWebRequest request = CreateJsonRequest(uri, "POST", jsonData))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Creating Doctor: {request.error}");
                }
                else
                {
                    Debug.Log("Doctor Created!");
                    Doctor createdDoctor = JsonUtility.FromJson<Doctor>(request.downloadHandler.text);
                    OnDoctorCreated?.Invoke(createdDoctor);
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta POST per creare un nuovo paziente.
        /// </summary>
        /// <param name="newPatient">L'oggetto paziente da serializzare e inviare.</param>
        private IEnumerator CreatePatient_DB(Patient newPatient, string password)
        {
            string uri = $"{baseUri}/register";
			//string jsonData = JsonHelper.ToJson(newPatient);
			// password, ruolo, nome, cognome, email, telefono,
			// specializzazione?, citta?, idPaziente, idDottore
			RegisterUser newUser = new RegisterUser()
			{
				nome = newPatient.nome,
				cognome = newPatient.cognome,
				username = newPatient.nome + newPatient.cognome + newPatient.email,
				password = password,
				email = newPatient.email,
				ruolo = "P",
				telefono = newPatient.telefono,
			};

			string jsonData = JsonUtility.ToJson(newUser);

            using (UnityWebRequest request = CreateJsonRequest(uri, "POST", jsonData))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Creating Patient: {request.error}");
                }
                else
                {
                    Debug.Log("Patient Created!");
                    Patient createdPatient = JsonUtility.FromJson<Patient>(request.downloadHandler.text);
                    OnPatientCreated?.Invoke(createdPatient);
                }
            }
        }

        // --- READ ---
        /// <summary>
        /// Coroutina che esegue la richiesta GET per ottenere tutti gli appuntamenti di un cliente.
        /// </summary>
        /// <param name="clientID">L'ID del cliente.</param>
        private IEnumerator ReadAllByClient_DB(int clientID)
        {
            string uri = $"{baseUri}/prenotazioni/pazienti/{clientID}";
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Reading Appointments: {request.error}");
                }
                else
                {
                    List<Appointment> appointments = JsonHelper.GetAppointmentsFromJson(request.downloadHandler.text);
                    OnAppointmentsLoaded?.Invoke(appointments);
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta GET per ottenere tutti gli appuntamenti di un dottore.
        /// </summary>
        /// <param name="doctorID">L'ID del dottore.</param>
        private IEnumerator ReadAllByDoctor_DB(int doctorID)
        {
            string uri = $"{baseUri}/prenotazioni/dottori/{doctorID}";
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Reading Appointments: {request.error}");
                }
                else
                {
                    List<Appointment> appointments = JsonHelper.GetAppointmentsFromJson(request.downloadHandler.text);
                    OnAppointmentsLoaded?.Invoke(appointments);
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta GET per ottenere un appuntamento specifico.
        /// </summary>
        /// <param name="appointmentID">L'ID dell'appuntamento.</param>
        private IEnumerator ReadByAppointmentID_DB(int appointmentID)
        {
            string uri = $"{baseUri}/prenotazioni/{appointmentID}";
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Reading Appointment: {request.error}");
                }
                else
                {
                    Appointment appointment = JsonUtility.FromJson<Appointment>(request.downloadHandler.text);
                    OnAppointmentsLoaded?.Invoke(new List<Appointment> { appointment });
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta GET per ottenere un dottore specifico.
        /// </summary>
        /// <param name="id">L'ID del dottore.</param>
        private IEnumerator ReadDoctor_DB(int id)
        {
            string uri = $"{baseUri}/dottori/{id}";
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Reading Doctor: {request.error}");
                }
                else
                {
                    Doctor doctor = JsonUtility.FromJson<Doctor>(request.downloadHandler.text);
                    OnDoctorsLoaded?.Invoke(new List<Doctor> { doctor });
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta GET per ottenere un paziente specifico.
        /// </summary>
        /// <param name="id">L'ID del paziente.</param>
        private IEnumerator ReadPatient_DB(int id)
        {
            string uri = $"{baseUri}/pazienti/{id}";
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
			{
				request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Reading Patient: {request.error}");
                }
                else
                {
                    Patient patient = JsonUtility.FromJson<Patient>(request.downloadHandler.text);
                    OnPatientsLoaded?.Invoke(new List<Patient> { patient });
                }
            }
        }

        // --- UPDATE ---
        /// <summary>
        /// Coroutina che esegue la richiesta PUT per aggiornare un dottore.
        /// </summary>
        /// <param name="id">L'ID del dottore da aggiornare.</param>
        /// <param name="newData">L'oggetto dottore con i nuovi dati.</param>
        private IEnumerator UpdateDoctor_DB(int id, Doctor newData)
        {
            string uri = $"{baseUri}/dottori/{id}";
            string jsonData = JsonHelper.ToJson(newData);

            using (UnityWebRequest request = CreateJsonRequest(uri, "PUT", jsonData))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Updating Doctor: {request.error}");
                }
                else
                {
                    Debug.Log("Doctor Updated!");
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta PUT per aggiornare un paziente.
        /// </summary>
        /// <param name="id">L'ID del paziente da aggiornare.</param>
        /// <param name="newData">L'oggetto paziente con i nuovi dati.</param>
        private IEnumerator UpdatePatient_DB(int id, Patient newData)
        {
            string uri = $"{baseUri}/pazienti/{id}";
            string jsonData = JsonHelper.ToJson(newData);

            using (UnityWebRequest request = CreateJsonRequest(uri, "PUT", jsonData))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Updating Patient: {request.error}");
                }
                else
                {
                    Debug.Log("Patient Updated!");
                }
            }
        }

        // --- DELETE ---
        /// <summary>
        /// Coroutina che esegue la richiesta DELETE per eliminare un appuntamento.
        /// </summary>
        /// <param name="appointmentID">L'ID dell'appuntamento da eliminare.</param>
        private IEnumerator DeleteAppointment_DB(int appointmentID)
        {
            string uri = $"{baseUri}/prenotazioni/{appointmentID}";
            using (UnityWebRequest request = UnityWebRequest.Delete(uri))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Deleting Appointment: {request.error}");
                }
                else
                {
                    Debug.Log("Appointment Deleted!");
                    OnAppointmentDeleted?.Invoke();
                }
            }
        }

        /// <summary>
        /// Coroutina che esegue la richiesta DELETE per eliminare un dottore.
        /// </summary>
        /// <param name="id">L'ID del dottore da eliminare.</param>
        private IEnumerator DeleteDoctor_DB(int id)
        {
            string uri = $"{baseUri}/dottori/{id}";
            using (UnityWebRequest request = UnityWebRequest.Delete(uri))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error Deleting Doctor: {request.error}");
                }
                else
                {
                    Debug.Log("Doctor Deleted!");
                    OnDoctorDeleted?.Invoke();
                }
            }
        }

		/// <summary>
		/// Coroutina che esegue la richiesta DELETE per eliminare un paziente.
		/// </summary>
		/// <param name="id">L'ID del paziente da eliminare.</param>
		private IEnumerator DeletePatient_DB(int id)
		{
			string uri = $"{baseUri}/pazienti/{id}";
			using (UnityWebRequest request = UnityWebRequest.Delete(uri))
			{
				yield return request.SendWebRequest();

				if (request.result != UnityWebRequest.Result.Success)
				{
					Debug.LogError($"Error Deleting Patient: {request.error}");
				}
				else
				{
					Debug.Log("Patient Deleted!");
					OnPatientDeleted?.Invoke();
				}
			}
		}
		
		/// <summary>
		/// Coroutina pubblica per testare la connessione all'API.
		/// Esegue una semplice richiesta GET e invoca un callback con il risultato.
		/// </summary>
		/// <param name="callback">Azione da invocare con 'true' se la connessione ha successo, altrimenti 'false'.</param>
		public IEnumerator TestConnection_DB(Action<bool> callback)
		{
			// Usiamo un endpoint semplice (es. /doctors) per il test
			string uri = $"{baseUri}/dottori"; 

			using (UnityWebRequest request = UnityWebRequest.Get(uri))
			{
				request.timeout = 5; // 5 secondi di timeout
				
				yield return request.SendWebRequest();

				// Controlla se la richiesta ha avuto successo (es. 200 OK)
				if (request.result == UnityWebRequest.Result.Success)
				{
					Debug.Log("DB Connection Test SUCCEEDED.");
					callback?.Invoke(true); // Successo!
				}
				else
				{
					Debug.LogWarning($"DB Connection Test FAILED: {request.error}");
					callback?.Invoke(false); // Fallito
				}
			}
		}
        #endregion
    }
}
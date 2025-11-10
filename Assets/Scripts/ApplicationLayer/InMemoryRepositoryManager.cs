using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Appointix.Domain;
using Appointix.Domain.Interfaces;

namespace Appointix.ApplicationLayer
{
    /// <summary>
    /// Singleton to allow CRUD operations on mock data, given when a MySQL database is not available
    /// The data isn't persistend, but it allows the project to run and demonstrate functionality without needing to setup MySQL services on the user's computer
    /// </summary>
    public class InMemoryRepositoryManager : IRepositoryManager
    {
        #region Singleton
        private static InMemoryRepositoryManager instance;
        public static InMemoryRepositoryManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new InMemoryRepositoryManager();
                }
                return instance;
            }
        }
        private InMemoryRepositoryManager() 
        {
            //patientsJsonText = AppContext.Instance.patientsJson.text;
            //LoadPatients();
        }
        #endregion

        private string patientsJsonText;

        private void LoadPatients()
        {
            List<Patient> patientsList = JsonHelper.GetPatientsFromJson(patientsJsonText);
            foreach (Patient patient in patientsList)
            {
                allPatients.Add(patient.id, patient);
            }
        }

        #region IRepositoryManager - Events
        public event Action<List<Patient>> OnPatientsLoaded;
        public event Action<List<Doctor>> OnDoctorsLoaded;
        public event Action<List<Appointment>> OnAppointmentsLoaded;

        public event Action<Patient> OnPatientCreated;
        public event Action<Doctor> OnDoctorCreated;
        public event Action<Appointment> OnAppointmentsCreated;
        
        public event Action<Patient> OnPatientsUpdate;
        public event Action<Doctor> OnDoctorsUpdate;
        public event Action<Appointment> OnAppointmentsUpdate;

        public event Action OnPatientDeleted;
        public event Action OnDoctorDeleted;
        public event Action OnAppointmentDeleted;

        public event Action<Patient> OnPatientLoginSuccess;
        public event Action<Doctor> OnDoctorLoginSuccess;
        public event Action<string> OnLoginFailed;

        #endregion

        private Dictionary<int, Patient> allPatients = new();
        private Dictionary<int, Doctor> allDoctors = new();
        private Dictionary<int, Appointment> allAppointments = new();

        #region IRepositoryManager - CRUD Functions
        #region Create
        public void CreateAppointment(int fk_doctorID, int fk_clientID, DateTime startDate)
        {
            Appointment appointment = new Appointment();
            // Verifica se esiste già un appuntamento con gli stessi dati
            bool alreadyExists = allAppointments.Values.Any(a =>
                a.idPaziente == fk_doctorID &&
                a.idDottore == fk_clientID &&
                a.inizioApp == startDate
            );

            if (alreadyExists)
            {
    
                Debug.Log("⚠️ L'appuntamento esiste già.");
            }
            else
            {
                // Se non esiste, lo aggiungo
                appointment = (new Appointment
                {
                    idPaziente = fk_doctorID,
                    idDottore = fk_clientID,
                    inizioApp = startDate
                });

                Debug.Log("✅ Nuovo appuntamento creato.");
            }
            OnAppointmentsCreated?.Invoke(appointment);
            
        }

        public void CreateDoctor(string name, string surname, string specialization, string email, string password, string phoneNumber, string city, int appointmentDurationInMinutes, string weekDaysAvailable, TimeSpan inHours, TimeSpan fnHours)
        {
            Doctor doctor = new Doctor();
            // Verifica se esiste già un dottore con gli stessi attributi
            bool alreadyExists = allDoctors.Values.Any(a =>
                a.email == email
            );

            if (alreadyExists)
            {
                Debug.Log("⚠️ Il dottore gia esiste.");
            }
            else
            {
                // Se non esiste, lo aggiungo
                doctor = (new Doctor
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
                });

                Debug.Log("✅ Nuovo dottore creato.");
            }
            OnDoctorCreated?.Invoke(doctor);
        }

        public void CreatePatient(string name, string surname, string email, string password, string phoneNumber)
        {
            Patient patient = new Patient();
            // Verifica se esiste già un paziente con gli stessi attributi
            bool alreadyExists = allPatients.Values.Any(a =>
            a.email == email
            );

            if (alreadyExists)
            {
                Debug.Log("⚠️ Il paziente gia esiste.");
            }
            else
            {
                // Se non esiste, lo aggiungo
                patient = (new Patient
                {
                    nome = name,
                    cognome = surname,
                    email = email,
                    telefono = phoneNumber
                });

                Debug.Log("✅ Nuovo paziente creato.");
            }
            OnPatientCreated?.Invoke(patient);
        }
        #endregion
        #region Read
        public void ReadAllByClient(int clientID)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach(Appointment app in allAppointments.Values)
            {
                if (app.idDottore == clientID)
                {
                    appointments.Add(app);
                }
            }
            OnAppointmentsLoaded?.Invoke(appointments);
        }

        public void ReadAllByDoctor(int doctorID)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach(Appointment app in allAppointments.Values)
            {
                if (app.idPaziente == doctorID)
                {
                    appointments.Add(app);
                }
            }
            OnAppointmentsLoaded?.Invoke(appointments);
        }

        public void ReadByAppointmentID(int appointmentID)
        {
            List<Appointment> appointments = new List<Appointment>();
            // Per evitare errori se l'ID non è presente, uso TryGetValue
            if (allAppointments.TryGetValue(appointmentID, out Appointment app))
            {
                appointments.Add(app);
            }
            OnAppointmentsLoaded?.Invoke(appointments);
        }

        public void ReadDoctor(int id)
        {
            List<Doctor> doctors = new List<Doctor>();
            if (allDoctors.TryGetValue(id, out Doctor doc))
            {
                doctors.Add(doc);
            }
            OnDoctorsLoaded?.Invoke(doctors);
        }

        public void ReadPatient(int id)
        {
            List<Patient> patients = new List<Patient>();
            if (allPatients.TryGetValue(id, out Patient pat))
            {
                patients.Add(pat);
            }
            OnPatientsLoaded?.Invoke(patients);
        }
        #endregion
        #region Update
        public void UpdateDoctor(int id, Doctor newData)
        {
            throw new NotImplementedException();
        }

        public void UpdatePatient(int id, Patient newData)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Delete
        public void DeleteAppointment(int appointmentID)
        {
            // 1️⃣ Controlla se l'appuntamento esiste nel dizionario
            if (allAppointments.TryGetValue(appointmentID, out Appointment appointmentToDelete))
            {
                // 2️⃣ Rimuovi l'appuntamento dal dizionario
                allAppointments.Remove(appointmentID);

                // 3️⃣ Stampa a console per debug
                Debug.Log($"🗑️ Appuntamento con ID {appointmentID} eliminato con successo.");

                // 4️⃣ Esegui eventuali callback/eventi collegati
                OnAppointmentDeleted?.Invoke();
            }
            else
            {
                // ❌ Nessun appuntamento trovato con quell’ID
                Debug.LogWarning($"⚠️ Nessun appuntamento trovato con ID {appointmentID}. Nessuna eliminazione effettuata.");
            }
        }

        public void DeleteDoctor(int id)
        {
            throw new NotImplementedException();
        }

        public void DeletePatient(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        // --- NUOVO METODO DI LOGIN ---
        #region Login
        public void Login(string email, string password, string ruolo)
        {

            if (ruolo == "P")
            {
                // Cerca il paziente tramite email
                Patient patient = allPatients.Values.FirstOrDefault(p => p.email == email);
                if (patient != null)
                {
                    Debug.Log($"Mock Login: Paziente {patient.nome} trovato.");
                    OnPatientLoginSuccess?.Invoke(patient);
                }
                else
                {
                    Debug.LogWarning("Mock Login: Paziente non trovato con questa email.");
                    OnLoginFailed?.Invoke("Email o ruolo errati (Mock)");
                }
            }
            else if (ruolo == "D")
            {
                // Cerca il dottore tramite email
                Doctor doctor = allDoctors.Values.FirstOrDefault(d => d.email == email);
                if (doctor != null)
                {
                    Debug.Log($"Mock Login: Dottore {doctor.nome} trovato.");
                    OnDoctorLoginSuccess?.Invoke(doctor);
                }
                else
                {
                    Debug.LogWarning("Mock Login: Dottore non trovato con questa email.");
                    OnLoginFailed?.Invoke("Email o ruolo errati (Mock)");
                }
            }
            else
            {
                Debug.LogWarning($"Mock Login: Ruolo sconosciuto '{ruolo}'.");
                OnLoginFailed?.Invoke("Ruolo non valido (Mock)");
            }
        }
        #endregion
    }
}
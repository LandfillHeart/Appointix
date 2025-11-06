using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
			patientsJsonText = AppContext.Instance.patientsJson.text;
			LoadPatients();
		}
		#endregion

		private string patientsJsonText;

		private void LoadPatients()
		{
			List<Patient> patientsList = JsonHelper.GetPatientsFromJson(patientsJsonText);
			foreach (Patient patient in patientsList)
			{
				allPatients.Add(patient.ID, patient);
			}
		}

		#region IRepositoryManager - Events
		public event Action<List<Patient>> OnPatientsLoaded;
		public event Action<List<Doctor>> OnDoctorsLoaded;
		public event Action<List<Appointment>> OnAppointmentsLoaded;

		public event Action<Patient> OnPatientCreated;
		public event Action<Doctor> OnDoctorCreated;
		public event Action<Appointment> OnAppointmentsCreated;
		
		public event Action OnPatientDeleted;
		public event Action OnDoctorDeleted;
		public event Action OnAppointmentDeleted;
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
				a.FK_Doctor_ID == fk_doctorID &&
				a.FK_Patient_ID == fk_clientID &&
				a.StartDateTime == startDate
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
					FK_Doctor_ID = fk_doctorID,
					FK_Patient_ID = fk_clientID,
					StartDateTime = startDate
				});

				Debug.Log("✅ Nuovo appuntamento creato.");
			}
			OnAppointmentsCreated?.Invoke(appointment);
			
		}

		public void CreateDoctor(string name, string surname, string specialization, string email, string phoneNumber, string city, int appointmentDurationInMinutes, string weekDaysAvailable, TimeSpan inHours, TimeSpan fnHours)
        {
            Doctor doctor = new Doctor();
			// Verifica se esiste già un dottore con gli stessi attributi
			bool alreadyExists = allDoctors.Values.Any(a =>
				a.Email == email
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
					Name = name,
					Surname = surname,
					Specialization = specialization,
					Email = email,
					PhoneNumber = phoneNumber,
					City = city,
					AppointmentDurationInMinutes = appointmentDurationInMinutes,
					WeekDaysAvailable = weekDaysAvailable,
					InHours = inHours,
					FnHours = fnHours
				});

				Debug.Log("✅ Nuovo dottore creato.");
			}
			OnDoctorCreated?.Invoke(doctor);
        }

		public void CreatePatient(string name, string surname, string email, string phoneNumber)
		{
			Patient patient = new Patient();
			// Verifica se esiste già un paziente con gli stessi attributi
			bool alreadyExists = allPatients.Values.Any(a =>
			a.Email == email
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
                	Name = name,
					Surname = surname,
					Email = email,
					PhoneNumber = phoneNumber
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
                if (app.FK_Patient_ID == clientID)
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
                if (app.FK_Doctor_ID == doctorID)
                {
					appointments.Add(app);
                }
            }
			OnAppointmentsLoaded?.Invoke(appointments);
		}

		public void ReadByAppointmentID(int appointmentID)
		{
			List<Appointment> appointments = new List<Appointment>();
			appointments.Add(allAppointments[appointmentID]);
			OnAppointmentsLoaded?.Invoke(appointments);
		}

		public void ReadDoctor(int id)
		{
			List<Doctor> doctors = new List<Doctor>();
			doctors.Add(allDoctors[id]);
			OnDoctorsLoaded?.Invoke(doctors);
		}

		public void ReadPatient(int id)
		{
			List<Patient> patients = new List<Patient>();
			patients.Add(allPatients[id]);
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
			throw new NotImplementedException();
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


	}
}
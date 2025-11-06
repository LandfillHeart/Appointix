using System;
using System.Collections.Generic;
using UnityEngine;

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
			throw new NotImplementedException();
		}

		public void CreateDoctor(string name, string surname, string specialization, string email, string phoneNumber, string city, int appointmentDurationInMinutes, string weekDaysAvailable, TimeSpan availableHours)
		{
			throw new NotImplementedException();
		}

		public void CreatePatient(string name, string surname, string email, string phoneNumber)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Read
		public void ReadAllByClient(int clientID)
		{
			throw new NotImplementedException();
		}

		public void ReadAllByDoctor(int doctorID)
		{
			throw new NotImplementedException();
		}

		public void ReadByAppointmentID(int appointmentID)
		{
			throw new NotImplementedException();
		}

		public void ReadDoctor(int id)
		{
			throw new NotImplementedException();
		}

		public void ReadPatient(int id)
		{
			// return allPatients[id];
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
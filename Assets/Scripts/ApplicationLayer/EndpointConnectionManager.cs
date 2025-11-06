using Appointix.Domain.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Appointix.ApplicationLayer
{
	/// <summary>
	/// Singleton that communicates with the JS Endpoint to interact with the MySQL Database
	/// Extends IRepositoryManager to allow Dependency Injection / Astraction between using data from a Database or Mock data in Memory
	/// </summary>
	public class EndpointConnectionManager : MonoBehaviour, IRepositoryManager
	{
		#region Singleton
		private static EndpointConnectionManager instance;
		public static EndpointConnectionManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new EndpointConnectionManager();
				}
				return instance;
			}
		}
		private EndpointConnectionManager() { }
		#endregion

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

		private static string baseUri = "http://localhost:3000/api";

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
			StartCoroutine(ReadDoctor_DB(id));
		}

		public void ReadPatient(int id)
		{
			StartCoroutine(ReadPatient_DB(id));
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

		#region Unity Web Requests
		#region Create
		private IEnumerator CreateAppointment_DB(Appointment newAppointment)
		{
			yield break;
		}

		private IEnumerator CreateDoctor_DB(Doctor newDoctor)
		{
			yield break;
		}

		private IEnumerator CreatePatient_DB(Patient newPatient)
		{
			yield break;
		}
		#endregion
		#region Read
		private IEnumerator ReadAllByClient_DB(int clientID)
		{
			yield break;
		}

		private IEnumerator ReadAllByDoctor_DB(int doctorID)
		{
			yield break;
		}

		private IEnumerator ReadByAppointmentID_DB(int appointmentID)
		{
			yield break;
		}

		private IEnumerator ReadDoctor_DB(int id)
		{
			yield break;
		}

		private IEnumerator ReadPatient_DB(int id)
		{
			
			yield break;
		}
		#endregion
		#region Update
		private IEnumerator UpdateDoctor_DB(int id, Doctor newData)
		{
			yield break;
		}

		private IEnumerator UpdatePatient_DB(int id, Patient newData)
		{
			yield break;
		}
		#endregion
		#region Delete
		private IEnumerator DeleteAppointment_DB(int appointmentID)
		{
			yield break;
		}
		private IEnumerator DeleteDoctor_DB(int id) 
		{ 
			yield break;
		}

		private IEnumerator DeletePatient_DB(int id)
		{
			yield break;
		}
		#endregion
		#endregion

	}

}

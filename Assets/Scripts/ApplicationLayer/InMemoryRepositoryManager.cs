using System;
using System.Collections.Generic;

namespace Appointix.ApplicationLayer
{
	/// <summary>
	/// Singleton to allow CRUD operations on mock data, given when a MySQL database is not available
	/// The data isn't persistend, but it allows the project to run and demonstrate functionality without needing to setup MySQL services on the user's computer
	/// </summary>
	public class InMemoryRepositoryManager : IRepositoryManager
	{
		#region IRepositoryManager
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
		public List<Appointment> ReadAllByClient(int clientID)
		{
			throw new NotImplementedException();
		}

		public List<Appointment> ReadAllByDoctor(int doctorID)
		{
			throw new NotImplementedException();
		}

		public Appointment ReadByAppointmentID(int appointmentID)
		{
			throw new NotImplementedException();
		}

		public Doctor ReadDoctor(int id)
		{
			throw new NotImplementedException();
		}

		public Patient ReadPatient(int id)
		{
			throw new NotImplementedException();
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
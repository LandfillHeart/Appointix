
using System;
using System.Collections.Generic;

namespace Appointix.Domain.Interfaces
{
	public interface IAppointmentRepository
	{
		public void CreateAppointment(int fk_doctorID, int fk_clientID, DateTime startDate); // end date defined by doctor.appointmentDurationInMinutes

		public void ReadByAppointmentID(int appointmentID);
		public void ReadAllByClient(int clientID);
		public void ReadAllByDoctor(int doctorID);

		public void DeleteAppointment(int appointmentID);
	}
}

using System;
using System.Collections.Generic;

namespace Appointix.Domain.Interfaces
{
	public interface IAppointmentRepository
	{
		public void CreateAppointment(int fk_doctorID, int fk_clientID, DateTime startDate); // end date defined by doctor.appointmentDurationInMinutes

		public Appointment ReadByAppointmentID(int appointmentID);
		public List<Appointment> ReadAllByClient(int clientID);
		public List<Appointment> ReadAllByDoctor(int doctorID);

		public void DeleteAppointment(int appointmentID);
	}
}
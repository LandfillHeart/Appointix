using Appointix.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Appointix.ApplicationLayer
{
	public interface IRepositoryManager : IPatientRepository, IDoctorRepository, IAppointmentRepository
	{
		public event Action<List<Patient>> OnPatientsLoaded;
		public event Action<List<Doctor>> OnDoctorsLoaded;
		public event Action<List<Appointment>> OnAppointmentsLoaded;

		public event Action<Patient> OnPatientCreated;
		public event Action<Doctor> OnDoctorCreated;
		public event Action<Appointment> OnAppointmentsCreated;

		public event Action OnPatientDeleted;
		public event Action OnDoctorDeleted;
		public event Action OnAppointmentDeleted;
	}
}

using System;

namespace Appointix.Domain.Interfaces
{
	public interface IDoctorRepository
	{
		// password, ruolo, nome, cognome, email, telefono,
		// specializzazione?, citta?, idPaziente, idDottore
		public void CreateDoctor(string name, string surname, string specialization, string email, string password, string phoneNumber, string city, int appointmentDurationInMinutes, string weekDaysAvailable, TimeSpan inHours, TimeSpan fnHours);
		public void ReadDoctor(int id);
		public void UpdateDoctor(int id, Doctor newData);
		public void DeleteDoctor(int id);
	}
}



namespace Appointix.Domain.Interfaces
{
	public interface IPatientRepository
	{
		// password, ruolo, nome, cognome, email, telefono,
		// specializzazione?, citta?, idPaziente, idDottore
		public void CreatePatient(string name, string surname, string email, string password, string phoneNumber);
		public void ReadPatient(int id);
		public void UpdatePatient(int id, Patient newData);
		public void DeletePatient(int id);
	}
}
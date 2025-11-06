
namespace Appointix.Domain.Interfaces
{
	public interface IPatientRepository
	{
		public void CreatePatient(string name, string surname, string email, string phoneNumber);
		public void ReadPatient(int id);
		public void UpdatePatient(int id, Patient newData);
		public void DeletePatient(int id);
	}
}
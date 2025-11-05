using Appointix.Domain;
using Appointix.Domain.Interfaces;

namespace Appointix.ApplicationLayer
{
	public interface IRepositoryManager : IPatientRepository, IDoctorRepository, IAppointmentRepository
	{

	}
}
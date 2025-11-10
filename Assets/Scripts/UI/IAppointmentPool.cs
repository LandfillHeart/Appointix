using System;

namespace Appointix.UI
{
	public interface IAppointmentPool
	{
		public event Action OnListClean;
		public void Repool(AppointmentItem appointmentItem);
	}
}
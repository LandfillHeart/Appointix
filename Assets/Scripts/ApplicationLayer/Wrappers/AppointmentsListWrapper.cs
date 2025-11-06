using System;
using System.Collections.Generic;


namespace Appointix.ApplicationLayer
{
	[Serializable]
	public class AppointmentsListWrapper
	{
		public Appointment[] appointments;

		public AppointmentsListWrapper(Appointment[] appointments)
		{
			this.appointments = appointments;
		}

		public List<Appointment> ToList()
		{
			if (appointments == null)
			{
				return new List<Appointment>();
			}
			return new List<Appointment>(appointments);
		}
	}
}
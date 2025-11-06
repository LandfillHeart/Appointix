using System;
using System.Collections.Generic;


namespace Appointix.ApplicationLayer
{
	[Serializable]
	public class DoctorsListWrapper
	{
		public Doctor[] doctors;

		public DoctorsListWrapper(Doctor[] doctors)
		{
			this.doctors = doctors;
		}

		public List<Doctor> ToList()
		{
			if (doctors == null)
			{
				return new List<Doctor>();
			}
			return new List<Doctor>(doctors);
		}
	}
}
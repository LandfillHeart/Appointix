using System;
using System.Collections.Generic;


namespace Appointix.ApplicationLayer
{
	[Serializable]
	public class PatientsListWrapper
	{
		public Patient[] patients;

		public PatientsListWrapper(Patient[] patients)
		{
			this.patients = patients;
		}

		public List<Patient> ToList()
		{
			if (patients == null)
			{
				return new List<Patient>();
			}
			return new List<Patient>(patients);
		}
	}
}
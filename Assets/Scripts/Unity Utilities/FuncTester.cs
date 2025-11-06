using UnityEngine;
using Appointix.ApplicationLayer;
using System.Collections.Generic;

namespace Appointix.UnityUtilities
{
	public class FuncTester : MonoBehaviour
	{
		private IRepositoryManager repoManager;

		public void Start()
		{
			AppContext.Instance.OnRepositoryManagerSet += (newManager) => repoManager = newManager;
		}

		public void AppContextGetConnectionDB()
		{
			Debug.Log(AppContext.Instance.TryConnectionToDB());
		}

		public void ReadPatientsFromRepo()
		{
			Patient patientCache; 
			for(int i = 1; i < 300; i++)
			{
				patientCache = repoManager.ReadPatient(i);
				Debug.Log($"{patientCache.ID} - {patientCache.Name} - {patientCache.Surname} - {patientCache.Email} - {patientCache.PhoneNumber} ");
			}
		}
	}
}
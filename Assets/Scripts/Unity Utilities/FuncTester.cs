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
			repoManager.ReadPatient(1);
			
		}
	}
}
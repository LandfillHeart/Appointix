
using System;
using UnityEngine;

namespace Appointix.ApplicationLayer
{
	public class AppContext : MonoBehaviour
	{
		#region Singleton
		private static AppContext instance;
		public static AppContext Instance => instance;
		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				return;
			}
			Destroy(gameObject);
		}
		#endregion

		[SerializeField] public TextAsset patientsJson;

		#region Events
		public event Action<IRepositoryManager> OnRepositoryManagerSet;
		#endregion

		public IRepositoryManager RepositoryManager { get; private set; }

		public bool TryConnectionToDB()
		{
			RepositoryManager = InMemoryRepositoryManager.Instance;
			OnRepositoryManagerSet(RepositoryManager);
			return false;
		}
	}
}
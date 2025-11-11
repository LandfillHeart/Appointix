using Appointix.ApplicationLayer;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using AppContext = Appointix.ApplicationLayer.AppContext;

namespace Appointix.UI
{
	public class PatientUI : MonoBehaviour, IAppointmentPool
	{
		[SerializeField] private GameObject appointmentsPanel;
		[SerializeField] private GameObject userPanel;

		[Header("Appointments Objs")]
		[SerializeField] private GameObject appointmentsItemPrefab;
		[SerializeField] private RectTransform appointmentsRectView;

		[Header("User Obs")]
		[SerializeField] private TextMeshProUGUI userName;
		[SerializeField] private TextMeshProUGUI userSurname;
		[SerializeField] private TextMeshProUGUI userEmail;

		public event Action OnListClean;

		private IRepositoryManager repoManager;

		private Queue<AppointmentItem> itemsPool = new();

		private void Start()
		{
			repoManager = AppContext.Instance.RepositoryManager;
			repoManager.OnAppointmentsLoaded += DisplayAppointments;
			repoManager.ReadAllByClient(AppContext.Instance.userID);
		}

		private void DisplayAppointments(List<Appointment> appsToDisplay)
		{
			AppointmentItem itemCache;
			OnListClean?.Invoke();
			foreach (Appointment appointment in appsToDisplay)
			{
				itemCache = Depool();
				itemCache.Setup(appointment, this);
			}
		}

		#region Pool Pattern
		// Pool Pattern: design pattern solitamente usato nello sviluppo giochi
		// piuttosto che creare constantemente oggetti nuovi, quelli inutilizzati vengono aggiunti ad un pool
		// se ci sono elementi nel pool, utilizza quelli piuttosto che instanziare un oggetto nuovo
		// questo perch� creare/eliminare oggetti � pi� lento di attivarli e disattivarli
		private AppointmentItem Depool()
		{
			if (itemsPool.Count == 0)
			{
				return Instantiate(appointmentsItemPrefab, appointmentsRectView).GetComponent<AppointmentItem>();
			}

			AppointmentItem toReturn = itemsPool.Dequeue();
			toReturn.gameObject.SetActive(true);
			return toReturn;
		}

		public void Repool(AppointmentItem item)
		{
			itemsPool.Enqueue(item);
		}
		#endregion
		public void SwitchToAppointments()
		{
			appointmentsPanel.SetActive(true);
			userPanel.SetActive(false);
		}

		public void SwitchToUser()
		{
			appointmentsPanel.SetActive(false);
			userPanel.SetActive(true);
		}


	}

}

using TMPro;
using UnityEngine;

namespace Appointix.UI
{
	public class AppointmentItem : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI userName;
		[SerializeField] private TextMeshProUGUI appointmentTime;

		private Appointment toDisplay;
		private DoctorUI owner;

		public void Setup(Appointment appointment, IAppointmentPool owner)
		{
			owner.OnListClean += () => gameObject.SetActive(false);
			toDisplay = appointment;
			appointmentTime.text = appointment.inizioApp.DayOfWeek.ToString();
		}

		private void OnDisable()
		{
			owner.OnListClean -= () => gameObject.SetActive(false);
			owner.Repool(this);
		}
	}
}
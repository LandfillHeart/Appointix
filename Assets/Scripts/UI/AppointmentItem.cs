using TMPro;
using UnityEngine;
using Appointix.Domain;
using System;

namespace Appointix.UI
{
    /// <summary>
    /// Gestisce la visualizzazione di un singolo prefab di appuntamento (AppointmentItem)
    /// e la sua interazione con un sistema di object pooling (IAppointmentPool).
    /// </summary>
    public class AppointmentItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI userName;
        [SerializeField] private TextMeshProUGUI appointmentTime;
        [SerializeField] private TextMeshProUGUI specializationText;

        private Appointment toDisplay;
        private IAppointmentPool owner;

        /// <summary>
        /// Inizializza questo item con i dati di un appuntamento
        /// e si iscrive agli eventi del sistema di pooling.
        /// </summary>
        /// <param name="appointment">I dati dell'appuntamento da mostrare.</param>
        /// <param name="owner">Il controller del pool che gestisce questo oggetto.</param>
        public void Setup(Appointment appointment, IAppointmentPool owner)
        {
            this.owner = owner;
            owner.OnListClean += SelfDestruct;
            toDisplay = appointment;

            userName.text = $"Dott. {appointment.nomeDottore} {appointment.cognomeDottore}";

            if (specializationText != null)
            {
                specializationText.text = appointment.specDottore;
            }

            appointmentTime.text = appointment.inizioApp.ToString("g");
}

        /// <summary>
        /// Metodo privato chiamato dall'evento OnListClean per disattivare l'oggetto.
        /// </summary>
        private void SelfDestruct()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Chiamato da Unity quando l'oggetto viene disabilitato (messo in pool).
        /// Pulisce l'iscrizione all'evento OnListClean.
        /// </summary>
        private void OnDisable()
        {
            if (owner != null)
            {
                owner.OnListClean -= SelfDestruct;
                // owner.Repool(this); // La tua logica di repool originale
            }
        }
    }
}
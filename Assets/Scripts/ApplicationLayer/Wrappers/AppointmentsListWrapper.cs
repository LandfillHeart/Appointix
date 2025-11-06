using System;
using System.Collections.Generic;

namespace Appointix.ApplicationLayer
{
    /// <summary>
    /// Classe wrapper ("contenitore") utilizzata per serializzare e deserializzare
    /// una lista di oggetti Appointment, aggirando le limitazioni di JsonUtility
    /// che non gestisce array o liste come elemento radice.
    /// </summary>
    [Serializable]
    public class AppointmentsListWrapper
    {
        /// <summary>
        /// L'array di appuntamenti contenuto nel wrapper.
        /// </summary>
        public Appointment[] appointments;

        /// <summary>
        /// Crea una nuova istanza del wrapper.
        /// </summary>
        /// <param name="appointments">L'array di appuntamenti da includere nel wrapper.</param>
        public AppointmentsListWrapper(Appointment[] appointments)
        {
            this.appointments = appointments;
        }

        /// <summary>
        /// Converte l'array interno di appuntamenti in una List<Appointment>.
        /// </summary>
        /// <returns>Una nuova List<Appointment> contenente gli appuntamenti.
        /// Restituisce una lista vuota se l'array interno è nullo.</returns>
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
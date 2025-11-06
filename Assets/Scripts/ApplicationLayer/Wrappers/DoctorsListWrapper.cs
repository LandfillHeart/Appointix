using System;
using System.Collections.Generic;

namespace Appointix.ApplicationLayer
{
    /// <summary>
    /// Classe wrapper ("contenitore") utilizzata per serializzare e deserializzare
    /// una lista di oggetti Doctor, aggirando le limitazioni di JsonUtility
    /// che non gestisce array o liste come elemento radice.
    /// </summary>
    [Serializable]
    public class DoctorsListWrapper
    {
        /// <summary>
        /// L'array di dottori contenuto nel wrapper.
        /// </summary>
        public Doctor[] doctors;

        /// <summary>
        /// Crea una nuova istanza del wrapper.
        /// </summary>
        /// <param name="doctors">L'array di dottori da includere nel wrapper.</param>
        public DoctorsListWrapper(Doctor[] doctors)
        {
            this.doctors = doctors;
        }

        /// <summary>
        /// Converte l'array interno di dottori in una List<Doctor>.
        /// </summary>
        /// <returns>Una nuova List<Doctor> contenente i dottori.
        /// Restituisce una lista vuota se l'array interno è nullo.</returns>
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
using System;
using System.Collections.Generic;

namespace Appointix.ApplicationLayer
{
    /// <summary>
    /// Classe wrapper ("contenitore") utilizzata per serializzare e deserializzare
    /// una lista di oggetti Patient, aggirando le limitazioni di JsonUtility
    /// che non gestisce array o liste come elemento radice.
    /// </summary>
    [Serializable]
    public class PatientsListWrapper
    {
        /// <summary>
        /// L'array di pazienti contenuto nel wrapper.
        /// </summary>
        public Patient[] patients;

        /// <summary>
        /// Crea una nuova istanza del wrapper.
        /// </summary>
        /// <param name="patients">L'array di pazienti da includere nel wrapper.</param>
        public PatientsListWrapper(Patient[] patients)
        {
            this.patients = patients;
        }

        /// <summary>
        /// Converte l'array interno di pazienti in una List<Patient>.
        /// </summary>
        /// <returns>Una nuova List<Patient> contenente i pazienti.
        /// Restituisce una lista vuota se l'array interno è nullo.</returns>
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
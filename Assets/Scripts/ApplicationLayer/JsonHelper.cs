using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Appointix.ApplicationLayer
{
    /// <summary>
    /// Utility Class which holds all the functions to retrieve data from json text files and convert objects into json
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Deserializza una stringa JSON (che rappresenta un array) in una lista di oggetti Patient.
        /// </summary>
        /// <param name="json">La stringa JSON contenente l'array di pazienti (es. "[{...}, {...}]").</param>
        /// <returns>Una List<Patient> contenente i dati deserializzati.</returns>
        public static List<Patient> GetPatientsFromJson(string json)
        {
            string wrappedJson = "{\"patients\":" + json + "}";
            // Si presume che PatientsListWrapper abbia un metodo .ToList() o una proprietà .patients
            return JsonUtility.FromJson<PatientsListWrapper>(wrappedJson).ToList(); 
        }

        /// <summary>
        /// Deserializza una stringa JSON (che rappresenta un array) in una lista di oggetti Doctor.
        /// </summary>
        /// <param name="json">La stringa JSON contenente l'array di dottori (es. "[{...}, {...}]").</param>
        /// <returns>Una List<Doctor> contenente i dati deserializzati.</returns>
        public static List<Doctor> GetDoctorsFromJson(string json)
        {
            string wrappedJson = "{\"doctors\":" + json + "}";
            return JsonUtility.FromJson<DoctorsListWrapper>(wrappedJson).ToList();
        }

        /// <summary>
        /// Deserializza una stringa JSON (che rappresenta un array) in una lista di oggetti Appointment.
        /// </summary>
        /// <param name="json">La stringa JSON contenente l'array di appuntamenti (es. "[{...}, {...}]").</param>
        /// <returns>Una List<Appointment> contenente i dati deserializzati.</returns>
        public static List<Appointment> GetAppointmentsFromJson(string json)
        {
            string wrappedJson = "{\"appointments\":" + json + "}";
            return JsonUtility.FromJson<AppointmentsListWrapper>(wrappedJson).ToList();
        }

        /// <summary>
        /// Converte un singolo oggetto Patient in una stringa JSON.
        /// </summary>
        /// <param name="patient">L'oggetto Patient da serializzare.</param>
        /// <param name="prettyPrint">Se true, formatta il JSON per la leggibilità (con rientri).</param>
        /// <returns>Una stringa JSON che rappresenta l'oggetto.</returns>
        public static string ToJson(Patient patient, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(patient, prettyPrint);
        }

        /// <summary>
        /// Converte un singolo oggetto Doctor in una stringa JSON.
        /// </summary>
        /// <param name="doctor">L'oggetto Doctor da serializzare.</param>
        /// <param name="prettyPrint">Se true, formatta il JSON per la leggibilità (con rientri).</param>
        /// <returns>Una stringa JSON che rappresenta l'oggetto.</returns>
        public static string ToJson(Doctor doctor, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(doctor, prettyPrint);
        }

        /// <summary>
        /// Converte un singolo oggetto Appointment in una stringa JSON.
        /// </summary>
        /// <param name="appointment">L'oggetto Appointment da serializzare.</param>
        /// <param name="prettyPrint">Se true, formatta il JSON per la leggibilità (con rientri).</param>
        /// <returns>Una stringa JSON che rappresenta l'oggetto.</returns>
        public static string ToJson(Appointment appointment, bool prettyPrint = false)
        {
            return JsonUtility.ToJson(appointment, prettyPrint);
        }
    }
}
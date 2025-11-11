using Appointix.ApplicationLayer;
using Appointix.Domain;
using Appointix.UI; // Importa il namespace dove hai AppointmentItem e IAppointmentPool
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controllore per la scena del paziente.
/// Implementa IAppointmentPool per gestire la creazione
/// e il riutilizzo dei prefab degli appuntamenti.
/// </summary>
public class PatientAppointmentController : MonoBehaviour, IAppointmentPool
{
    [Header("Riferimenti UI")]
    [SerializeField] private GameObject appointmentRowPrefab; // Il tuo prefab 'AppointmentItem'
    [SerializeField] private Transform container; // L'area grigia (Vertical Layout Group)
    [SerializeField] private TextMeshProUGUI patientNameText; // (Opzionale) Testo "Benvenuto"

    /// <summary>
    /// Evento scatenato quando la lista deve essere pulita.
    /// Richiesto da IAppointmentPool.
    /// </summary>
    public event Action OnListClean;

    // Lista che funge da "pool" per gli oggetti UI riutilizzabili
    private List<AppointmentItem> itemPool = new List<AppointmentItem>();

    /// <summary>
    /// Metodo Start di Unity.
    /// Si iscrive agli eventi e richiede i dati all'AppContext.
    /// </summary>
    void Start()
    {
        // 1. Controlla che AppContext sia pronto
        if (Appointix.ApplicationLayer.AppContext.Instance == null || Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager == null)
        {
            Debug.LogError("PatientAppointmentController: AppContext non pronto!");
            return;
        }

        // 2. Iscriviti all'evento per ricevere i dati
        Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager.OnAppointmentsLoaded += HandleAppointmentsLoaded;

        // 3. (Opzionale) Popola il nome del paziente
        if (patientNameText != null && Appointix.ApplicationLayer.AppContext.Instance.LoggedInPatient != null)
        {
            patientNameText.text = $"I TUOI APPUNTAMENTI, {Appointix.ApplicationLayer.AppContext.Instance.LoggedInPatient.nome.ToUpper()}:";
        }
        else if (patientNameText != null)
        {
            patientNameText.text = "I TUOI APPUNTAMENTI:";
        }

        // 4. Richiedi gli appuntamenti per l'ID paziente salvato
        int patientId = Appointix.ApplicationLayer.AppContext.Instance.userID;
        if (patientId > 0)
        {
            Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager.ReadAllByClient(patientId);
        }
        else
        {
            Debug.LogError("PatientAppointmentController: ID paziente non valido.");
        }
    }

    /// <summary>
    /// Chiamato da AppContext quando i dati degli appuntamenti sono arrivati.
    /// </summary>
    /// <param name="appointments">La lista di appuntamenti ricevuta.</param>
    private void HandleAppointmentsLoaded(List<Appointment> appointments)
    {
        // 1. Innesca l'evento per "pulire" la lista (disattiva tutti gli item usati)
        OnListClean?.Invoke();

        if (appointments.Count == 0)
        {
            Debug.Log("Nessun appuntamento trovato.");
            // (Qui puoi mostrare un messaggio "Nessun appuntamento")
            return;
        }

        // 2. Itera la lista e popola la UI
        foreach (var appt in appointments)
        {
            // Prendi un item dal pool (o creane uno nuovo)
            AppointmentItem item = GetFromPool();
            item.gameObject.SetActive(true); // Attivalo
            item.transform.SetAsLastSibling(); // Mettilo in fondo alla lista

            // Inizializzalo con i dati
            item.Setup(appt, this);
        }
    }

    /// <summary>
    /// Ottiene un AppointmentItem dal pool.
    /// Se non ci sono item disponibili, ne crea uno nuovo.
    /// </summary>
    /// <returns>Un AppointmentItem pronto per l'uso.</returns>
    private AppointmentItem GetFromPool()
    {
        // Cerca un oggetto disattivo nel pool da riutilizzare
        foreach (var item in itemPool)
        {
            if (!item.gameObject.activeSelf)
            {
                return item;
            }
        }

        // Se non ne trova, ne crea uno nuovo
        GameObject newInstance = Instantiate(appointmentRowPrefab, container);
        AppointmentItem newItem = newInstance.GetComponent<AppointmentItem>();
        itemPool.Add(newItem); // Aggiungilo al pool per il futuro
        return newItem;
    }

    /// <summary>
    /// Metodo OnDestroy di Unity.
    /// Esegue la disiscrizione dagli eventi per evitare memory leak.
    /// </summary>
    private void OnDestroy()
    {
        // Disiscrizione OBBLIGATORIA
        if (Appointix.ApplicationLayer.AppContext.Instance != null && Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager != null)
        {
            Appointix.ApplicationLayer.AppContext.Instance.RepositoryManager.OnAppointmentsLoaded -= HandleAppointmentsLoaded;
        }
    }

    // ... (Il tuo metodo OnDestroy è qui) ...

    /// <summary>
    /// Metodo richiesto dall'interfaccia IAppointmentPool.
    /// Chiamato da un AppointmentItem quando vuole essere "rimesso" nel pool.
    /// </summary>
    /// <param name="item">L'item da rimettere nel pool.</param>
    public void Repool(AppointmentItem item)
    {
        // La tua logica di pooling (gestita da OnDisable/GetFromPool) 
        // non ha bisogno di questo, ma l'interfaccia lo richiede.
        // Possiamo lasciarlo vuoto o aggiungere un log.
        Debug.Log($"Repooling item: {item.name}");
        item.gameObject.SetActive(false); // La logica più semplice è disattivarlo.
    }
}



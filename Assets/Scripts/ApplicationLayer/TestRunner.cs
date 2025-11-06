using UnityEngine;
using Appointix.ApplicationLayer; // Assicurati che il namespace sia corretto

/// <summary>
/// Questo script avvia il test di connessione all'avvio del gioco.
/// </summary>
public class TestRunner : MonoBehaviour
{
    void Start()
    {
        // Attende 1 secondo per assicurarsi che i Singleton (Awake) siano pronti
        Invoke(nameof(RunTest), 1.0f); 
    }

    void RunTest()
    {
        Debug.Log("TestRunner: Avvio del test di connessione tramite AppContext...");
        AppContext.Instance.TryConnectionToDB();

        // AGGIUNGI QUESTA COROUTINE DI TEST
        StartCoroutine(TestReadPatient());
    }

    // AGGIUNGI QUESTO METODO
    private System.Collections.IEnumerator TestReadPatient()
    {
        // Aspetta 3 secondi per dare tempo al test di connessione di fallire
        yield return new WaitForSeconds(3.0f); 
        
        Debug.Log("TestRunner: Eseguo test di lettura sul repository scelto...");
        
        // Ora il RepositoryManager (quello finto) è impostato.
        // Questa chiamata dovrebbe attivare il "Lazy Loading".
        AppContext.Instance.RepositoryManager.ReadPatient(1); 
    }
}
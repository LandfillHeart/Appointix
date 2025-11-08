using Appointix.ApplicationLayer;
using UnityEngine;

public class SelectRepoOnStartup : MonoBehaviour
{
	[SerializeField] private GameObject loginPanel;
	[SerializeField] private GameObject confirmNoPersistancePanel;

	private void Start()
	{
		AppContext.Instance.OnRepositoryManagerSet += ShowPanelContext;
		AppContext.Instance.TryConnectionToDB();
	}

	private void ShowPanelContext(IRepositoryManager repoManager)
	{
		if (repoManager is EndpointConnectionManager)
		{
			loginPanel.SetActive(true);
			return;
		}

		confirmNoPersistancePanel.SetActive(true);
	}

	public void LoginNoPersistance()
	{
		loginPanel.SetActive(true);
	}

	public void QuitApp()
	{
		// application quit è ignorato in editor, quindi non ti preoccupare se non succede nulla
		Application.Quit();
		Debug.Log("Cliccato il bottone per quittare l'app...");
	}
	
}

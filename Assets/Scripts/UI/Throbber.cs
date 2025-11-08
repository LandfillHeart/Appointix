using UnityEngine;

public class Throbber : MonoBehaviour
{
	[SerializeField] private float rotSpeed = 1f;

	private RectTransform throbberTransform;
	private Vector3 rotCache = Vector3.zero;

	private void Awake()
	{
		rotCache = new Vector3(0, 0, rotSpeed);
		throbberTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		throbberTransform.rotation = Quaternion.Euler(throbberTransform.rotation.eulerAngles + rotCache);
	}
}

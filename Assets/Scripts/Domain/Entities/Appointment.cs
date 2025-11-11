using System;

[Serializable]
public class Appointment
{
	public int id;
	public int idPaziente;
	public int idDottore;
	public DateTime inizioApp;
	public DateTime fineApp;

	public string nomeDottore;
	public string cognomeDottore;
	public string specDottore;
}
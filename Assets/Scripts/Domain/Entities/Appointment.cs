using System;

[Serializable]
public class Appointment
{
	public int ID;
	public int FK_Doctor_ID;
	public int FK_Patient_ID;
	public DateTime StartDateTime;
	public DateTime EndDateTime;
}
using System;

public class Appointment
{
	public int ID { get; private set; }
	public int FK_Doctor_ID { get; private set; }
	public int FK_Patient_ID { get; private set; }
	public DateTime StartDateTime { get; private set; }
	public DateTime EndDateTime { get; private set; }
}
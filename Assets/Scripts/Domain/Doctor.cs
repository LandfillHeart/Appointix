using System;

public class Doctor
{
	public int ID { get; private set; }
	public string Name { get; private set; }
	public string Surname { get; private set; }
	public string Specialization { get; private set; }
	public string Email { get; private set; }
	public string PhoneNumber { get; private set; }
	public string City { get; private set; }
	public int AppointmentDurationInMinutes { get; private set; }
	public string WeekDaysAvailable { get; private set; }
	public TimeSpan AvailableHours { get; private set; }
}
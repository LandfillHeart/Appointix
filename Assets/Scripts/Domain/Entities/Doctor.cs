using System;

[Serializable]
public class Doctor
{
	public int ID;
	public string Name;
	public string Surname;
	public string Specialization;
	public string Email;
	public string PhoneNumber;
	public string City;
	public int AppointmentDurationInMinutes;
	public string WeekDaysAvailable;
	public TimeSpan InHours;
	public TimeSpan FnHours;
}
using System;

[Serializable]
public class Doctor
{
	public int id;
	public string nome;
	public string cognome;
	public string specializzazione;
	public string email;
	public string telefono;
	public string citta;
	public int durata;
	public string giorniDisponibili;
	public TimeSpan orarioInizio;
	public TimeSpan orarioFine;
}
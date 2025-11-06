using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Appointix.ApplicationLayer
{

	/// <summary>
	/// Utility Class which holds all the functions to retrieve data from json text files and convert objects into json
	/// </summary>
	public static class JsonHelper
	{
		public static List<Patient> GetPatientsFromJson(string json)
		{
			string wrappedJson = "{\"patients\":" + json + "}";
			return JsonUtility.FromJson<PatientsListWrapper>(wrappedJson).ToList();
		}
	}
}
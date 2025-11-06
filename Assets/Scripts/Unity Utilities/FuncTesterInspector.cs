using UnityEditor;
using UnityEngine;

namespace Appointix.UnityUtilities
{
	[CustomEditor(typeof(FuncTester))]
	public class FuncTesterInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			FuncTester scriptReference = (FuncTester)target;
			if(GUILayout.Button("Test DB Connection"))
			{
				scriptReference.AppContextGetConnectionDB();
			}

			if (GUILayout.Button("Patients from Repo"))
			{
				scriptReference.ReadPatientsFromRepo();
			}
		}
	}
}
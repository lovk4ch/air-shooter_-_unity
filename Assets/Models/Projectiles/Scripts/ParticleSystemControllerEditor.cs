namespace GAP_ParticleSystemController{
	using UnityEngine;

	#if UNITY_EDITOR
	using UnityEditor;

	[CustomEditor(typeof(ParticleSystemController))]
	public class ParticleSystemControllerEditor : Editor
	{
		[System.Obsolete]
		#pragma warning disable CS0809 // Устаревший член переопределяет неустаревший член
		public override void OnInspectorGUI()
		#pragma warning restore CS0809 // Устаревший член переопределяет неустаревший член
		{
			DrawDefaultInspector();

			ParticleSystemController psCtrl = (ParticleSystemController)target;

			if (GUILayout.Button ("Fill Lists")) 
			{
				psCtrl.FillLists ();
			}
			if (GUILayout.Button ("Empty Lists")) 
			{
				psCtrl.EmptyLists ();
			}
			if(GUILayout.Button("Apply"))
			{
				psCtrl.UpdateParticleSystem();
			}
			if(GUILayout.Button("Reset"))
			{
				psCtrl.ResetParticleSystem();
			}
		}
	}
	#endif
}

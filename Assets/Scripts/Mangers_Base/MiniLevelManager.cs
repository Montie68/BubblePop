using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public abstract class MiniLevelManager : SerializedMonoBehaviour
{
	#region Public
	//public members go here
	//[SerializeField, LabelText("Distance to Player"), Range(0.3f, 1f)]
	//protected float _distToPlyr = 0.6f;

	#endregion

	#region Private
	//private members go here
	#endregion
	#region DebugVariables
	// debug variables go here

	#endregion
	// Place all unity Message Methods here like OnCollision, Update, Start ect. 
	#region Unity Messages 

	#endregion
	#region Public Methods
	//Place your public methods here
	public abstract Dictionary<string, GameObject> GetLevelList();
	public virtual void SetLevel(GameObject obj) { }
	#endregion
	#region Private Methods
	//Place your public methods here


	#endregion
}

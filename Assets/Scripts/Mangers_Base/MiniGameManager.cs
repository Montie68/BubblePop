using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;


public abstract class MiniGameManager : SerializedMonoBehaviour
{
	#region Public
	//public members go here
	public MiniLevelManager _levelManager;

	public delegate void OnLevelLoaded(GameObject currentLevel);

	protected Dictionary<string, GameObject> _levels;
	protected List<string> _levelNames;
	protected List<GameObject> _levelPrefabs;

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
	public virtual void GetGameLevels() => _levels =  _levelManager.GetLevelList();

	protected abstract bool SplitDictonary();
	#endregion
	#region Private Methods
	//Place your public methods here


	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace AddLifeMatch3
{

	public class Match3GameManger : MiniGameManager
	{
		#region Public
		//public members go here
		public static event OnLevelLoaded onLevelLoaded;

		#endregion

		#region Private
		//private members go here

		private int _currentLevel = 0;
		#endregion
		#region DebugVariables
		// debug variables go here

		#endregion
		// Place all unity Message Methods here like OnCollision, Update, Start ect. 
		#region Unity Messages 
		void Awake()
		{
			_levels = new Dictionary<string, GameObject>();
			_levelNames = new List<string>();
			_levelPrefabs = new List<GameObject>();
		}
		// Start is called before the first frame update
		void Start()
		{
			_levels = _levelManager.GetLevelList();

			StartCoroutine(LoadLevel());
		}

		private IEnumerator LoadLevel()
		{
			if (_levels.Count == 0)
			{
				Debug.LogError("No Levels Attacheched to the LevelManager");
				goto LevelLoadEnd;
			}

			yield return new WaitUntil(() => SplitDictonary());

			SpawnLevel(_currentLevel++);
		LevelLoadEnd:
			yield return null;
		}

		[Button("Load Level")]
		private void SpawnLevel(int index = 0)
		{
			// Load First Level
			GameObject level = Instantiate(_levelPrefabs[index], _levelPrefabs[index].transform.position + _levelManager.transform.position, _levelPrefabs[index].transform.rotation, _levelManager.transform);

			//set the Title
			var titles = level.GCsC<TMPro.TextMeshPro>();
			foreach (var t in titles)
			{
				if (t.tag == "MiniGameLevelTitle")
				{
					t.text = _levelNames[index];
				}
			}
			//_levelManager.SetBoard(level);
			onLevelLoaded?.Invoke(level);
		}

		// Update is called once per frame
		void Update()
		{

		}
		#endregion
		#region Public Methods
		//Place your public methods here
		protected override bool SplitDictonary()
		{
			try
			{
				foreach (var n in _levels.Keys)
				{
					_levelNames.Add(n);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
				return false;
			}
			try
			{
				foreach (var v in _levels.Values)
				{
					_levelPrefabs.Add(v);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogException(e);
				return false;
			}
			return true;
		}
		#endregion
		#region Private Methods
		//Place your public methods here


		#endregion
	}
}
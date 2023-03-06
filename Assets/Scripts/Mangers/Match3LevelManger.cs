using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace AddLifeMatch3
{
	public class Match3LevelManger : MiniLevelManager
	{
		#region Public
		//public members go here
		public delegate void OnSelectedTile(Collectable tile, bool selected = false);
		public static event OnSelectedTile onSelectedTile;

		public delegate void OnRemoveTiles(List<GameObject> gameObj);
		public static event OnRemoveTiles onRemoveTiles;
		#endregion

		#region Private
		//private members go here
		[SerializeField,DictionaryDrawerSettings(KeyLabel = "Level Name", ValueLabel = "Level Prefab")]
		private Dictionary<string, GameObject> _levels;
		[SerializeField, InfoBox("Timer the hand has to leave before removing Tiles/Pieces"), LabelText("Timer")]
		private float _leaveTimer = 1.5f;
		private float _currentCountDown = 0;

		private BoardController _currentLevel;
		private List<GameObject> _selectedTiles = new List<GameObject>();
		List<Collectables> _board = new List<Collectables>();

		#endregion
		#region DebugVariables
		// debug variables go here

		#endregion
		// Place all unity Message Methods here like OnCollision, Update, Start ect. 
		#region Unity Messages 
		// Start is called before the first frame update
		private void Start()
		{
			// OVR ONLY
			//Transform camPos = Camera.main.transform;
			//Vector3 boardPos = transform.position;
			//transform.forward = camPos.forward;
			//var dist = camPos.position - boardPos;

			//transform.localPosition = new Vector3(camPos.localPosition.x, camPos.localPosition.y / 2, camPos.localPosition.z + _distToPlyr);

		}
		private void OnEnable()
		{
			Collectable.selected += TileSelected;
			Collectable.onLeave += OnHandLeave;
			Match3GameManger.onLevelLoaded += SetBoard;

		}

		private void OnDisable()
		{
			Collectable.selected -= TileSelected;
			Collectable.onLeave -= OnHandLeave;
			Match3GameManger.onLevelLoaded -= SetBoard;

		}


		// Update is called once per frame
		void FixedUpdate()
		{
			if (_currentLevel == null) return;

			_board = _board != _currentLevel.GetObjectList() ? _currentLevel.GetObjectList() : _board;

		}
		#endregion
		#region Public Methods
		//Place your public methods here
		public override Dictionary<string, GameObject> GetLevelList() => _levels;

		public void SetBoard(GameObject obj)
		{
			SetLevel(obj);
			_board = _currentLevel.GetObjectList();
		}
		public override void SetLevel(GameObject obj) => _currentLevel = obj.GetComponent<BoardController>();

		#endregion
		#region Private Methods
		//Place your public methods here
		private void TileSelected(Collectable tile)
		{
			
			if (_selectedTiles.Count > 0 && tile.name != _selectedTiles[0].name) {

					return;
			}
			 
			if (_selectedTiles.Contains(tile.gameObject)) {
				if (_selectedTiles.LastIndex() == 0) return;
				if (_selectedTiles.SecondLastElement() == tile.gameObject )
				{
					StartCountDown();
					RemoveSelectedTile(_selectedTiles.LastElement().Collected());
				}

				return;
			}
			if (!TileIsAjacent(tile.GetData()))
			{
				return;
			}
			StartCountDown();
			onSelectedTile?.Invoke(tile, true);
			_selectedTiles.Add(tile.gameObject);
		}

		private void RemoveSelectedTile(Collectable tile)
		{
			onSelectedTile?.Invoke(tile);
			if (_selectedTiles.Count > 1) _selectedTiles.Remove(tile.gameObject);
			else _selectedTiles = new List<GameObject>();
		}

		private bool TileIsAjacent(Collectables data)
		{
			if (_selectedTiles.Count == 0) return true;
			if (_board.Contains(data))
			{
				// create list of the cardinal directions coordinates from the data position include diagonals
				List<Vector2> directions = new List<Vector2>() {
					new Vector2(data.position.x + 1, data.position.y),
					new Vector2(data.position.x - 1, data.position.y),
					new Vector2(data.position.x, data.position.y + 1),
					new Vector2(data.position.x, data.position.y - 1),
					new Vector2(data.position.x + 1, data.position.y + 1),
					new Vector2(data.position.x - 1, data.position.y - 1),
					new Vector2(data.position.x + 1, data.position.y - 1),
					new Vector2(data.position.x - 1, data.position.y + 1)
				};
				// check if data's position is on the side, top or bottom of the board
				bool isLeft = data.position.x == 0;
				bool isRight = data.position.x == _currentLevel.GetBoardSize().x - 1;
				bool isBottom = data.position.y == 0;
				bool isTop = data.position.y == _currentLevel.GetBoardSize().y - 1;

				// remove the directions that are not on the board
				if (isRight)
				{
					directions.Remove(new Vector2(data.position.x + 1, data.position.y));
					directions.Remove(new Vector2(data.position.x + 1, data.position.y-1));
					directions.Remove(new Vector2(data.position.x + 1, data.position.y+1));
 				}
				if (isLeft)
				{
					directions.Remove(new Vector2(data.position.x - 1, data.position.y));
					directions.Remove(new Vector2(data.position.x - 1, data.position.y+1));
					directions.Remove(new Vector2(data.position.x - 1, data.position.y-1));
				}
				if (isBottom)
				{
					directions.Remove(new Vector2(data.position.x, data.position.y - 1));
					directions.Remove(new Vector2(data.position.x + 1, data.position.y - 1));
					directions.Remove(new Vector2(data.position.x - 1, data.position.y - 1));
				}
				if (isTop)
				{
					directions.Remove(new Vector2(data.position.x, data.position.y + 1));
					directions.Remove(new Vector2(data.position.x + 1, data.position.y + 1));
					directions.Remove(new Vector2(data.position.x - 1, data.position.y + 1));
				}
				// check if the tile is ajacent to the selected tiles is the same name
				if (directions.Contains(_selectedTiles.LastElement().Collected().GetData().position))
				{
					return true;
				}
			}
			return false;
		}
		private void OnHandLeave(bool leave, Collectable tile)
		{
			if (tile != null && _selectedTiles.Count == 1)
			{
				StartCountDown();
				StartCoroutine(RemoveFirstElemant(tile));
				return;
			}

			StartCoroutine(RemoveTiles());
		}
		private void StartCountDown()
		{
			_currentCountDown = Time.time + _leaveTimer;
		}

		private IEnumerator RemoveFirstElemant(Collectable tile)
		{
			while (Time.time < _currentCountDown)
			{
				if (_selectedTiles.Count > 1) goto endFunc;
				yield return new WaitForFixedUpdate();
			}

			RemoveSelectedTile(tile);
		endFunc:
			yield return null;
		}

		bool _isCountDownOn = false;
		private IEnumerator RemoveTiles()
		{
			if (_isCountDownOn) goto endFunc;
			_isCountDownOn = true;
			StartCountDown();
			while (Time.time < _currentCountDown)
			{ 
				yield return new WaitForFixedUpdate();
			}
			TileRemover();

		endFunc:
			_isCountDownOn = Time.time < _currentCountDown;
		}

		private void TileRemover()
		{
			if (_selectedTiles.Count < 3)
			{
				for (int i = 0; i < _selectedTiles.Count; i++) {
					onSelectedTile?.Invoke(_selectedTiles[i].Collected());
				}
				_selectedTiles = new List<GameObject>();

				return;
			}
			_currentLevel.RemoveCollectables(_selectedTiles);
			onRemoveTiles?.Invoke(_selectedTiles);
			_selectedTiles = new List<GameObject>();
			_isCountDownOn = false;
		}
		#endregion
	}
}
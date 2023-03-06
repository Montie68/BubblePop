using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
using System.Threading;
using UnityEditor;

namespace AddLifeMatch3
{

	[HideMonoScript]
	public class BoardController : SerializedMonoBehaviour
	{
		#region Public
		//public members go here


		#endregion

		#region Private
		//private members go here
		[SerializeField, PropertyOrder(1)]
		private Vector2I _boardSize = new Vector2I(10, 10);
		[SerializeField, PropertyOrder(4)] 
		List<GameObject> _collectObjects = new List<GameObject>();
		[SerializeField, Required, PropertyOrder(2)]
		private CollectableSpawner _spawner;
		[SerializeField, Required, PropertyOrder(3)]
		private GameObject _boardParent;

		[SerializeField, InfoBox("Deley between row spawning in Milliseconds"), PropertyOrder(5)]
		float _rowDeley = 0;
		[SerializeField, InfoBox("Deley between column spawning in Milliseconds"), PropertyOrder(6)]
		float _columnDeley = 0;
		private List<Collectables> _collectables = new List<Collectables>();

		#endregion
		#region DebugVariables
		// debug variables go here


		#endregion
		// Place all unity Message Methods here like OnCollision, Update, Start ect. 
		#region Unity Messages 
		// Start is called before the first frame update
		void Start()
		{
			//convert Deleys to seconds
			_columnDeley /= 1000;
			_rowDeley /= 1000;
			InitalizeBoard();
		}
		private void OnEnable()
		{
			Match3LevelManger.onSelectedTile += UpdateBoardList;
		}
		private void OnDisable()
		{
			Match3LevelManger.onSelectedTile -= UpdateBoardList;
		}
		// Update is called once per frame
		//void Update()
		//{

		//}
		#endregion
		#region Public Methods
		//Place your public methods here
		public  List<Collectables> GetObjectList() =>  _collectables;

		public Vector2 GetBoardSize() => _boardSize;

		public void RemoveCollectables(List<GameObject> gameObjs)
		{
			foreach (var g in gameObjs)
			{
				RemoveObject(g);
			}
			FillGaps();
		}
		#endregion
		#region Private Methods
		//Place your public methods here
		private void RemoveObject(GameObject obj)
		{
			_collectables.Remove( _collectables.Find(col => col.position == obj.Collected().GetLocation()));
			Destroy(obj);		

		}
		private void InitalizeBoard()
		{
			CentreBoard();

			for (int y = 0; y < _boardSize.y; y++)
			{
				for (int x = 0; x < _boardSize.x; x++)
				{
					_collectables.Add(new Collectables(new Vector2(x, y), _collectObjects[Random.Range(0, _collectObjects.Count)]));
				}
			}

			StartCoroutine(PlaceObjects());
		}

		private void CentreBoard()
		{
			// Centre Board on the X axis
			float mover = _boardSize.x % 2 == 0 ? 0.5f : 0f;
			_boardParent.transform.localPosition = new Vector3((
				(transform.localEulerAngles.y == 0 ? -1 : 1) * _boardSize.x / 2) - mover, 
				_boardParent.transform.localPosition.y,
				_boardParent.transform.localPosition.z);
			_spawner.transform.localPosition = new Vector3((
				(_boardParent.transform.localEulerAngles.y == 0 ? -1 : 1) * _boardSize.x / 2) - mover,
				_spawner.transform.localPosition.y,
				_spawner.transform.localPosition.z);
		}

		private IEnumerator PlaceObjects()
		{
			yield return new WaitForSeconds(1);

			int b = 0;
			for (int y = 0; y < _boardSize.y; y++)
			{
				for (int x = 0; x < _boardSize.x; x++)
				{
					GameObject obj = InstantiateObject(x, _spawner.transform.localPosition.y, Tile: _collectables[b].gameObject);

					_collectables[b].SetGameObject(obj);
					obj.Collected().SetCollectable(_collectables[b]);
					b++;
					//yield return new WaitForSeconds(_columnDeley);
				}
				//yield return new WaitForSeconds(_rowDeley );
			}
			yield return null;
		}
		private GameObject InstantiateObject(float x, float y, GameObject Tile = null, bool isEditor = false)
		{
			Tile = isEditor || Tile != null ? _collectObjects[Random.Range(0, _collectObjects.Count)] : Tile;
			GameObject obj = Instantiate(Tile,
							Vector3.zero,
							Quaternion.identity, _boardParent.transform);
			obj.transform.localPosition = new Vector3(x, y, 0);
			
			obj.name = obj.name.Split("(Clone)")[0];

			return obj;
		}
		private void UpdateBoardList(Collectable tile, bool selected)
		{
			_collectables.Find(col => col.position == tile.GetLocation()).SetSelected(selected);
		}



		private void FillGaps()
		{
			// Find the gaps in the board.
				List<Vector2I> gaps = new List<Vector2I>();
				for (int y = 0; y < _boardSize.y; y++)
				{
					for (int x = 0; x < _boardSize.x; x++)
					{
						Vector2 pos = new Vector2(x, y);

						if (_collectables.FindAll(i => i.position == new Vector2(x, y)).Count == 0)
						{
							List<Collectables> colls = new List<Collectables>();

							colls = _collectables.FindAll(i => i.position.x == x && i.position.y > y);
							if (colls.Count == 0)
							{
								gaps.Add(pos);
								continue;
							}
							// Wrong Set Top element in _collectables
							_collectables[_collectables.IndexOf(colls.FirstElement())] = new Collectables(pos, colls.FirstElement().gameObject);
							var obj = FindObjectsOfType<Collectable>().ToList().Find(i => i.GetLocation() == colls.FirstElement().position);
							obj.SetLocation(pos);
							obj.SetMoving(0.25f);
							obj.SetTweenPath();
						} 
					}
				}

				// Fill the gaps.
				foreach (var gap in gaps)
				{
					if (_spawner == null)
					{
						_spawner = gameObject.GetComponentInChildren<CollectableSpawner>();
					}
 					_collectables.Add(new Collectables(gap, _collectObjects[Random.Range(0, _collectObjects.Count)]));
					GameObject obj = InstantiateObject(gap.x, _spawner.transform.localPosition.y, _collectables.LastElement().gameObject);
					_collectables.LastElement().SetGameObject(_collectables.LastElement().gameObject);
					obj.Collected().SetCollectable(_collectables.LastElement());
					obj.Collected().SetLocation(gap);
					obj.Collected().SetMoving(0.25f);
				}
			// todo re-order _collectables
			
		}
		#endregion
		#region OdinEditor Methods
#if UNITY_EDITOR
		[Button("Generate", ButtonSizes.Large), GUIColor(0, 1, 0), HideInPlayMode,  PropertyOrder(20)]
		private void Generate()
		{
			CentreBoard();
			int b = 0;
			for (int y = 0; y < _boardSize.y; y++)
			{
				for (int x = 0; x < _boardSize.x; x++)
				{
					var obj = InstantiateObject(x, y, isEditor: true);
					obj.Collected().SetLocation(new Vector2(x, y));
					obj.GC<MeshRenderer>().enabled = true;
					b++;
				}
			}
		}


		[Button("Remove All", ButtonSizes.Large), GUIColor(0.9f, 0, 0), HideInPlayMode, PropertyOrder(21)]
		public void RemoveCollectables()
		{
			var objs = _boardParent.GCsC<Collectable>();

			foreach (var c in objs)
			{
				DestroyImmediate(c.gameObject);
			}
		}



#endif
		#endregion
	}
}

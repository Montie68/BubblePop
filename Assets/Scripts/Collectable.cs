using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System;

namespace AddLifeMatch3
{
	public class Collectable : MonoBehaviour
	{
		#region Public
		//public members go here
		protected bool isMoving = false;
		// selection events
		public delegate void Selected(Collectable gameObj);
		public static event Selected selected;

		public delegate void OnLeave(bool leave = true, Collectable gameObj = null);
		public static event OnLeave onLeave;
		#endregion

		#region Private
		//private members go here
		private Vector2 _location;
		[SerializeField]
		private float _fallDuration = 1;
		[SerializeField]
		private Ease _easeStyle = Ease.InElastic;
		[SerializeField, Required]
		private MeshRenderer _selectedMeshRender;
		[SerializeField, LabelText("Piece data")]
		private Collectables _pieceData;
		private Rigidbody _rigidbody;
		private Sequence movePath;
		private MeshRenderer _meshRenderer;

		private float _customInterval = -1;


		#endregion
		#region DebugVariables
		// debug variables go here

		#endregion
		// Place all unity Message Methods here like OnCollision, Update, Start ect. 
		#region Unity Messages 
		// Start is called before the first frame update
		void Start()
		{

			_rigidbody = GetComponent<Rigidbody>();
			_meshRenderer = GetComponent<MeshRenderer>();
			SetTweenPath(true);
		}
		private void OnEnable()
		{
			Match3LevelManger.onSelectedTile += UpdateCollectable;
			// for mouse input test
			Input.onSelected += SimOnTriggerEnter;
			Input.onLeave += SimOnTriggerExit;
		}

		private void OnDisable()
		{
			Match3LevelManger.onSelectedTile -= UpdateCollectable;
			// for mouse input test
			Input.onSelected -= SimOnTriggerEnter;
			Input.onLeave -= SimOnTriggerExit;
		}
		private void OnDestroy()
		{
			movePath.Kill();
		}
		// Update is called once per frame
		void FixedUpdate()
		{
			if (_location != _pieceData.position)
			{
				 _pieceData.SetPosition(_location);
			}
			if (!isMoving)
				return;

			if (transform.localPosition.y <= _location.y)
			{
				movePath.TogglePause();
				isMoving = false;
				transform.localPosition = _location;
				_customInterval = -1;
			}
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.tag != "Player")
			{
				if (other.tag == "Match3Spawner")
				{
					_meshRenderer.enabled = false;

				}
				return;
			}

			selected?.Invoke(this);
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.tag != "Player")
			{
				if (other.tag == "Match3Spawner")
				{
					_meshRenderer.enabled = true;
				}
				return;
			}

			onLeave?.Invoke(true, this);
		}
		#endregion
		#region Public Methods
		//Place your public methods here
		public void SetLocation(Vector2 loc) => _location = loc;

		public void SetMoving(float interval)
		{
			_customInterval = interval;
		}
		public void SetCollectable(Collectables data)
		{
			SetLocation(data.position);
			_pieceData = data;
		}
		public ref Collectables GetData() => ref _pieceData;
		public Vector2 GetLocation() => _location;
		public void SetTweenPath(bool fadeMaterial = false)
		{
			movePath = DOTween.Sequence();

			isMoving = true;
			
			var material = _meshRenderer.materials[0];
			if (material != null && fadeMaterial)
			{
				material.color = new Color(material.color.r, material.color.g, material.color.b, 0.0f);
				movePath.Append(material.DOFade(1f, _fallDuration * 3));
			}
			movePath.Append(transform.DOLocalMoveY(_location.y, _customInterval == -1 ? _fallDuration : _customInterval, true).SetEase(_easeStyle));
			movePath.Play();

		}
		#endregion
		#region Private Methods
		//Place your public methods here
		private void SimOnTriggerExit(Collider other, GameObject gameObj)
		{
			if (gameObj != gameObject)
			{
				return;
			}
			OnTriggerExit(other);
		}

		private void SimOnTriggerEnter(Collider other, GameObject gameObj)
		{
			if (gameObj != gameObject)
			{
				return;
			}
			OnTriggerEnter(other);
		}

		private void UpdateCollectable(Collectable tile, bool selected)
		{
			if (tile.gameObject != this.gameObject)
				return;

			_pieceData.SetSelected(selected);
			//	Debug.LogError(_pieceData.ToString());
			_selectedMeshRender.enabled = selected;
		}

		#endregion
	}
}
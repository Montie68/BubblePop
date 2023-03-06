using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using AddLifeMatch3;
public class Input : MonoBehaviour
{
	#region Public
	//public members go here
	public delegate void Selected(Collider other, GameObject gameObj);
	public delegate void OnLeave(Collider other, GameObject gameObj);

	public static event Selected onSelected;
	public static event OnLeave onLeave;

	#endregion

	#region Private
	//private members go here
	private Vector3 _mousePos;

	private float _mouseButtonValue = 0;
	private GameObject _lastCollection;
	private 
	bool _isHit = false;
    #endregion
    #region DebugVariables
    // debug variables go here

    #endregion
    // Place all unity Message Methods here like OnCollision, Update, Start ect. 
    #region Unity Messages 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 mousePos = (Vector3)Mouse.current.position.ReadValue();
		mousePos.z = 0.25f;
		_mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		_mouseButtonValue = Mouse.current.leftButton.ReadValue();
    }

	private void LateUpdate()
	{
		if (_mouseButtonValue > 0) {
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(_mousePos, Vector3.forward, out hit)) {
				if (hit.collider.gameObject.Collected() != null && !_isHit) {
					_isHit = !_isHit;
					_lastCollection = hit.collider.gameObject;
					onSelected?.Invoke(gameObject.GC<BoxCollider>(), _lastCollection);
				}
			} else if (_isHit) {
				onLeave?.Invoke(gameObject.GC<BoxCollider>(), _lastCollection);
				_isHit = !_isHit;
				_lastCollection = null;
			}
		}
	}
	#endregion
	#region Public Methods
	//Place your public methods here

	#endregion
	#region Private Methods
	//Place your public methods here


	#endregion
}

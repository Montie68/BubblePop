using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddLifeMatch3;

public class colorPicker : MonoBehaviour
{
	#region Public
	//public members go here

	#endregion

	#region Private
	//private members go here
	[SerializeField] ballColors _ballColor = ballColors.white;
	MeshRenderer _meshRenderer;

    #endregion
    #region DebugVariables
    // debug variables go here

    #endregion
    // Place all unity Message Methods here like OnCollision, Update, Start ect. 
    #region Unity Messages 
    // Start is called before the first frame update
    void Start()
    {
		_meshRenderer = gameObject.GC<MeshRenderer>();
		 Color ballColor;
		var value = (int)_ballColor;
		string col = "#" + value.ToString("X");
		  ColorUtility.TryParseHtmlString(col, out ballColor);
		_meshRenderer.material.color = ballColor;

	}

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
    #region Public Methods
    //Place your public methods here

    #endregion
    #region Private Methods
    //Place your public methods here
 
    
    #endregion
}

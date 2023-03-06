using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreManger : MonoBehaviour
{
	#region Public
	//public members go here

	#endregion

	#region Private
	//private members go here
	private object _textField;
	private int _score = 0;
	string _orignalText;
	#endregion
	#region DebugVariables
	// debug variables go here

	#endregion
	// Place all unity Message Methods here like OnCollision, Update, Start ect. 
	#region Unity Messages 
	// Start is called before the first frame update
	void Start()
	{
		if (_textField == null) { 
			var scoreBox = GameObject.FindGameObjectWithTag("ScoreUI");
			_textField = scoreBox.GetComponent<TextMeshProUGUI>() ?? null;
			_textField ??= scoreBox.GetComponent<Text>();
		}
		AddScore(new List<GameObject>());
		AddLifeMatch3.Match3LevelManger.onRemoveTiles += AddScore;

	}

	private void OnDestroy()
	{
		AddLifeMatch3.Match3LevelManger.onRemoveTiles -= AddScore;
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

	private void AddScore(List<GameObject> gameObj)
	{
		var total = gameObj.Count == 0 
			? 0 
			: Mathf.Log10(gameObj.Count) * 100 + gameObj.Count;

		_score += (int)total;
		
		if (_textField.GetType() == typeof(Text))
		{
			Text temp = (Text)_textField;
			if (_orignalText == null) _orignalText = temp.text;
			temp.text = string.Format(_orignalText, _score);
		}
		else if (_textField.GetType() == typeof(TextMeshProUGUI))
		{
			TextMeshProUGUI temp = (TextMeshProUGUI)_textField;
			if (_orignalText == null) _orignalText = temp.text;
			temp.text = string.Format(_orignalText, _score);
		}
	}
	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneTransitioner : MonoBehaviour
{
	//! --------parameters--------
	[SerializeField]
	string _nextScene;

	//! --------internal variables--------
	bool _transitioned;

	//! --------functions--------
	void Update()
	{
		if(Input.anyKey&&!_transitioned)
		{
			SceneManager.LoadScene(_nextScene);
			_transitioned = true;
		}
	}
}

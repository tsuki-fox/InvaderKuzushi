using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySceneInitializer : MonoBehaviour
{
	[SerializeField]
	AudioClip _mainBGM;

	AudioSource _audioSource;

	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		_audioSource.clip = _mainBGM;
		_audioSource.Play();	
	}
}

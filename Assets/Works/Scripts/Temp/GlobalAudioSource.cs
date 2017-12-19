using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GlobalAudioSource : MonoBehaviour
{
	static GlobalAudioSource _self = null;

	AudioSource _source;

	public static void PlayOneShot(AudioClip clip)
	{
		if(_self==null)
		{
			var go = new GameObject();
			go.AddComponent<GlobalAudioSource>();
			_self = go.GetComponent<GlobalAudioSource>();
		}
		_self._source.PlayOneShot(clip);
	}

	void Awake()
	{
		_source = GetComponent<AudioSource>();	
	}
}

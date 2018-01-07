using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Blink : MonoBehaviour
{
	[SerializeField]
	float _interval = 1f;

	void Awake()
	{
		Caller.Interval(_interval).Subscribe(_ =>
		{
			gameObject.SetActive(!gameObject.activeSelf);
		});
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLevelManager : MonoBehaviour
{
	//! --------parameter--------
	[SerializeField]
	int _maxLevel = 3;
	[SerializeField]
	List<Sprite> _spriteByLevel;
	[SerializeField]
	SpriteRenderer _renderTarget;
	[SerializeField]
	float _showDue = 1f;
	[SerializeField]
	GameObject _fullChargeVFX;
	[SerializeField]
	AudioClip _fullChargeSE;

	//! --------internal variables--------
	int _level;

	//! --------properties--------
	public int level
	{
		get { return _level; }
		set
		{
			int next = Mathf.Clamp(value, 0, _maxLevel);
			_renderTarget.sprite = _spriteByLevel[next];

			_renderTarget.SetAlpha(1f);
			_renderTarget.Delay(_showDue).SetAlpha(0f);

			//フルチャージエフェクトON/OFF
			if (next == _maxLevel)
				_fullChargeVFX.SetActive(true);
			else
				_fullChargeVFX.SetActive(false);

			if (next == _maxLevel && next != _level)
				GlobalAudioSource.PlayOneShot(_fullChargeSE);

			_level = next;
		}
	}
	public bool isFullCharge
	{
		get { return _level == _maxLevel; }
	}
	//! --------functions--------
	void Initialize()
	{
		_level = 0;
		_renderTarget.color.SetAlpha(0f);
	}
}

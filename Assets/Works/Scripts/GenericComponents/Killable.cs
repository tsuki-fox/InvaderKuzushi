using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Generals/Killable")]
public class Killable : MonoBehaviour
{
	public delegate void OnDamageHandler(Killable self);
	public delegate void OnRecoveryHandler(Killable self);
	public delegate void OnStraddle(Killable self);

	/// <summary>ダメージイベント</summary>
	public event OnDamageHandler onDamage = delegate { };
	/// <summary>回復イベント</summary>
	public event OnRecoveryHandler onRecovery = delegate { };
	/// <summary>踏ん張りイベント</summary>
	public event OnStraddle onStraddle = delegate { };

	[SerializeField, Header("最大へルス")]
	float _maxHealth = 100;
	[Extractable]
	float _health;
	[SerializeField, Header("踏ん張り有効化")]
	bool _straddleEnabled = false;
	[SerializeField,Header("オブジェクトプールサポート有効化")]
	bool _objectPoolSupport = false;

	/// <summary>最大ヘルス</summary>
	public float maxHealth
	{
		get { return _maxHealth; }
		set { _maxHealth = value; }
	}
	/// <summary>現在ヘルス</summary>
	[SerializeField,Extractable]
	public float health
	{
		get { return _health; }
	}

	void Awake()
	{
		_health = _maxHealth;	
	}

	/// <summary>ダメージを受ける</summary>
	/// <param name="value">受ける量 : value>0 </param>
	public void TakeDamage(float value)
	{
		value = Mathf.Max(0, value);
		_health -= value;
		onDamage(this);
		if (_health <= 0)
		{
			if (_straddleEnabled && _health > 1)
			{
				_health = 0.01f;
				onStraddle(this);
			}
			else
			{
				if (_objectPoolSupport)
					TF.ObjectPool.Repay(gameObject);
				else
					Destroy(gameObject);
			}
		}
	}

	/// <summary>ヘルスを回復する</summary>
	/// <param name="value">回復する量 : value>0 </param>
	public void Recovery(float value)
	{
		value = Mathf.Max(0, value);
		_health += value;
		_health = Mathf.Clamp(_health, 0, _maxHealth);
		onRecovery(this);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BusNotifier : MonoBehaviour
{
	//! --------value declares--------
	[SerializeField]
	List<TagName> _enter;
	[SerializeField]
	List<TagName> _stay;
	[SerializeField]
	List<TagName> _exit;

	[SerializeField]
	bool _freeNotifiable = true;

	//! --------functions--------
	void OnCollisionEnter2D(Collision2D collision)
	{
		foreach (var tag in _enter.Where(item => collision.gameObject.GetEnumTagName() == item))
			CollisionBus.Notify(CollisionBus.Timing.Enter, gameObject, collision.gameObject, collision);
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		foreach (var tag in _stay.Where(item => collision.gameObject.GetEnumTagName() == item))
			CollisionBus.Notify(CollisionBus.Timing.Stay, gameObject, collision.gameObject, collision);
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		foreach (var tag in _exit.Where(item => collision.gameObject.GetEnumTagName() == item))
			CollisionBus.Notify(CollisionBus.Timing.Exit, gameObject, collision.gameObject, collision);
	}

	void OnFree()
	{
		if (_freeNotifiable)
			LifeCycleBus.Notify(gameObject);
	}
}

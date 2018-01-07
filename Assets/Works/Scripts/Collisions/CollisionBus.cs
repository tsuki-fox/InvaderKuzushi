using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Collisions
{
	public class CollisionBus : MonoBehaviour
	{
		//! ----type declares----
		public sealed class Combo
		{
			public TagName tagA;
			public TagName tagB;

			public Combo(TagName a,TagName b)
			{
				tagA = a;
				tagB = b;
			}
			public static bool operator ==(Combo lhs, Combo rhs)
			{
				return lhs.tagA == rhs.tagA
					&& lhs.tagB == rhs.tagB;
			}
			public static bool operator !=(Combo lhs, Combo rhs)
			{
				return lhs.tagA != rhs.tagA
					|| lhs.tagB != rhs.tagB;
			}
			public override bool Equals(object obj)
			{
				if (obj == null || GetType() != obj.GetType())
					return false;

				var arg = (Combo)obj;
				return this == arg;
			}
			public override int GetHashCode()
			{
				return tagA.GetHashCode() ^ tagB.GetHashCode();
			}
		}

		public sealed class Unsubscriber
		{
			Combo _combo;
			OnCollisionEnterHandler _handler;
			public Unsubscriber(Combo combo,OnCollisionEnterHandler handler)
			{
				_combo = combo;
				_handler = handler;
			}
			public void Unsubscribe()
			{
				CollisionBus.Unsubscribe(_combo, _handler);
			}
		}

		//! ----delegates----
		public delegate void OnCollisionEnterHandler(GameObject objA, GameObject objB, Collision2D collision);
		public delegate void OnNotifiedHandler(GameObject objA, GameObject objB);

		//! ----internal values----
		static Dictionary<Combo, OnCollisionEnterHandler> _handlers = new Dictionary<Combo, OnCollisionEnterHandler>();
		static OnNotifiedHandler onNotified = delegate { };

		//! ----functions----
		public static Unsubscriber Subscribe(Combo combo,OnCollisionEnterHandler handler)
		{
			if (_handlers.ContainsKey(combo))
				_handlers[combo] += handler;
			else
				_handlers.Add(combo, handler);
			return new Unsubscriber(combo, handler);
		}
		public static Unsubscriber Subscribe(TagName a,TagName b,OnCollisionEnterHandler handler)
		{
			var combo = new Combo(a, b);
			return Subscribe(combo, handler);
		}

		public static void Unsubscribe(Combo combo,OnCollisionEnterHandler handler)
		{
			if (_handlers.ContainsKey(combo))
				_handlers[combo] -= handler;
		}

		public static void Notify(GameObject objA,GameObject objB,Collision2D collision)
		{
			var combo = new Combo(objA.GetEnumTagName(), objB.GetEnumTagName());

			if (_handlers.ContainsKey(combo))
				_handlers[combo](objA, objB, collision);
			else
				_handlers.Add(combo, delegate { });

			onNotified(objA, objB);
		}

		public static void Clean()
		{
			_handlers = new Dictionary<Combo, OnCollisionEnterHandler>();
		}

		void Awake()
		{
			onNotified = delegate { };
			_handlers = new Dictionary<Combo, OnCollisionEnterHandler>();

			onNotified += (objA, objB) =>
			{
				var logger = GetComponent<Cores.Logger>();
				logger.Log("{0} : {1}", objA.tag, objB.tag);
			};
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TF
{
	public class ObjectPool : MonoBehaviour
	{
		//! --------classes--------
		/// <summary>
		/// プロファイル
		/// </summary>
		[System.Serializable]
		public class ObjectProfile
		{
			public GameObject _prefab;
			public int _reserveAmount;
#if UNITY_EDITOR
			public Texture2D _icon;
#endif
#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(ObjectProfile))]
			public class ObjectProfileDrawer : PropertyDrawer
			{
				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUI.BeginProperty(position, label, property);

					var prefabProp = property.FindPropertyRelative("_prefab");
					var amountProp = property.FindPropertyRelative("_reserveAmount");
					var icon = (Texture2D)property.FindPropertyRelative("_icon").objectReferenceValue;

					var iconRect = new Rect(position)
					{
						width = 32,
						height = 32
					};
					var prefabRect = new Rect(position)
					{
						x = position.x + iconRect.width,
						width = position.width - iconRect.width,
						height = position.height * 0.5f
					};
					var amountRect = new Rect(prefabRect)
					{
						x = position.x + iconRect.width,
						width = prefabRect.width,
						y = prefabRect.y + prefabRect.height
					};

					if (icon != null)
						EditorGUI.DrawTextureTransparent(iconRect, icon);
					else
						EditorGUI.LabelField(iconRect, "NULL");
					prefabProp.objectReferenceValue = EditorGUI.ObjectField(prefabRect, "Prefab", prefabProp.objectReferenceValue, typeof(GameObject), false);
					EditorGUI.BeginDisabledGroup(prefabProp.objectReferenceValue == null);
					EditorGUI.PropertyField(amountRect, amountProp);
					EditorGUI.EndDisabledGroup();

					EditorGUI.EndProperty();
					if (EditorGUI.EndChangeCheck())
					{
						if (prefabProp.objectReferenceValue != null)
							property.FindPropertyRelative("_icon").objectReferenceValue = AssetPreview.GetMiniThumbnail(prefabProp.objectReferenceValue);
						else
							property.FindPropertyRelative("_icon").objectReferenceValue = null;
					}
				}

				public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
				{
					return base.GetPropertyHeight(property, label) * 2f;
				}
			}
#endif
		}

		//! --------statics--------
		static Dictionary<string, List<GameObject>> _reserved = new Dictionary<string, List<GameObject>>();
		static GameObject _poolRoot = null;
		static Dictionary<string, GameObject> _groupRoots = new Dictionary<string, GameObject>();

		/// <summary>
		/// 初期化関数
		/// </summary>
		static void Initialize()
		{
			var pool = FindObjectOfType<ObjectPool>();
			if (pool == null)
				Debug.LogWarning("ObjectPoolインスタンスがシーンに存在しません");
			else
				_poolRoot = pool.gameObject;
		}
		/// <summary>初期化チェック</summary>
		/// <returns>初期化されているかどうか</returns>
		static bool CheckInitialize()
		{
			if (_poolRoot == null)
				Initialize();
			return _poolRoot != null ? true : false;
		}
		/// <summary>オブジェクトインスタンス生成</summary>
		/// <param name="source">元プレハブ</param>
		/// <returns>インスタンス</returns>
		static GameObject CreateInstance(GameObject source)
		{
			var instance = GameObject.Instantiate(source);
			instance.name = source.name + _reserved[source.name].Count;
			return instance;
		}
		/// <summary>オブジェクトグループに新規インスタンスを追加</summary>
		/// <param name="key">キー</param>
		static void AddObjectGroup(string key)
		{
			var objRoot = new GameObject(key);
			objRoot.transform.SetParent(_poolRoot.transform);
			_groupRoots.Add(key, objRoot);

			_reserved.Add(key, new List<GameObject>());
		}
		/// <summary>インスタンスを確保する</summary>
		/// <param name="prefab">参照元のプレハブ</param>
		/// <param name="reserveAmount">確保量</param>
		public static void Reserve(GameObject prefab, int reserveAmount)
		{
			CheckInitialize();

			if (!_groupRoots.ContainsKey(prefab.name))
				AddObjectGroup(prefab.name);

			for (int i = 0; i < reserveAmount; i++)
			{
				var obj = CreateInstance(prefab);
				obj.SetActive(false);
				obj.transform.SetParent(_groupRoots[prefab.name].transform);
				_reserved[prefab.name].Add(obj);
			}
		}
		/// <summary>インスタンスを借りる(Borrow)</summary>
		/// <param name="prefab">借りたいインスタンスの元プレハブ</param>
		/// <returns>インスタンス</returns>
		public static GameObject Borrow(GameObject prefab)
		{
			CheckInitialize();
			if (!_reserved.ContainsKey(prefab.name))
			{
				Debug.LogWarningFormat("Object not reserved! name:{0}", prefab.name);
				return null;
			}

			//待機中のオブジェクトを探して返す
			foreach (var item in _reserved[prefab.name])
			{
				if (!item.activeSelf)
				{
					item.SetActive(true);
					item.BroadcastMessage("OnBorrow", SendMessageOptions.DontRequireReceiver);
					return item;
				}
			}

			//全て使用中ならば新たに確保する
			Reserve(prefab, 1);
			return Borrow(prefab);
		}
		/// <summary>インスタンスを返済する</summary>
		/// <param name="instance">返済するインスタンス</param>
		public static void Repay(GameObject instance)
		{
			if (_poolRoot.IsChild(instance))
				Debug.LogWarningFormat("{0} isn't child of [ObjectPool]", instance.name);
			else
			{
				instance.BroadcastMessage("OnRepay", SendMessageOptions.DontRequireReceiver);
				instance.SetActive(false);
			}
		}

		//! --------instances--------
		[SerializeField]
		List<ObjectProfile> _profiles = new List<ObjectProfile>();

		/// <summary>
		/// シーンロード時に自動確保
		/// </summary>
		void ReserveOnLoad()
		{
			foreach (var prof in _profiles)
			{
				if (prof._prefab != null)
					ObjectPool.Reserve(prof._prefab, prof._reserveAmount);
			}
#if OBJECT_POOL_DEBUG
			Debug.LogFormat("ObjectPool@ReserveOnLoad()> Reserved:{0}", _profiles.Sum(item => item._reserveAmount));
#endif
		}
		/// <summary>
		/// プロファイルを追加
		/// </summary>
		void AddProfile()
		{
			_profiles.Add(new ObjectProfile());
		}
		void OnEnable()
		{
			ReserveOnLoad();	
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(ObjectPool))]
		public class ObjectPoolInspector : Editor
		{
			public override void OnInspectorGUI()
			{
				var self = target as ObjectPool;
				serializedObject.Update();

				EditorGUI.BeginChangeCheck();

				//追加ボタン
				if (GUILayout.Button("Add Profile"))
					self.AddProfile();

				//profile表示
				var remIndexes = new List<int>();
				var profiles = serializedObject.FindProperty("_profiles");
				for (var i = 0; i < profiles.arraySize; i++)
				{
					var prof = profiles.GetArrayElementAtIndex(i);

					EditorGUILayout.BeginHorizontal("box");
					EditorGUILayout.PropertyField(prof);
					MyGUIUtil.BeginBackgroundColorChange(Color.red);
					if (GUILayout.Button("X", GUILayout.Width(16), GUILayout.Height(32)))
						remIndexes.Add(i);
					MyGUIUtil.EndBackgroundColorChange();
					EditorGUILayout.EndHorizontal();
				}

				foreach (var index in remIndexes)
					profiles.DeleteArrayElementAtIndex(index);
				serializedObject.ApplyModifiedProperties();

				if(EditorGUI.EndChangeCheck())
					Undo.RecordObject(this, "ObjectPoolInspector Changed");
			}
		}
#endif
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using TF;
using Assets.Players;

namespace Assets.Managers
{
	public class SceneConductor : MonoBehaviour
	{
		//! ----types----
		public enum SceneState
		{
			Title,
			Play,
			Result,
		}

		//! ----delegates----
		public delegate void OnTitleSceneStartHandler();
		public delegate void OnTitleSceneEndHandler();
		public delegate void OnPlaySceneStartHandler();
		public delegate void OnPlaySceneEndHandler();
		public delegate void OnResultSceneStartHandler();
		public delegate void OnResultSceneEndHandler();

		//! ----events----
		public static event OnTitleSceneStartHandler onTitleSceneStart = delegate { };
		public static event OnTitleSceneEndHandler onTitleSceneEnd = delegate { };
		public static event OnPlaySceneStartHandler onPlaySceneStart = delegate { };
		public static event OnPlaySceneEndHandler onPlaySceneEnd = delegate { };
		public static event OnResultSceneStartHandler onResultSceneStart = delegate { };
		public static event OnResultSceneEndHandler onResultSceneEnd = delegate { };

		//! ----parameters----
		[SerializeField]
		AudioClip _titleSceneBGM;
		[SerializeField]
		AudioClip _playSceneBGM;
		[SerializeField]
		AudioClip _resultSceneBGM;

		[SerializeField]
		GameObject _titleSceneHUD;
		[SerializeField]
		GameObject _playSceneHUD;
		[SerializeField]
		GameObject _resultSceneHUD;
		[SerializeField]
		GameObject _replaySelector;
		[SerializeField]
		GameObject _exitSelector;

		[SerializeField]
		AudioClip _titleBGM;
		[SerializeField]
		AudioClip _playBGM;
		[SerializeField]
		AudioClip _resultBGM;

		[SerializeField]
		AudioClip _okSE;

		[SerializeField]
		GameObject _playerPrefab;

		//! ----internal----
		static SceneState _sceneState;
		bool _isSelectedExit = false;
		AudioSource _audioSource;

		//! ----properties----
		public static SceneState sceneState { get { return _sceneState; } }

		//! ----functions----
		void TitleUpdate()
		{
			if(MyInput.anyKeyDown)
			{
				MyInput.locked = true;
				GlobalAudioSource.PlayOneShot(_okSE);
				Caller.Timer(1f).Subscribe(_ =>
				{
					MyInput.locked = false;
					onTitleSceneEnd();
					onPlaySceneStart();
				});
			}
		}

		void ResultUpdate()
		{
			if(MyInput.cursorUp||MyInput.cursorDown)
			{
				_isSelectedExit = !_isSelectedExit;
				_replaySelector.SetActive(!_isSelectedExit);
				_exitSelector.SetActive(_isSelectedExit);
			}

			if(MyInput.ok)
			{
				if(_isSelectedExit)
				{
					onResultSceneEnd();
					Application.Quit();
				}
				else
				{
					MyInput.locked = true;
					GlobalAudioSource.PlayOneShot(_okSE);
					Caller.Timer(1f).Subscribe(_ =>
					{
						MyInput.locked = false;
						_sceneState = SceneState.Play;
						onResultSceneEnd();
						onPlaySceneStart();
					});
				}
			}
		}

		void Start()
		{
			_audioSource = GetComponent<AudioSource>();

			onTitleSceneStart += () => _titleSceneHUD.SetActive(true);
			onTitleSceneEnd += () => _titleSceneHUD.SetActive(false);
			onPlaySceneStart += () => _playSceneHUD.SetActive(true);
			onPlaySceneEnd += () => _playSceneHUD.SetActive(false);
			onResultSceneStart += () => _resultSceneHUD.SetActive(true);
			onResultSceneEnd += () => _resultSceneHUD.SetActive(false);

			onTitleSceneStart += () => _sceneState = SceneState.Title;
			onPlaySceneStart += () => _sceneState = SceneState.Play;
			onResultSceneStart += () => _sceneState = SceneState.Result;

			onTitleSceneStart += () =>
			{
				_audioSource.clip = _titleBGM;
				_audioSource.Play();
			};
			onPlaySceneStart += () =>
			{
				_audioSource.clip = _playBGM;
				_audioSource.Play();
			};
			onResultSceneStart += () =>
			{
				_audioSource.clip = _resultBGM;
				_audioSource.Play();
			};

			onPlaySceneStart += () =>
			{
				var player = ObjectPool.Alloc(_playerPrefab);
				player.transform.position = new Vector3(0f, -4f);

				player.GetComponent<PlayerCore>().onDead += () =>
				{
					Caller.Timer(3f).Subscribe(_ =>
					{
						onPlaySceneEnd();
						onResultSceneStart();
					});
				};
			};
			onPlaySceneEnd += () =>
			{
				ObjectPool.Free(PlayerCore.instance.gameObject);
			};

			SpawnManager.onAllFormationSpawned += () =>
			{
				Caller.Timer(3f).Subscribe(_ =>
				{
					onPlaySceneEnd();
					onResultSceneStart();
				});
			};

			Observable.NextFrame().Subscribe(_ => onTitleSceneStart());
		}

		void Update()
		{
			switch (sceneState)
			{
				case SceneState.Title:
					TitleUpdate();
					break;
				case SceneState.Play:
					break;
				case SceneState.Result:
					ResultUpdate();
					break;
			}
		}
	}
}
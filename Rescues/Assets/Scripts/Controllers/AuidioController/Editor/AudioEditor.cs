using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;


namespace Rescues
{
    public sealed class AudioEditor : EditorWindow
    {
        private int _toolBarInt = 0;
        private int _selectedAudioSource = 0;
        private int _previousSelectedItem = 0;
        private int _selectedAudioClip = 0;
        private string[] _toolbarStrings = { "Audio Clips", "Audio Sources", "Audio Effects" };
        private Vector2 _scrollPosition;
        private List<AudioClip> _audioClips = new List<AudioClip>();
        private List<AudioSource> _audioSources = new List<AudioSource>();
        private AudioMixerGroup[] _audioMixerGroup;
        private Dictionary<string, AudioMixer> _mixers = new Dictionary<string, AudioMixer>();
        private AudioMixer _musicMixer;
        private AudioMixer _voiceMixer;
        private AudioMixer _effectsMixer;
        private AudioMixer _masterMixer;
        private bool _isLooped;
        private bool _playOnAwake;


        [MenuItem("Window/Audio Editor")]

        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(AudioEditor));
        }



        private void OnGUI()
        {

            FindAllSoundFiles();
            _toolBarInt = GUILayout.Toolbar(_toolBarInt, _toolbarStrings);
            if (_toolBarInt == 0)
            {
                AudioClipsToolbarInit();
            }
            if (_toolBarInt == 1)
            {
                AudoSourcesToolbarInit();
            }
            if (_toolBarInt == 2)
            {
                GUILayout.Label("Not Implemented Yet", EditorStyles.boldLabel);
            }

        }

        private void AudioClipsToolbarInit()
        {
            (bool findAll, bool resetAll) = BottomPanelButtons("Find All", "Reset");
            if (findAll)
            {
                _audioClips.Clear();
                var sounds = Resources.FindObjectsOfTypeAll<AudioClip>();
                foreach (var clip in sounds)
                {
                    _audioClips.Add(clip);
                }
            }
            if (resetAll)
            {
                _audioClips.Clear();
            }
            if (_audioClips.Count != 0)
            {
                GUILayout.BeginVertical("Box", GUILayout.Width(Screen.width / 3), GUILayout.Height(Screen.height / 2));
                GUILayout.Label("All Clips in Resources Folder:");
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(Screen.width / 3), GUILayout.Height(Screen.height / 2));
                _selectedAudioClip = CreateSelectionGrid(_audioClips, _selectedAudioClip);
                if (_previousSelectedItem != _selectedAudioClip)
                {
                    Selection.activeObject = _audioClips[_selectedAudioClip];
                    _previousSelectedItem = _selectedAudioClip;
                }
                GUILayout.EndScrollView();
                _isLooped = GUILayout.Toggle(_isLooped, "Looped");
                _playOnAwake = GUILayout.Toggle(_playOnAwake, "Play On Awake");
                if (GUILayout.Button("Create AudioSource"))
                {
                    FindMixer();
                    Debug.Log(_mixers.Count);
                    CreateAudioSource(_audioClips[_selectedAudioClip], _isLooped, _playOnAwake, _masterMixer);
                }
                GUILayout.EndVertical();
            }
        }

        private void FindMixer()
        {
            if (_audioMixerGroup != null)
                return;

            _audioMixerGroup = Resources.FindObjectsOfTypeAll<AudioMixerGroup>();
            if (_audioMixerGroup != null)
            {
                foreach (var mixer in _audioMixerGroup)
                {
                    if (mixer.name == "Master")
                        _mixers.Add(mixer.name, _masterMixer = mixer.audioMixer);
                    if (mixer.name == "Effects")
                        _mixers.Add(mixer.name, _effectsMixer = mixer.audioMixer);
                    if (mixer.name == "Music")
                        _mixers.Add(mixer.name, _musicMixer = mixer.audioMixer);
                    if (mixer.name == "Voice")
                        _mixers.Add(mixer.name, _voiceMixer = mixer.audioMixer);
                }
            }
        }

        private GameObject CreateAudioSource(AudioClip audioClip, bool isLooped, bool playeOnAwake, AudioMixer audioMixerOutput)
        {
            var audioSourceGO = new GameObject(audioClip.name);
            var audioSource = audioSourceGO.AddComponent<AudioSource>();
            var GOtransforms = FindObjectsOfType<Transform>();
            foreach (var transform in GOtransforms)
            {
                if (transform.name == "Sounds" && transform.gameObject.activeSelf)
                {
                    audioSourceGO.transform.SetParent(transform.transform);
                    audioSource.clip = audioClip;
                    audioSource.loop = isLooped;
                    audioSource.playOnAwake = playeOnAwake;
                    audioSource.outputAudioMixerGroup = _audioMixerGroup[0];
                }
            }
            return audioSourceGO;
        }

        private (bool, bool) BottomPanelButtons(string leftBtnName, string rightBtnName)
        {
            Rect bottomPanel = new Rect(5, Screen.height - 50, Screen.width - 10, 300);
            GUILayout.BeginArea(bottomPanel);
            GUILayout.BeginHorizontal();
            var leftButton = GUILayout.Button(leftBtnName, GUILayout.Width(100));
            GUILayout.Space(bottomPanel.width - 200);
            var rigthButton = GUILayout.Button(rightBtnName, GUILayout.Width(100));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            return (leftButton, rigthButton);
        }

        private void AudoSourcesToolbarInit()
        {
            (bool findAll, bool resetAll) = BottomPanelButtons("Find All", "Reset");
            if (findAll)
            {

                var audioSources = Resources.FindObjectsOfTypeAll<AudioSource>();
                foreach (var audioSource in audioSources)
                {
                    _audioSources.Add(audioSource);
                }
            }
            if (resetAll)
            {
                _audioSources.Clear();
            }
            if (_audioSources.Count != 0)
            {
                GUILayout.BeginVertical("Box");
                _selectedAudioSource = CreateSelectionGrid(_audioSources, _selectedAudioSource);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Find AudioSource"))
                {
                    Selection.activeGameObject = _audioSources[_selectedAudioSource].gameObject;
                }
                if (GUILayout.Button("Select All"))
                {
                    Selection.objects = _audioSources.ToArray();
                }
                if (GUILayout.Button("Deselect All"))
                {
                    Selection.objects = default;
                }

                GUILayout.EndVertical();
            }
        }

        private int CreateSelectionGrid<TSource>(List<TSource> list, int selectionNumber) where TSource : UnityEngine.Object
        {
            string[] namesOfListTSource = new string[list.Count];
            for (int i = 0; i < namesOfListTSource.Length; i++)
            {
                namesOfListTSource[i] = list[i].name;
            }
            selectionNumber = GUILayout.SelectionGrid(selectionNumber, namesOfListTSource, 1, GUILayout.Width(200));
            return selectionNumber;
        }

        private void FindAllSoundFiles()
        {
            //throw new NotImplementedException();
        }
    }
}

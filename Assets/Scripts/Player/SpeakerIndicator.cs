using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Dissonance;

public class SpeakerIndicator : MonoBehaviour
{
    private GameObject _indicator;
    private Light _light;
    private Transform _transform;

    private float _intensity;

    private IDissonancePlayer _player;
    private VoicePlayerState _state;

    private bool IsSpeaking => _player.Type == NetworkPlayerType.Remote && _state is {IsSpeaking: true};

    private void OnEnable()
    {
        _indicator = Instantiate(Resources.Load<GameObject>("SpeechIndicator"),transform);
        _indicator.transform.localPosition = new Vector3(0, 6, 0f);

        _light = _indicator.GetComponent<Light>();
        _transform = _indicator.GetComponent<Transform>();

        _player = GetComponent<IDissonancePlayer>();

        StartCoroutine(FindPlayerState());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator FindPlayerState()
    {
        while (!_player.IsTracking)
            yield return null;

        while (_state == null)
        {
            _state = FindObjectOfType<DissonanceComms>().FindPlayer(_player.PlayerId);
            yield return null;
        }
    }

    private void Update()
    {
        if (IsSpeaking)
        {
            _intensity = Mathf.Max(Mathf.Clamp(Mathf.Pow(_state.Amplitude, 0.175f), 0.25f, 1), _intensity - Time.unscaledDeltaTime);
            _indicator.SetActive(true);
        }
        else
        {
            _intensity -= Time.unscaledDeltaTime * 2;

            if (_intensity <= 0)
                _indicator.SetActive(false);
        }

        UpdateLight(_light, _intensity);
        UpdateChildTransform(_transform, _intensity);
    }

    private static void UpdateChildTransform([NotNull] Transform transform, float intensity)
    {
        transform.localScale = new Vector3(intensity, intensity, intensity);
    }

    private static void UpdateLight([NotNull] Light light, float intensity)
    {
        light.intensity = intensity;
    }
}


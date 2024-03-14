using System;
using System.Collections;
using System.Collections.Generic;
using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using Mirror;
using UnityEngine;

public class VoiceChatBehavior : NetworkBehaviour
{
    [SerializeField] private PlayerInputActions _playerInputActions;
    private VoiceBroadcastTrigger _voiceBroadcastTrigger;
    public string voiceChatRoomName = "Global";
    private void Awake()
    {
        gameObject.AddComponent<MirrorIgnorancePlayer>();
        _voiceBroadcastTrigger = gameObject.AddComponent<VoiceBroadcastTrigger>();
        if (_voiceBroadcastTrigger)
        {
            _voiceBroadcastTrigger.Mode = CommActivationMode.Open;
            _voiceBroadcastTrigger.IsMuted = true;
            _voiceBroadcastTrigger.RoomName = voiceChatRoomName;
        }
        if(!_playerInputActions)
            _playerInputActions = ScriptableObject.CreateInstance<PlayerInputActions>();
        _playerInputActions.EnableGameplayInput();
    }

    private void OnEnable()
    {
        _playerInputActions.onToggleSpeak += ToggleMute;
        _playerInputActions.onReleaseSpeak += ToggleMute;
    }
    
    private void OnDisable()
    {
        _playerInputActions.onToggleSpeak -= ToggleMute;
        _playerInputActions.onReleaseSpeak -= ToggleMute;
    }

    private void ToggleMute()
    {
        if (_voiceBroadcastTrigger && isLocalPlayer)
        {
            _voiceBroadcastTrigger.ToggleMute();
        }
    }
}

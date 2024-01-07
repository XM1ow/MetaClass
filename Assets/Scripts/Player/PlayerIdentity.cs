using System.Collections;
using System.Collections.Generic;
using Dissonance;
using Mirror;
using UnityEngine;

public class PlayerIdentity : NetworkBehaviour, IDissonancePlayer
{
    private DissonanceComms _comms;
    
    public bool IsTracking { get; private set; }
    
    [SyncVar]
    private string _playerId;
    public string PlayerId => _playerId;

    public Vector3 Position => transform.position;

    public Quaternion Rotation => transform.rotation;

    public NetworkPlayerType Type
    {
        get
        {
            if (_comms == null || _playerId == null) return NetworkPlayerType.Unknown;
            return _comms.LocalPlayerName.Equals(_playerId) ? NetworkPlayerType.Local : NetworkPlayerType.Remote;
        }
    }
    
    public void OnDestroy()
    {
        if (_comms != null)
            _comms.LocalPlayerNameChanged -= SetPlayerName;
    }

    public void OnEnable()
    {
        _comms = FindObjectOfType<DissonanceComms>();
    }

    public void OnDisable()
    {
        if (IsTracking)
            StopTracking();
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        var comms = FindObjectOfType<DissonanceComms>();
        if (comms == null)
        {
            Debug.LogError("cannot find DissonanceComms in scene");
        }
        Debug.Log("Tracking `OnStartLocalPlayer` Name={comms.LocalPlayerName}");
        
        if (comms.LocalPlayerName != null)
            SetPlayerName(comms.LocalPlayerName);
        comms.LocalPlayerNameChanged += SetPlayerName;
    }

    private void SetPlayerName(string playerName)
    {
        if (IsTracking)
            StopTracking();

        _playerId = playerName;
        StartTracking();

        if (isLocalPlayer)
            CmdSetPlayerName(playerName);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!string.IsNullOrEmpty(PlayerId))
            StartTracking();
    }

    [Command]
    private void CmdSetPlayerName(string playerName)
    {
        _playerId = playerName;

        RpcSetPlayerName(playerName);
    }

    [ClientRpc]
    private void RpcSetPlayerName(string playerName)
    {
        if (!isLocalPlayer)
            SetPlayerName(playerName);
    }

    private void StartTracking()
    {
        if (_comms != null)
        {
            _comms.TrackPlayerPosition(this);
            IsTracking = true;
        }
    }

    private void StopTracking()
    {
        if (_comms != null)
        {
            _comms.StopTracking(this);
            IsTracking = false;
        }
    }
}

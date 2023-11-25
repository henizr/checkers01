using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : Player
{

public static event Action ClientOnInfoUpdated;
public static event Action<bool> AuthorityOnLobbyOwnerStateUpdated;



    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
string displayName;
public string DisplayName
{
    get { return displayName; }
    [Server]
    set { displayName = value; }
}

    [SyncVar(hook = nameof(AuthorityHandleLobbyOwnerStateUpdated))]
    bool lobbyOwner;

    void AuthorityHandleLobbyOwnerStateUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority)

            return;

        AuthorityOnLobbyOwnerStateUpdated?.Invoke(newState);
    }

    public bool LobbyOwner
    {
        get { return lobbyOwner; }
        [Server]
        set { lobbyOwner = value; }
    }
    void ClientHandleDisplayNameUpdated(string oldName, string newName)
{
    ClientOnInfoUpdated?.Invoke();
}
public override void OnStartClient()
{

 
if (!isClientOnly)
    return;
((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Add(this);
}


public override void OnStopClient()
{

 

if (!isClientOnly)

((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Remove(this);

ClientOnInfoUpdated?.Invoke();

}


}

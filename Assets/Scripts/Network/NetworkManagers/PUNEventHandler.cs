using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;

//TODO: make this handle the events

public class PUNEventHandler : IPunCallbacks
{
    public void OnConnectedToMaster()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnectedToPhoton()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnectedFromPhoton()
    {
        throw new System.NotImplementedException();
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedLobby()
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        Debug.Log("asd");
    }

    public void OnLeftLobby()
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnLobbyStatisticsUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipRequest(object[] viewAndPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(object[] viewAndPlayers)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonMaxCccuReached()
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        throw new System.NotImplementedException();
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        throw new System.NotImplementedException();
    }

    public void OnReceivedRoomListUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdatedFriendList()
    {
        throw new System.NotImplementedException();
    }

    public void OnWebRpcResponse(OperationResponse response)
    {
        throw new System.NotImplementedException();
    }
}

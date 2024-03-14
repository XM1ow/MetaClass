using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkSlidesSyncManager : NetworkBehaviour
{
    public LoadPPT localSlidesPlayer;
    
    
    [Command(requiresAuthority = false)]
    public void CmdSendBytePackage(byte[] bytes, int index, bool isLastPackage)
    {
        if(!isLastPackage)
            ReceiveBytePackage(bytes);
        else
        {
            ReceiveBytePackage(bytes);
            BuildSlide(bytes, index);
        }
            
    }

    [ClientRpc]
    public void ReceiveBytePackage(byte[] bytes)
    {
        if (!localSlidesPlayer) return;
        if (bytes.Length == 0) return;
        localSlidesPlayer.ReadBufferToTemp(bytes);
        Debug.Log($"Receiving a byte package of {bytes.Length} bytes");
    }

    [ClientRpc]
    public void BuildSlide(byte[] bytes, int index)
    {
        if (!localSlidesPlayer) return;
        if (bytes.Length == 0) return;
        // Deserializing slide bytes to picture
        localSlidesPlayer.ReadBufferToTemp(bytes);
        localSlidesPlayer.MakeSlidePrefab(index);
    }

    #region first page

    [Command(requiresAuthority = false)]
    public void CmdFirstPage()
    {
        ClientFirstPage();
    }

    [ClientRpc]
    public void ClientFirstPage()
    {
        localSlidesPlayer.FirstPage();
    }

    #endregion

    #region last page

    [Command(requiresAuthority = false)]
    public void CmdLastPage()
    {
        ClientLastPage();
    }

    [ClientRpc]
    public void ClientLastPage()
    {
        localSlidesPlayer.LastPage();
    }

    #endregion

    #region previous page

    [Command(requiresAuthority = false)]
    public void CmdPreviousPage()
    {
        ClientPreviousPage();
    }

    [ClientRpc]
    public void ClientPreviousPage()
    {
        localSlidesPlayer.ToPrePage();
    }

    #endregion

    #region next page

    [Command(requiresAuthority = false)]
    public void CmdNextPage()
    {
        ClientNextPage();
    }

    [ClientRpc]
    public void ClientNextPage()
    {
        localSlidesPlayer.ToNextPage();
    }

    #endregion
    
}

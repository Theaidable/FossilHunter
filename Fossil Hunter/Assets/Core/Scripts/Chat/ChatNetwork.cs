using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

/// <summary>
/// Dette script bruges til at håndtere chat-funktionen over netværk
/// </summary>
/// <author> David Gudmund Danielsen </author>
public class ChatNetwork : NetworkBehaviour
{
    #region Fields
    private readonly List<IChatUI> chatUIs = new List<IChatUI>();
    
    //Singleton Pattern
    private static ChatNetwork instance;
    public static ChatNetwork Instance
    {
        get { return instance; }
    }
    #endregion

    /// <summary>
    /// Private constructor på grund af Singleton
    /// </summary>
    private ChatNetwork() { }

    /// <summary>
    /// Før start, så skabes der en instance af objektet
    /// </summary>
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Helpers

    #region Registrer Chat UI
    /// <summary>
    /// Registrer Chat UI så vi kan sende beskeder hertil
    /// </summary>
    /// <param name="chatUI"></param>
    public void RegisterChatUI(IChatUI chatUI)
    {
        if (chatUIs.Contains(chatUI) == false)
        {
            chatUIs.Add(chatUI);
        }
    }

    /// <summary>
    /// Unregister UI hvis vi ikke skal sende beskeder hertil længere
    /// </summary>
    /// <param name="chatUI"></param>
    public void UnRegisterChatUI(IChatUI chatUI)
    {
        chatUIs.Remove(chatUI);
    }
    #endregion

    #region Server og Client Funktioner
    /// <summary>
    /// Send besked fra client til server
    /// </summary>
    /// <param name="text"></param>
    public void SendLocalMessage(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        string sender = $"Player {NetworkManager.Singleton.LocalClientId}";
        SendMessageServerRpc(sender, text);
    }

    /// <summary>
    /// ServerRpc = Client -> Server
    /// Metoden sender en besked fra client til server
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="text"></param>
    /// <param name="rpcParams"></param>
    [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
    private void SendMessageServerRpc(string sender, string text, ServerRpcParams rpcParams = default)
    {
        string final = $"[{sender}] {text}";
        BroadcastMessageClientRpc(final);
    }

    /// <summary>
    /// ClientRpc = Server -> All clients
    /// Metoden sender besked fra server til alle clients
    /// Beskeden er den fra metoden "SendMessageServerRpc"
    /// </summary>
    /// <param name="message"></param>
    [ClientRpc]
    private void BroadcastMessageClientRpc(string message)
    {
        foreach (var ui in chatUIs)
        {
            ui.AddMessage(message);
        }
    }
    #endregion

    #endregion
}

/// <summary>
/// Et simpelt interface som både lærer- og elev-chat-UI kan implementere
/// </summary>
public interface IChatUI
{
    void AddMessage(string message);
}

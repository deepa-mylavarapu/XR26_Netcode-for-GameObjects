using Unity.Netcode;
using TMPro;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI chatDisplay;

    public void SendMessage()
    {
        if (IsOwner && !string.IsNullOrEmpty(inputField.text))
        {
            SendMessageServerRpc(inputField.text);
            inputField.text = "";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SendMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        BroadcastMessageClientRpc($"Player {rpcParams.Receive.SenderClientId}: {message}");
    }

    [ClientRpc]
    void BroadcastMessageClientRpc(string message)
    {
        chatDisplay.text += message + "\n";
    }
}

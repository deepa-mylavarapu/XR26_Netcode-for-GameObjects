using Unity.Netcode;
using TMPro;
using UnityEngine;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI chatDisplay;

    public void SendMessage()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            SendMessageServerRpc(inputField.text);
            inputField.text = "";
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void SendMessageServerRpc(string message, ServerRpcParams rpcParams = default)
    {
        Debug.Log("Received message from client: " + message);

        string formatted = $"Player {rpcParams.Receive.SenderClientId}: {message}";
        BroadcastMessageClientRpc(formatted);
    }

    [ClientRpc]
    void BroadcastMessageClientRpc(string message)
    {
        if (chatDisplay != null)
        {
            Debug.Log("Broadcasting message: " + message);
            chatDisplay.text += message + "\n";
        }
    }

}


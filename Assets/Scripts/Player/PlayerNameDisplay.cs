using Unity.Netcode;
using TMPro;
using UnityEngine;

public class PlayerNameDisplay : NetworkBehaviour
{
    [SerializeField] private TextMeshPro nameText;
    private NetworkVariable<string> playerName = new NetworkVariable<string>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            SetNameServerRpc("Player_" + OwnerClientId);

        playerName.OnValueChanged += (_, newVal) => nameText.text = newVal;
    }

    [ServerRpc]
    void SetNameServerRpc(string newName)
    {
        playerName.Value = newName;
    }
}

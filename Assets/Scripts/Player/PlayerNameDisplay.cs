using Unity.Netcode;
using TMPro;
using UnityEngine;
using Unity.Collections;

public class PlayerNameDisplay : NetworkBehaviour
{
    [SerializeField] private TextMeshPro nameText;
    private NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            SetNameServerRpc(new FixedString128Bytes("Player_" + OwnerClientId));

        playerName.OnValueChanged += (_, newVal) => nameText.text = newVal.ToString();
    }

    [ServerRpc]
    void SetNameServerRpc(FixedString128Bytes newName)
    {
        playerName.Value = newName;
    }
}

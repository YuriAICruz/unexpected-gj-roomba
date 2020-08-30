using System;
using Mirror;
using Roomba.Systems.Interfaces;

namespace Roomba.Systems.LevelAssets
{
    public abstract class InteractableBase : NetworkBehaviour, IInteractable
    {
        [SyncVar] private bool _interacted;

        protected virtual void Start()
        {
            if (!isLocalPlayer && _interacted)
            {
                Interact();
            }
        }

        public virtual void Interact()
        {
            if (isLocalPlayer)
                CmdSetInteraction(true);
        }

        [Command]
        void CmdSetInteraction(bool value)
        {
            _interacted = value;

            RpcInteraction();
        }

        [ClientRpc]
        void RpcInteraction()
        {
            if (!isLocalPlayer)
                Interact();
        }
    }
}
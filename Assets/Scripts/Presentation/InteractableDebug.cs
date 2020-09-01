using Roomba.Systems.Interfaces;
using Roomba.Systems.LevelAssets;
using UnityEngine;
using Zenject;

namespace Roomba.Presentation
{
    public class InteractableDebug : InteractableBase
    {
        public override void Interact()
        {
            base.Interact();
            gameObject.SetActive(false);
        }
    }
}
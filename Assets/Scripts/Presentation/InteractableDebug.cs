using Roomba.Systems.Interfaces;
using UnityEngine;
using Zenject;

namespace Roomba.Presentation
{
    public class InteractableDebug : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            gameObject.SetActive(false);
        }
    }
}
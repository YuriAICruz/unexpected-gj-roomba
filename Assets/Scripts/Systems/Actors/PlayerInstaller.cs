using UnityEngine;
using Zenject;

namespace Roomba.Systems.Actors
{
    public class PlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(Camera.main.GetComponent<CameraController>());
        }
    }
}
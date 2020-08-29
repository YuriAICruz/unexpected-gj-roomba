using Roomba.Presentation;
using Roomba.Systems;
using UnityEngine;
using Zenject;

public class InGameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInstance(Camera.main.GetComponent<CameraController>());

        Container.Bind<GameManager>().AsSingle().NonLazy();
    }
}
using Roomba.Presentation;
using Roomba.Systems;
using Roomba.Systems.Actors;
using UnityEngine;
using Zenject;

public class InGameInstaller : MonoInstaller
{
    [Inject] private GlobalSettings _globalSettings;
    
    public override void InstallBindings()
    {
        Container.BindInstance(Camera.main.GetComponent<CameraController>());

        Container.Bind<GameManager>().AsSingle().NonLazy();
          
        Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(_globalSettings.playerPrefab);
    }
}
using System.Security.Cryptography.X509Certificates;
using Mirror;
using Roomba.Systems;
using Roomba.Systems.Actors;
using Roomba.Systems.Input;
using Roomba.Systems.Network;
using UnityEngine;
using Zenject;
using NetworkConnection = Roomba.Systems.Network.NetworkConnection;

namespace Roomba.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
    {
        public InputCollector.InputSetting inputSetting;

        public GameNetworkManager networkManager;

        public GlobalSettings globalSettings;
        
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<AxisSignal>();
            Container.DeclareSignal<ActionSignal>();
            
            Container.DeclareSignal<NetworkConnectionSignal>();
            Container.DeclareSignal<NetworkServerConnectionSignal>();
            
            Container.BindInstance(inputSetting);
            Container.BindInstance(globalSettings);
            
            Container.BindInterfacesAndSelfTo<InputCollector>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<ApplicationManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<NetworkConnection>().AsSingle().NonLazy();
            
            Container.Bind<IDataRepository>().To<LocalJsonFileDataRepository>().AsSingle().NonLazy();
            
            Container.BindFactory<GameNetworkManager, GameNetworkManager.Factory>().FromComponentInNewPrefab(networkManager);
            
            Container.BindInstance(Container.Resolve<GameNetworkManager.Factory>().Create()).AsSingle();
        }
    }
}
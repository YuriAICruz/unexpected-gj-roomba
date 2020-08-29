using System.Security.Cryptography.X509Certificates;
using Roomba.Systems;
using Roomba.Systems.Input;
using UnityEngine;
using Zenject;

namespace Roomba.Installers
{
    [CreateAssetMenu(fileName = "ProjectInstaller", menuName = "Installers/ProjectInstaller")]
    public class ProjectInstaller : ScriptableObjectInstaller<ProjectInstaller>
    {
        public InputCollector.InputSetting inputSetting;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<AxisSignal>();
            Container.DeclareSignal<ActionSignal>();
            
            Container.BindInstance(inputSetting);
            
            Container.BindInterfacesAndSelfTo<InputCollector>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ApplicationManager>().AsSingle().NonLazy();
        }
    }
}
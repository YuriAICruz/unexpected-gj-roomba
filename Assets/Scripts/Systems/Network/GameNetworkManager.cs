using Mirror;
using Roomba.Systems.Actors;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Roomba.Systems.Network
{
    public class GameNetworkManager : NetworkManager
    {
        [Inject] private SignalBus _signalBus;
        //[Inject] private Player.Factory _playerFactory;
        [Inject] private ZenjectSceneLoader _sceneLoader;
        [Inject] private GlobalSettings _globalSettings;
        private DiContainer _currentSceneContainer;

        public class Factory : PlaceholderFactory<GameNetworkManager>
        {
        }

        public override void Awake()
        {
            playerPrefab = _globalSettings.playerPrefab.gameObject;
            base.Awake();
        }

        #region Client

        public override void OnClientConnect(Mirror.NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            _signalBus.Fire(new NetworkConnectionSignal(NetworkConnectionSignal.State.Connected));
        }

        public override void OnClientDisconnect(Mirror.NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            _signalBus.Fire(new NetworkConnectionSignal(NetworkConnectionSignal.State.Disconnected));
        }

        public override void OnClientError(Mirror.NetworkConnection conn, int errorCode)
        {
            base.OnClientError(conn, errorCode);
            _signalBus.Fire(new NetworkConnectionSignal(NetworkConnectionSignal.State.Error));
        }

        #endregion

        #region Server

        public override void OnServerConnect(Mirror.NetworkConnection conn)
        {
            base.OnServerConnect(conn);

            _signalBus.Fire(new NetworkServerConnectionSignal(NetworkServerConnectionSignal.State.Connected));
        }

        public override void OnServerDisconnect(Mirror.NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            _signalBus.Fire(new NetworkServerConnectionSignal(NetworkServerConnectionSignal.State.Disconnected));
        }

        public override void OnServerError(Mirror.NetworkConnection conn, int errorCode)
        {
            base.OnServerError(conn, errorCode);
            _signalBus.Fire(new NetworkServerConnectionSignal(NetworkServerConnectionSignal.State.Error));
        }

        #endregion

        public override void OnServerAddPlayer(Mirror.NetworkConnection conn)
        {
            Transform startPos = GetStartPosition();
            
            GameObject player = _currentSceneContainer.Resolve<Player.Factory>().Create().gameObject;

            if (startPos != null)
                player.transform.SetPositionAndRotation(startPos.position, startPos.rotation);

            player.transform.SetParent(null);

            NetworkServer.AddPlayerForConnection(conn, player);

            //base.OnServerAddPlayer(conn);
        }

        protected override AsyncOperation LoadSceneAsync(string newSceneName,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            return _sceneLoader.LoadSceneAsync(newSceneName, loadSceneMode,
                container =>
                {
                    _currentSceneContainer = container;
                });
        }
    }
}
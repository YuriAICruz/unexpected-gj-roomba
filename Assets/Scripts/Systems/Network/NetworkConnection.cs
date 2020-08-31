using Mirror;
using UnityEngine;

namespace Roomba.Systems.Network
{
    public class NetworkConnectionSignal
    {
        public enum State
        {
            Disconnected = 0,
            Connected = 1,
            Connecting = 2,
            Error = 3,
        }

        public State state;

        public NetworkConnectionSignal(State state)
        {
            this.state = state;
        }
    }
    
    public class NetworkServerConnectionSignal
    {
        public enum State
        {
            Disconnected = 0,
            Connected = 1,
            Connecting = 2,
            Error = 3,
        }

        public State state;

        public NetworkServerConnectionSignal(State state)
        {
            this.state = state;
        }
    }
    
    public class NetworkConnection
    {
        private readonly GameNetworkManager _networkManager;

        public NetworkConnection(GameNetworkManager networkManager)
        {
            _networkManager = networkManager;
        }
        
        public void Joint()
        {
            if(NetworkClient.isConnected) return;
            
            _networkManager.StartClient();
        }
        
        public void SetAddress(string networkAddress)
        {
            if(NetworkClient.isConnected) return;
            
            _networkManager.networkAddress = networkAddress;
        }
        
        public void Host()
        {
            if(NetworkClient.isConnected) return;
            
            _networkManager.StartHost();
        }
        
        public void Serve()
        {
            _networkManager.StartServer();
        }
    }
}
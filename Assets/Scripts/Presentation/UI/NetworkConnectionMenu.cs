using System;
using Roomba.Systems.Network;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Roomba.Presentation.UI
{
    [RequireComponent(typeof(CanvasGroupView))]
    public class NetworkConnectionMenu : MonoBehaviour
    {
        [Inject] private NetworkConnection _network;
        [Inject] private SignalBus _signalBus;
        private CanvasGroupView _cv;

        public Button join, host, serve;

        public InputField address;

        private void Awake()
        {
            _signalBus.Subscribe<NetworkConnectionSignal>(OnConnectionChange);
            _cv = GetComponent<CanvasGroupView>();

            join.onClick.AddListener(() =>
            {
                _network.SetAddress(address.text);
                _network.Joint();
            });
            host.onClick.AddListener(() =>
            {
                _network.SetAddress(address.text);
                _network.Host();
            });
            serve.onClick.AddListener(() =>
            {
                _network.SetAddress(address.text);
                _network.Serve();
            });
            
            address.onEndEdit.AddListener(_network.SetAddress);
        }

        private void OnConnectionChange(NetworkConnectionSignal connection)
        {
            switch (connection.state)
            {
                case NetworkConnectionSignal.State.Disconnected:
                    _cv.Show();
                    break;
                case NetworkConnectionSignal.State.Connected:
                    _cv.Hide();
                    break;
                case NetworkConnectionSignal.State.Error:
                    _cv.Show();
                    break;
            }
        }
    }
}
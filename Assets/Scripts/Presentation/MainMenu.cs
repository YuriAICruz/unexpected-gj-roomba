using System;
using Roomba.Systems;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace Roomba.Presentation
{
    public class MainMenu : MonoBehaviour
    {
        [Inject] private ApplicationManager _manager;
        [Inject] private InputCollector _input;
        
        public Button startGame, options;

        private void Start()
        {
            startGame.onClick.AddListener(_manager.StartGame);
            options.onClick.AddListener(_manager.OpenMenu);
        }
    }
}
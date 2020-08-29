using System;
using Roomba.Systems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
namespace Roomba.Presentation
{
    public class MainMenu : MonoBehaviour
    {
        [Inject] private ApplicationManager _manager;
        [Inject] private InputCollector _input;
        
        public Button startGame, options, closeO, quit;

        public GameObject FirstOption, OptionsMenu;
        private void Start()
        {
            startGame.onClick.AddListener(_manager.StartGame);
            options.onClick.AddListener(OpenMenu);
            closeO.onClick.AddListener(CloseMenu);
            quit.onClick.AddListener(_manager.QuitGame);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(startGame.gameObject);
        }

        private void CloseMenu()
        {
            OptionsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(options.gameObject);
        }

        private void OpenMenu()
        {
            OptionsMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(FirstOption);
        }

        private void Update()
        {
            _input.Tick();
        }
    }
}
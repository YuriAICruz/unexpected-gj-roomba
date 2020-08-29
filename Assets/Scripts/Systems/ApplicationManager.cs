using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Roomba.Systems
{
    public class ApplicationManager
    {
        public void OpenMenu()
        {
            throw new NotImplementedException();
        }
        
        public void StartGame()
        {
            SceneManager.LoadScene("Tests");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
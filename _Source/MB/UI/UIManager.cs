using System;
using System.Collections.Generic;
using MB.Player.Abstract;
using UnityEngine;

namespace MB.UI
{
    public class UIManager : MonoBehaviour, IObserver
    {
        [field: SerializeField] private GameObject menuUI;

        private Dictionary<PlayerAction, Action> _uiActionsList;

        private void Awake()
        {
            _uiActionsList = new Dictionary<PlayerAction, Action>
            {
                { PlayerAction.InMenu, HandleInMenu }
            };
        }

        public void OnNotify(PlayerAction playerAction)
        {
            if (_uiActionsList.ContainsKey(playerAction)) _uiActionsList[playerAction]();
        }

        private void HandleInMenu()
        {
            menuUI.SetActive(true);
        }
    }
}
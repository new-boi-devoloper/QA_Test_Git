using System;
using MB.Player.PlayerFunctions;
using UnityEngine;
using Zenject;

namespace MB.Player.ArmyManagement
{
    public class UnitSelectionManagerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform selectionArearectTransform;
        [SerializeField] private Canvas canvas;

        private IArmyChoser _playerInteractor;

        private void Update()
        {
            if (selectionArearectTransform.gameObject.activeSelf) UpdateVisuals();
        }

        private void OnEnable()
        {
            _playerInteractor.OnSelectionAreaStart += PlayerInteractor_OnOnSelectionAreaStart;
            _playerInteractor.OnSelectionAreaEnd += PlayerInteractor_OnOnSelectionAreaEnd;

            selectionArearectTransform.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _playerInteractor.OnSelectionAreaStart += PlayerInteractor_OnOnSelectionAreaStart;
            _playerInteractor.OnSelectionAreaEnd += PlayerInteractor_OnOnSelectionAreaEnd;
        }

        private void PlayerInteractor_OnOnSelectionAreaStart(object sender, EventArgs e)
        {
            selectionArearectTransform.gameObject.SetActive(true);
            Debug.Log("PlayerInteractor_OnOnSelectionAreaStart");
            UpdateVisuals();
        }

        private void PlayerInteractor_OnOnSelectionAreaEnd(object sender, EventArgs e)
        {
            selectionArearectTransform.gameObject.SetActive(false);

            Debug.Log("PlayerInteractor_OnOnSelectionAreaEnd");
        }

        public void UpdateVisuals()
        {
            var selectionAreaRect = _playerInteractor.GetSelectionAreaRect();

            var canvasScale = canvas.transform.localScale.x;

            selectionArearectTransform.anchoredPosition =
                new Vector2(selectionAreaRect.x, selectionAreaRect.y) / canvasScale;
            selectionArearectTransform.sizeDelta =
                new Vector2(selectionAreaRect.width, selectionAreaRect.height) / canvasScale;
        }

        [Inject]
        public void Construct(IArmyChoser playerInteractor)
        {
            _playerInteractor = playerInteractor;
        }
    }
}
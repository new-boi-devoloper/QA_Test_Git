using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using MB.SO.QuestSo;
using UnityEngine;

namespace MB.UI.DialogUI
{
    public class DialogController
    {
        private readonly DialogView _dialogView;

        public DialogController(DialogView dialogView)
        {
            _dialogView = dialogView;
        }

        public async UniTask<bool> InvokeDialog(DialogData dialogData)
        {
            var firstDialogUnit = dialogData.DialogUnits.FirstOrDefault(d => d.IsFirst);
            if (firstDialogUnit == null)
            {
                Debug.LogError("No starting dialog unit found.");
                return false;
            }

            _dialogView.DialogContainer.SetActive(true);
            var currentDialogUnit = firstDialogUnit;
            while (currentDialogUnit != null)
            {
                // Показ текста диалога
                await ShowDialogText(currentDialogUnit.DialogText);

                // Ожидание ответа игрока
                var (nextDialogUnit, selectedResponse) = await ShowDialogOptions(currentDialogUnit);
                if (nextDialogUnit == null)
                {
                    // Игрок вышел из диалога
                    _dialogView.DialogContainer.SetActive(false);
                    return false;
                }

                // Если это финальный диалог, проверяем тип ответа
                if (currentDialogUnit.DialogFlowUnitType == DialogFlowUnitType.Final)
                {
                    // Показываем дополнительные сообщения, если они есть
                    if (nextDialogUnit.DialogText != null && nextDialogUnit.DialogText.Length > 0)
                    {
                        Debug.Log("бесконечно ждём что-то");
                        await ShowDialogText(nextDialogUnit.DialogText);
                        Debug.Log("не ждём что-то");
                    }

                    // Возвращаем результат в зависимости от выбранного ответа
                    _dialogView.DialogContainer.SetActive(false);
                    Debug.Log($"ну типа тру{selectedResponse.DialogMeaningUnitType}");
                    return true;
                }

                currentDialogUnit = nextDialogUnit;
            }

            // Выключаем диалоговый контейнер
            _dialogView.DialogContainer.SetActive(false);

            return false;
        }

        private async UniTask ShowDialogText(IEnumerable<string> dialogText)
        {
            // Временно, надо сделать считывание пробела в режиме InteractionMode.NoInteraction
            foreach (var text in dialogText)
            {
                _dialogView.NpcDialogField = text;
                await UniTask.Delay(TimeSpan.FromSeconds(3)); // Задержка между строками
            }
        }

        private async UniTask<(DialogUnit, DialogUnit.ResponseToDialogUnit)> ShowDialogOptions(
            DialogUnit currentDialogUnit)
        {
            // Очистка предыдущих кнопок
            _dialogView.ClearButtons();

            // Создание кнопок для всех ответов
            var buttons = new List<DialogButton>();
            foreach (var response in currentDialogUnit.ResponseToDialog)
            {
                var button = _dialogView.CreateButton(response.ResponseLeadingToDialog);
                buttons.Add(button);
            }

            // Ожидание выбора игрока
            var completedTask = await UniTask.WhenAny(buttons.Select(b => b.OnButtonClick()));

            // Возвращаем следующий DialogUnit и выбранный ответ
            var selectedResponse = currentDialogUnit.ResponseToDialog[completedTask.winArgumentIndex];
            return (selectedResponse.NextDialog, selectedResponse);
        }
    }
}
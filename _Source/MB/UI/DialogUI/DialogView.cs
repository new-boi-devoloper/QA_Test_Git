using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MB.UI.DialogUI
{
    public class DialogView : MonoBehaviour
    {
        [field: SerializeField] public GameObject DialogContainer { get; private set; }

        [SerializeField] private VerticalLayoutGroup buttonsPlace;
        [SerializeField] private DialogButton buttonPrefab;
        [SerializeField] private TextMeshProUGUI npcDialogField;

        public string NpcDialogField
        {
            set => npcDialogField.text = value;
        }

        public void ClearButtons()
        {
            foreach (Transform child in buttonsPlace.transform) Destroy(child.gameObject);
        }

        public DialogButton CreateButton(string buttonText)
        {
            var button = Instantiate(buttonPrefab, buttonsPlace.transform);
            button.SetButtonInfo(buttonText);
            return button;
        }
    }
}
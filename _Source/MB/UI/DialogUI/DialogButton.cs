using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MB.UI.DialogUI
{
    public class DialogButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buttonText;

        private UniTaskCompletionSource<bool> _clickCompletionSource;

        public void SetButtonInfo(string text)
        {
            buttonText.text = text;
            _clickCompletionSource = new UniTaskCompletionSource<bool>();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _clickCompletionSource.TrySetResult(true);
        }

        public async UniTask<bool> OnButtonClick()
        {
            return await _clickCompletionSource.Task;
        }
    }
}
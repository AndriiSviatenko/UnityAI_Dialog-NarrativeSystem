using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;
using NarrativeSystem.Nodes;
using System.Threading;

namespace NarrativeSystem.Runtime
{
    public class BasicDialogueUI : MonoBehaviour, IDialogueView
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [SerializeField] private Transform _choiceContainer;
        [SerializeField] private Button _choiceButtonPrefab;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _skipButton;
        [SerializeField] private Button _backButton;

        public void Initialize(GameObject panel, TextMeshProUGUI speakerText, TextMeshProUGUI contentText, Transform choiceContainer, Button choiceButtonPrefab, Button continueButton)
        {
            _panel = panel;
            _speakerText = speakerText;
            _contentText = contentText;
            _choiceContainer = choiceContainer;
            _choiceButtonPrefab = choiceButtonPrefab;
            _continueButton = continueButton;
        }

        public void SetSkipBackButtons(Button skipButton, Button backButton)
        {
            _skipButton = skipButton;
            _backButton = backButton;
        }

        private TaskCompletionSource<bool> _continueTcs;
        private TaskCompletionSource<int> _choiceTcs;

        private void Awake()
        {
            _panel.SetActive(false);
            _continueButton.onClick.AddListener(() => _continueTcs?.TrySetResult(true));
        }

        public async Task DisplayLine(DialogueLine line, CancellationToken ct)
        {
            _panel.SetActive(true);
            _speakerText.text = line.SpeakerID;
            _contentText.text = line.Text;
            _choiceContainer.gameObject.SetActive(false);
            _continueButton.gameObject.SetActive(true);

            _continueTcs = new TaskCompletionSource<bool>();
            
            using (ct.Register(() => _continueTcs?.TrySetCanceled()))
            {
                try 
                {
                    await _continueTcs.Task;
                }
                catch (TaskCanceledException)
                {
                    // Swallow cancellation as it's handled by the runner
                    throw;
                }
            }
        }

        public async Task<int> DisplayChoices(List<ChoiceOption> choices, CancellationToken ct)
        {
            _panel.SetActive(true);
            _choiceContainer.gameObject.SetActive(true);
            _continueButton.gameObject.SetActive(false);

            foreach (Transform child in _choiceContainer)
            {
                Destroy(child.gameObject);
            }

            _choiceTcs = new TaskCompletionSource<int>();

            for (int i = 0; i < choices.Count; i++)
            {
                int index = i;
                var btn = Instantiate(_choiceButtonPrefab, _choiceContainer);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = choices[index].Text;
                btn.onClick.AddListener(() => _choiceTcs.TrySetResult(index));
            }

            using (ct.Register(() => _choiceTcs?.TrySetCanceled()))
            {
                try 
                {
                    return await _choiceTcs.Task;
                }
                catch (TaskCanceledException)
                {
                    throw;
                }
            }
        }

        public void Close()
        {
            _panel.SetActive(false);
        }
    }
}

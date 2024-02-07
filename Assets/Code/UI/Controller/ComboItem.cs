using System.Collections;
using Code.Game.Core;
using Code.Game.Main;
using Code.Utils.Pooling;
using TMPro;
using UnityEngine;

namespace Code.UI.Controller
{
    public class ComboItem : MonoBehaviour, IPoolObject
    {
        [SerializeField] private TMP_Text comboValueText;
        [SerializeField] private CanvasGroup comboCanvasGroup;

        public void SetData(int combo)
        {
            comboValueText.text = $"{combo}x Combo";
            Animate(155, 2f);
        }

        private void Animate(float moveYAmount, float duration)
        {
            comboCanvasGroup.alpha = 1;
            StartCoroutine(AnimateMoveAndFadeOut(moveYAmount, duration));
        }

        private IEnumerator AnimateMoveAndFadeOut(float moveYAmount, float duration)
        {
            var startPosition = transform.localPosition;
            var endPosition = new Vector3(startPosition.x, startPosition.y + moveYAmount, startPosition.z);

            var startTime = Time.time;
            var endTime = startTime + duration;

            while (Time.time < endTime)
            {
                var t = (Time.time - startTime) / duration;
                var easeT = 1 - (1 - t) * (1 - t); // Ease OutQuad easing function

                // Move
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, easeT);

                // Fade Out
                comboCanvasGroup.alpha = 1 - easeT;

                yield return null;
            }

            // Ensure the final state is applied
            transform.localPosition = endPosition;
            comboCanvasGroup.alpha = 0;

            Pool.ReleaseObject(gameObject);
        }

        public void OnObjectReturned()
        {
            GameEvents.GameStateChange -= GameEventsOnGameStateChange;
        }

        public void OnObjectInit()
        {
            GameEvents.GameStateChange += GameEventsOnGameStateChange;
        }

        private void GameEventsOnGameStateChange(GameState gameState)
        {
            if (gameState != GameState.Gameplay)
            {
                Pool.ReleaseObject(gameObject);
            }
        }

        public SimpleObjectPool Pool { get; set; }
    }
}
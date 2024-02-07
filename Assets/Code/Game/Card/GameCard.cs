using System;
using System.Collections;
using Code.Utils.Pooling;
using UnityEngine;

namespace Code.Game.Card
{
    public class GameCard : MonoBehaviour, IGameCard, IPoolObject
    {
        [SerializeField] private SpriteRenderer card;
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private float flipDuration = 0.25f;
        [SerializeField] private AnimationCurve flipCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private Sprite cardBackSprite;
        [SerializeField] private Sprite cardFrontSprite;
        public float time;

        public Action<GameCard> OnCardClick;
        public Action OnCardShown;

        private Quaternion startRotation;
        private Quaternion endRotation;

        private bool isFlipping;
        private bool isFrontSideVisible; // Flag to track if the front side of the card is visible


        private void OnMouseDown()
        {
            OnCardClick?.Invoke(this);
        }

        public void Flip()
        {
            if (isFlipping)
                return;
            
            // Start flipping animation
            StartCoroutine(AnimateFlip());
        }

        public void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public bool Match(string cardIconName)
        {
            return icon.sprite.name.Equals(cardIconName);
        }

        public string GetName()
        {
            return icon.sprite.name;
        }

        public void SetAsMatched()
        {
            Pool.ReleaseObject(gameObject);
        }

        private IEnumerator AnimateFlip()
        {
            isFlipping = true;
            var elapsedTime = 0f;

            startRotation = transform.rotation;
            endRotation = startRotation * Quaternion.Euler(0, 90, 0);

            isFrontSideVisible = !isFrontSideVisible;

            while (elapsedTime < flipDuration)
            {
                var t = flipCurve.Evaluate(elapsedTime / flipDuration);
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

                if (t >= 0.97f)
                {
                    // Switch card sprite to front side if it's visible, otherwise switch to back side
                    card.sprite = isFrontSideVisible ? cardFrontSprite : cardBackSprite;
                    icon.gameObject.SetActive(isFrontSideVisible);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = endRotation;

            // Set the start and end rotations for the second half of the flip
            startRotation = endRotation;
            endRotation = startRotation * Quaternion.Euler(0, -90, 0);
            elapsedTime = 0f;

            while (elapsedTime < flipDuration)
            {
                var t = flipCurve.Evaluate(elapsedTime / flipDuration);
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = endRotation;
            if(isFrontSideVisible)
                OnCardShown?.Invoke();

            isFlipping = false;
        }

        public SimpleObjectPool Pool { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Utils
{
    public static class Utilities
    {
        public static void Shuffle<T>(List<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Range(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static IEnumerator FadeInCoroutine(float duration, CanvasGroup canvasGroup)
        {
            canvasGroup.gameObject.SetActive(true);

            canvasGroup.alpha = 0f;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }
        
        public static IEnumerator MoveYCoroutine(Transform obj, float targetY, float duration, float delay, Action onComplete)
        {
            yield return new WaitForSeconds(delay);
            float startTime = Time.time;
            float endTime = startTime + duration;

            var position = obj.localPosition;
            Vector3 startPosition = position;
            Vector3 endPosition = new Vector3(position.x, targetY, position.z);

            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / duration;

                float easeT = 1 - (1 - t) * (1 - t);

                obj.localPosition = Vector3.Lerp(startPosition, endPosition, easeT);

                yield return null;
            }

            obj.localPosition = endPosition;
            onComplete?.Invoke();
        }
        
        public static IEnumerator Delay(float delayInSeconds, Action finishedCallback)
        {
            yield return new WaitForSeconds(delayInSeconds);
            finishedCallback();
        }
        
    }
}
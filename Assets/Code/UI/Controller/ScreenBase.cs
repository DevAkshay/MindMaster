using UnityEngine;

namespace Code.UI.Controller
{
    public abstract class ScreenBase : MonoBehaviour
    {
        public virtual void OnShow()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}
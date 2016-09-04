using UnityEngine;
using UnityEngine.UI;

namespace UnityCSCommon.Utils.UI
{
    /// <summary>
    /// Changes the graphic of <see cref="Toggle"/> depending on it's state.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleGraphicChanger : MonoBehaviour
    {
        [SerializeField] private Graphic _onGraphic;
        [SerializeField] private Graphic _offGraphic;

        void Start()
        {
            GetComponent<Toggle>().onValueChanged.AddListener (ChangeGraphic);
        }

        private void ChangeGraphic (bool value)
        {
            _onGraphic.gameObject.SetActive (value);
            _offGraphic.gameObject.SetActive (!value);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace _0.Custom.Scripts.Menu
{
    public class CarUI : MonoBehaviour
    {
        public int price;
        public Image background;
        public Button button;
        public GameObject equipped;
        public void Select(Color color)
        {
            background.color = color;
        }

        public void Equip(bool equip)
        {
            equipped.SetActive(equip);
        }
    }
}
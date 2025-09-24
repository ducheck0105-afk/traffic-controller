using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel
{
    public class PieceObject : MonoBehaviour
    {
        public Image backgroundImage;
        public Image rewardIcon;
        public Text rewardAmount;
        public DailySpin.RewardEnum rewardCategory;

        public int index;

        public void SetValues(int pieceNo)
        {
            index = pieceNo;

            if (DailySpin.ins.useCustomBackgrounds)
            {
                backgroundImage.color = Color.white;
                backgroundImage.sprite = DailySpin.ins.CustomBackgrounds[pieceNo];
            }
            else
            {
                backgroundImage.color = DailySpin.ins.PiecesOfWheel[pieceNo].backgroundColor;
                backgroundImage.sprite = DailySpin.ins.PiecesOfWheel[pieceNo].backgroundSprite;
            }

            rewardCategory = DailySpin.ins.PiecesOfWheel[pieceNo].rewardCategory;
            rewardAmount.text = DailySpin.ins.PiecesOfWheel[pieceNo].rewardAmount.ToString();

            for (int i = 0; i < DailySpin.ins.categoryIcons.Length; i++)
            {
                if (rewardCategory == DailySpin.ins.categoryIcons[i].category)
                {
                    rewardIcon.sprite = DailySpin.ins.categoryIcons[i].rewardIcon;
                    DailySpin.ins.PiecesOfWheel[pieceNo].rewardIcon = DailySpin.ins.categoryIcons[i].rewardIcon;
                }
            }
        }
    }
}
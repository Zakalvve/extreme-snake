using TMPro;
using UnityEngine;
using ExtremeSnake.Utils;

namespace ExtremeSnake.Game.UI
{
    [CreateAssetMenu(menuName = "My Assets/Multiplier Gradients")]
    public class MultiplierGradients : ScriptableObject
    {
        public TMP_ColorGradient Default;
        public TMP_ColorGradient Two;
        public TMP_ColorGradient Four;
        public TMP_ColorGradient Eight;
        public TMP_ColorGradient Sixteen;
        public TMP_ColorGradient ThirtyTwo;
        public TMP_ColorGradient SixtyFour;

        public TMP_ColorGradient GetGradient(int multiplier) {
            multiplier = UtilsClass.FloorPower2(multiplier);
            switch (multiplier) {
                case 2:
                    return Two;
                case 4: 
                    return Four;
                case 8:
                    return Eight;
                case 16:
                    return Sixteen;
                case 32:
                    return ThirtyTwo;
                case 64:
                    return SixtyFour;
                default:
                    return Default;
            }
        }
    }
}

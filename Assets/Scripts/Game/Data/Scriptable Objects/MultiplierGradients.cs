using TMPro;
using UnityEngine;

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

        public Color DefaultColor;
        public Color TwoColor;
        public Color FourColor;
        public Color EightColor;
        public Color SixteenColor;
        public Color ThirtyTwoColor;
        public Color SixtyFourColor;

        public TMP_ColorGradient GetGradient(int multiplier) {
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

        public Color GetFlatColor(int multiplier) {
            switch (multiplier) {
                case 1:
                    return Color.white;
                case 2:
                    return TwoColor;
                case 4:
                    return FourColor;
                case 8:
                    return EightColor;
                case 16:
                    return SixteenColor;
                case 32:
                    return ThirtyTwoColor;
                case 64:
                    return SixtyFourColor;
                default:
                    return DefaultColor;
            }
        }
    }
}

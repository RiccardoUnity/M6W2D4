using UnityEngine;

namespace GameManagerStatic
{
    public static class Main
    {
        public static class StringManager
        {
            public static string InputOption() => "Cancel";

            public static string InputHorizontal() => "Horizontal";

            public static string InputVertical() => "Vertical";

            public static string InputSilentMode() => "SilentMode";

            public static string InputSprint() => "Sprint";

            public static string InputJump() => "Jump";

            public static string InputMouseX() => "Mouse X";

            public static string InputMouseY() => "Mouse Y";

            public static string ParAnimatorIsSilent() => "isSilent";

            public static string ParAnimatorHSpeed() => "hSpeed";

            public static string ParAnimatorVSpeed() => "vSpeed";

            public static string ParAnimatorJump() => "jump";

            public static string ParAnimatorIsGrounded() => "isGrounded";

            public static string TagPlayer() => "Player";

            public static string TagBox() => "Box";

            public static string PathAudioManager() => "AudioManager";
        }

        public static class ColorManager
        {
            // FFFFFF
            public static Color White() => new Color(1f, 1f, 1f, 1f);
            // EB0000
            public static Color Red() => new Color(0.921f, 0f, 0f, 1f);
            // FF7D00
            public static Color Orange() => new Color(1f, 0.490f, 0f, 1f);
            // FFDB00
            public static Color Yellow() => new Color(1f, 0.859f, 0f, 1f);
            // 00B800
            public static Color Green() => new Color(0f, 0.722f, 0f, 1f);
            // 0080FF
            public static Color Azure() => new Color(0f, 0.502f, 1f, 1f);
            // 0033BD
            public static Color Blue() => new Color(0f, 0.2f, 0.741f, 1f);
            // 9100D4
            public static Color Purple() => new Color(0.569f, 0f, 0.831f, 1f);
            // 242424
            public static Color Black() => new Color(0.141f, 0.141f, 0.141f, 1f);

            private static Color[] _colors = {White(), Red(), Orange(), Yellow(), Green(), Azure(), Blue(), Purple(), Black()};
            private static Color _sceneColor = Azure();
            private static Color _lightSceneColor = White();

            //Scelgo un colore random escludendo il primo e ultimo valore
            public static void SetSceneColor()
            {
                Color color;
                do
                {
                    color = _colors[Random.Range(1, _colors.Length - 1)];
                } while (color == _sceneColor);
                _sceneColor = color;
            }
            public static Color GetSceneColor() => _sceneColor;

            public static void SetLightSceneColor() => _lightSceneColor = Color.Lerp(White(), _sceneColor, 0.25f);
            public static Color GetLightSceneColor() => _lightSceneColor;
        }
    }
}

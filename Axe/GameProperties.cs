using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace com.dreamwagon.axe
{
    public static class GameProperties
    {
        /** The area that is always visible regardless of display size**/
        /** On certain displays, content outside this rectangle may be clipped**/
        public static Rectangle TitleSafeRect;

        /** The player who has control of the game. **/
        /** all other controllers can be 'rogue' **/
        public static PlayerIndex controllingPlayer;

        /** Variable to allow sound effects to be toggled off **/
        public static bool SoundEffects = true;

        public static bool IsSavingProgress = true;

        /// <summary>
        /// Table of Colors some colors...
        /// </summary>
        public static Color[] SomeColors =
        {
            //
            Color.White,
            new Color(136,174,255, 255 ), //Light blue
            new Color(255,136,192, 255 ), //Light pink
            new Color(136,255,136, 255 ), //Light green
            new Color(255,251,136, 255 ), //Light yellow
            new Color(206,136,255, 255 ), //Light purple
            new Color(255,161,136, 255 ), //Light orange-red
        };


        public static Vector2 ScreenCenterPoint = new Vector2();
        public static Vector2 ScreenCenterTopPoint = new Vector2();
        public static Vector2 ScreenCenterBottomPoint = new Vector2();
    }
}

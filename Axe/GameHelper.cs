using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

namespace com.dreamwagon.axe
{
    public static class GameHelper
    {
        public static void SetScreenCenterPoint( GraphicsDevice g )
        {
            GameProperties.ScreenCenterPoint = new Vector2(g.Viewport.TitleSafeArea.Center.X, g.Viewport.TitleSafeArea.Center.Y);
            GameProperties.ScreenCenterTopPoint = new Vector2(g.Viewport.TitleSafeArea.Center.X, g.Viewport.TitleSafeArea.Top);
            GameProperties.ScreenCenterBottomPoint = new Vector2(g.Viewport.TitleSafeArea.Center.X, g.Viewport.TitleSafeArea.Bottom);
        }


        public static String getOnOffStringForBool(bool b)
        {
            return b ? "On" : "Off";
        }
    }

    /**
     * Returns true if the current player is signed into a Live account, and
     * Has the privileges to purchase the game.
     */
    public static class PlayerIndexExtensions
    {
        public static bool CanBuyGame(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];

            if (gamer == null)
                return false;

            if (!gamer.IsSignedInToLive)
                return false;

            return gamer.Privileges.AllowPurchaseContent;
        }

        /**
         * Returns a name from the gamer signed into the console.
         */
        public static String ValidGamerTag(this PlayerIndex player)
        {
            Gamer g = Gamer.SignedInGamers[player];
            String tagName = "Guest" + (int)player;
            if (!(g == null))
                tagName = g.Gamertag;

            return tagName;
        }


        /**
         * Returns true if the user is signed in to the console, false if not signed in
         */
        public static bool CanSaveOrLoadGame(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];

            if (gamer == null)
                return false;
            else if (gamer.IsGuest)
                return false;
            else
                return true;

        }


        /**
         * I dont think we will need this, but in case we do....
         */
        public static bool IsAllowedOnlineSession(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];
            
            if (gamer == null)
                return false;
            else if (gamer.IsSignedInToLive && gamer.Privileges.AllowOnlineSessions)
                return true;
            else
                return false;
        }

        public static bool IsSignedInToLIVE(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];

            if (gamer == null)
                return false;
            else if (gamer.IsSignedInToLive)
                return true;
            else
                return false;
        }
    }
}

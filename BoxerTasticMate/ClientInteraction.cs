/*
 * This method will deal with all interaction with the game clients
 * e.g. login
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices; // used for mouse detection

namespace BoxerTasticMate
{
    class ClientInteraction
    {
        static Random ranbomise;

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool BringWindowToTop(IntPtr hWnd);




        /*
         * Used to acquire the X coordinate of the game client as it would be on a screen other than the one it
         * is currently on.
         */
        private static int acquireRealWorldXCoordinate(MyScreen screenGameWindow, int gameClientIdx, int panelScreenWidth)
        {
            // work out % of screen across that the game window's X coord is
            double percentageAcross = screenGameWindow.xCoord[gameClientIdx] * 100;
            percentageAcross = percentageAcross / panelScreenWidth;
            double remainder = percentageAcross % 1;

            // now figure out the equivalent on the real world screen
            double xCoordinate = percentageAcross * screenGameWindow.screenWidth;
            xCoordinate = xCoordinate / 100;
            xCoordinate += screenGameWindow.screenXCoord;

            if (remainder > 0)
            {
                xCoordinate += 1;
            }

            return Convert.ToInt32(xCoordinate);
        }

        /*
         * Used to acquire the Y coordinate of the game client as it would be on a screen other than the one it
         * is currently on.
         */
        private static int acquireRealWorldYCoordinate(int clientHeightOrWidth, int clientCoordinate, 
            int defaultClientHeightWidth, int defaultClientCoordinate, int coordinateWeWishToTranslate)
        {
            // work out % of screen across that the game window's Y coord is
            double percentage = (coordinateWeWishToTranslate - defaultClientCoordinate) * 100;
            percentage = percentage / defaultClientHeightWidth;
            double remainder = percentage % 1;

            // now figure out the equivalent on the real world screen
            double yCoordinate = percentage * clientHeightOrWidth;
            yCoordinate = yCoordinate / 100;
            yCoordinate += clientCoordinate;

            if (remainder > 0)
            {
                yCoordinate += 1;
            }

            return Convert.ToInt32(yCoordinate);
        }

        /*
         * This method logs you into the game client
         * 
         * currentClient - provides the height, width, and coordinates of the game client
         */
        public static void loginToClient(MyScreen currentScreen, int currentClient)
        {
            ranbomise = new Random();

            // make client active
            SetForegroundWindow(currentScreen.getProcess(currentClient).MainWindowHandle);

            // acquire X and Y coordinates
            int xCoordTopLeft = acquireRealWorldYCoordinate(currentScreen.getGameClientWidth(currentClient),
                currentScreen.getGameClientXCoord(currentClient), Coordinates.defaultWidthEve, 
                Coordinates.defaultXCoordEve, Coordinates.accountNameTopLeftXcoord);

            int xCoordBottomRight = acquireRealWorldYCoordinate(currentScreen.getGameClientWidth(currentClient),
                currentScreen.getGameClientXCoord(currentClient), Coordinates.defaultWidthEve,
                Coordinates.defaultXCoordEve, Coordinates.accountNameBottonRightXcoord);

            int yCoordTopLeft = acquireRealWorldYCoordinate(currentScreen.getGameClientHeight(currentClient), 
                Coordinates.accountNameTopLeftYcoord, Coordinates.defaultHeightEve,
                Coordinates.defaultYCoordEve, Coordinates.accountNameTopLeftYcoord);

            int yCoordBottomRight = acquireRealWorldYCoordinate(currentScreen.getGameClientHeight(currentClient),
                Coordinates.accountNameTopLeftYcoord, Coordinates.defaultHeightEve,
                Coordinates.defaultYCoordEve, Coordinates.accountNameBottonRightYcoord);

            // now take those coords and randomly choose and X and Y coordinate from that range
            int x = ranbomise.Next(xCoordTopLeft, xCoordBottomRight);
            int y = ranbomise.Next(yCoordTopLeft, yCoordBottomRight);

            // move mouse to account name
            MouseHandling.mouseMove(x, y);

            // select account entry box

            // highlight account name area 

            // send account name

            // move mouse to password entry box

            // highlight password area

            // send password

            // hit RETURN or click on login button

        }
    }
}

/*
 * This class deals with the game windows on the config area (panel)
 * The methods manipulate the game window controls.
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; // Panel control

using System.Drawing; // Point object - screen coordinates


namespace BoxerTasticMate
{
    class GameWindow
    {
        // Resizing game window 
        // ====================
        public static bool resizing = false;
        public static bool topLeftResizing = false;
        public static bool topRightResizing = false;
        public static bool bottomLeftResizing = false;
        public static bool bottomRightResizing = false;
        public static bool leftResizing = false;
        public static bool rightResizing = false;
        public static bool topResizing = false;
        public static bool bottomResizing = false;

        private static int topRightCornerXCoord;
        private static int topRightCornerYCoord;
        private static int topLeftCornerXCoord;
        private static int topLeftCornerYCoord;
        private static int bottomRightCornerXCoord;
        private static int bottomRightCornerYCoord;
        private static int bottomLeftCornerXCoord;
        private static int bottomLeftCornerYCoord;
        private static int topXCoord;
        private static int topYCoord;
        private static int bottomXCoord;
        private static int bottomYCoord;
        private static int leftXCoord;
        private static int leftYCoord;
        private static int rightXCoord;
        private static int rightYCoord;


        // used in the moving of game window object
        public static int mousePrevXCoord, mousePrevYCoord;


        

        /*
         * The value returned repesents either 
         * 1 top, 2 bottom, 3 left, 4 right
         * 5 top-right, 6 top-left, 7 bottom-right, 8 bottom-left
         * 
         */
        public static int getGameWindowEntryPoint(int currentGameWindow, int xCoordOfGUI, int yCordOfGUI, int panelScreenXcoord, int panelScreenYcoord, List<Panel> gameWindows)
        {
            // make sure w don;t crash due to not finding the game window (returns 999)
            if (currentGameWindow != 999)
            {
                // get panelScreen's coords
                int panelScreenScreenXCoord = xCoordOfGUI + panelScreenXcoord + 8; // this.Location.X
                int panelScreenScreenYCoord = yCordOfGUI + panelScreenYcoord  + 30; // this.Location.Y
                // get gameWindow's coords
                int gameWindowScreenXCoord = panelScreenScreenXCoord + gameWindows[currentGameWindow].Location.X;
                int gameWindowScreenYCoord = panelScreenScreenYCoord + gameWindows[currentGameWindow].Location.Y;

                // work out the bounds of the entry areas
                // BOTTON RIGHT CORNER
                bottomRightCornerXCoord = gameWindowScreenXCoord + (gameWindows[currentGameWindow].Width - 20);
                bottomRightCornerYCoord = gameWindowScreenYCoord + (gameWindows[currentGameWindow].Height - 20);
                // BOTTON RIGHT CORNER
                bottomLeftCornerXCoord = gameWindowScreenXCoord;
                bottomLeftCornerYCoord = gameWindowScreenYCoord + (gameWindows[currentGameWindow].Height - 20);
                // TOP LEFT CORNER
                topLeftCornerXCoord = gameWindowScreenXCoord;
                topLeftCornerYCoord = gameWindowScreenYCoord;
                // TOP RIGHT CORNER
                topRightCornerXCoord = gameWindowScreenXCoord + (gameWindows[currentGameWindow].Width - 20);
                topRightCornerYCoord = gameWindowScreenYCoord;

                // TOP
                topXCoord = gameWindowScreenXCoord + 21;
                topYCoord = gameWindowScreenYCoord;
                // BOTTOM
                bottomXCoord = gameWindowScreenXCoord + 21;
                bottomYCoord = gameWindowScreenYCoord + (gameWindows[currentGameWindow].Height - 21);
                // LEFT
                leftXCoord = gameWindowScreenXCoord;
                leftYCoord = gameWindowScreenYCoord + 21;
                // RIGHT
                rightXCoord = gameWindowScreenXCoord + (gameWindows[currentGameWindow].Width - 21);
                rightYCoord = gameWindowScreenYCoord + 21;

                // Has the mouse entered the bottom right corner ?
                if ((Cursor.Position.X >= bottomRightCornerXCoord && Cursor.Position.X <= bottomRightCornerXCoord + 20) &&    // bottom right
                    (Cursor.Position.Y >= bottomRightCornerYCoord && Cursor.Position.Y <= bottomRightCornerYCoord + 20)     // bottom right
                )
                {
                    // change cursor to resize from bottom corner
                    return 7; // bottom right corner
                }
                else if ((Cursor.Position.X >= bottomLeftCornerXCoord && Cursor.Position.X <= bottomLeftCornerXCoord + 20) &&    // bottom left
                    (Cursor.Position.Y >= bottomLeftCornerYCoord && Cursor.Position.Y <= bottomLeftCornerYCoord + 20))    // bottom left
                {
                    return 8; // bottom left corner
                }
                else if ((Cursor.Position.X >= topLeftCornerXCoord && Cursor.Position.X <= topLeftCornerXCoord + 20) &&    // top left
                    (Cursor.Position.Y >= topLeftCornerYCoord && Cursor.Position.Y <= topLeftCornerYCoord + 20))    // top left
                {
                    return 6; // top left corner 
                }
                else if ((Cursor.Position.X >= topRightCornerXCoord && Cursor.Position.X <= topRightCornerXCoord + 20) &&    // top right
                    (Cursor.Position.Y >= topRightCornerYCoord && Cursor.Position.Y <= topRightCornerYCoord + 20))    // top right
                {
                    return 5; // top right corner
                }
                else if ((Cursor.Position.X >= topXCoord && Cursor.Position.X <= topXCoord + (gameWindows[currentGameWindow].Width - 21)) &&    // top 
                    (Cursor.Position.Y >= topYCoord && Cursor.Position.Y <= topYCoord + 21))    // top 
                {
                    return 1; // top
                }
                else if ((Cursor.Position.X >= bottomXCoord && Cursor.Position.X <= bottomXCoord + (gameWindows[currentGameWindow].Width - 21)) &&    // bottom 
                    (Cursor.Position.Y >= bottomYCoord && Cursor.Position.Y <= bottomYCoord + 21))    // bottom 
                {
                    return 2; // bottom
                }
                else if ((Cursor.Position.X >= leftXCoord && Cursor.Position.X <= leftXCoord + 21) &&    // left 
                    (Cursor.Position.Y >= leftYCoord && Cursor.Position.Y <= leftYCoord + (gameWindows[currentGameWindow].Height - 21)))    // left 
                {
                    return 3; // left
                }
                else if ((Cursor.Position.X >= rightXCoord && Cursor.Position.X <= rightXCoord + 21) &&    // right 
                    (Cursor.Position.Y >= rightYCoord && Cursor.Position.Y <= rightYCoord + (gameWindows[currentGameWindow].Height - 21)))    // right 
                {
                    return 4; // right
                }
            }

            return 0;
        } // END OF getGameWindowEntryPoint()

        private static void setFlagsToFalse(int flagNotToReset)
        {
            if (flagNotToReset != 1)
                topResizing = false;
            if (flagNotToReset != 2)
                bottomResizing = false;
            if (flagNotToReset != 3)
                leftResizing = false;
            if (flagNotToReset != 4)
                rightResizing = false;
            if (flagNotToReset != 5)
                topRightResizing = false;
            if (flagNotToReset != 6)
                topLeftResizing = false;
            if (flagNotToReset != 7)
                bottomRightResizing = false;
            if (flagNotToReset != 8)
                bottomLeftResizing = false;
        }

        /*
         * this changes the cursor to the appropriate one for where the xursor is on the game window
         */
        public static void changeToAppropriateCursor(bool draggingObject, int currentGameWindow, int xCoordOfGUI, int yCordOfGUI, 
            int panelScreenXcoord, int panelScreenYcoord, List<Panel> gameWindows)
        {
            //      getGameWindowEntryPoint(int currentGameWindow, int xCoordOfGUI, int yCordOfGUI, int panelScreenXcoord, int panelScreenYcoord, List<Panel> gameWindows)
            int cursorLocation = getGameWindowEntryPoint(currentGameWindow, xCoordOfGUI, yCordOfGUI, panelScreenXcoord, panelScreenYcoord, gameWindows);

            switch (cursorLocation)
            {
                case 0:
                    setFlagsToFalse(99); // sending99 will result in all flags being set to false

                    if (draggingObject)
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                    else
                        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
                    break;
                case 1: // Top border
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                    topResizing = true;
                    setFlagsToFalse(1);
                    break;
                case 2: // Bottom border
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                    bottomResizing = true;
                    setFlagsToFalse(2);
                    break;
                case 3: // Left border
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    leftResizing = true;
                    setFlagsToFalse(3);
                    break;
                case 4: // Right border
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                    rightResizing = true;
                    setFlagsToFalse(4);
                    break;
                case 5: // top right corner
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                    topRightResizing = true;
                    setFlagsToFalse(5);
                    break;
                case 6: // top left corner
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                    topLeftResizing = true;
                    setFlagsToFalse(6);
                    break;
                case 7: // bottom right corner
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                    bottomRightResizing = true;
                    setFlagsToFalse(7);
                    break;
                case 8: // bottom left corner
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                    bottomLeftResizing = true;
                    setFlagsToFalse(8);
                    break;
            }

        } // END OF changeToAppropriateCursor()



        // ===============================================================
        // ==============  Game Window Identification  ===================
        // ===============================================================

        /*
         * This method works out which game window the cursor is currently over
         */
        public static int whichGameWindow(MyScreen currentScreen, int xCoordOfGUI, int yCordOfGUI, int panelScreenXcoord, int panelScreenYcoord)
        {
            // get panelScreen's coords
            int panelScreenScreenXCoord = xCoordOfGUI + panelScreenXcoord + 8;  // this.Location.X + panelScreen.Location.X
            int panelScreenScreenYCoord = yCordOfGUI + panelScreenYcoord + 30;

            // This needs altering to the total # of game windows on the current screen !!!!!!!
            int totalGameWindowsOnCurrentSCRN = currentScreen.getTotalNumberOfGameWindows();

            int[] currentLocationGameWindowList = new int[100];
            int count = 0;
            for (int i = 0; i < totalGameWindowsOnCurrentSCRN; i++)
            {
                // get gameWindow's actual screen coords (not coords within panel)
                int gameWindowScreenXCoord = panelScreenScreenXCoord + currentScreen.xCoord[i];
                int gameWindowScreenYCoord = panelScreenScreenYCoord + currentScreen.yCoord[i];
                int XXCord = gameWindowScreenXCoord;
                int YYCord = gameWindowScreenYCoord;
                int gameWindowWidth = currentScreen.width[i];
                int gameWindowHeight = currentScreen.height[i];

                // check cursor location with boundaries of each of the game windows
                if ((Cursor.Position.X >= XXCord) && (Cursor.Position.X <= (XXCord + gameWindowWidth))
                    && (Cursor.Position.Y >= YYCord) && (Cursor.Position.Y <= (YYCord + gameWindowHeight))
                    )
                {
                    // build up list of game windows in current location
                    currentLocationGameWindowList[count] = currentScreen.gameWindow[i];
                    count++;
                }
            }
            return currentScreen.getTopMostGameWindow(currentLocationGameWindowList, count);
        } // END OF whichGameWindow()


        // ===============================================================
        // ==============  Resizing The Game Window   ====================
        // ===============================================================

        /*
         * This method checks to see if we are attempting to resize a game window, and if so resizes it in relation to the mouse position on the
         * main screen
         */
        public static Panel resizeObjectCheck(int currentGameWindow, int xCoordOfGUI, int yCordOfGUI, int panelScreenXcoord, int panelScreenYcoord, 
            int panelScreenWidth, int panelScreenHeight, Panel gameWindows, int cursorPositionX, int cursorPositionY)
        {
            // get mouse position
            int mouseCurrentXCoord = cursorPositionX;
            int mouseCurrentYCoord = cursorPositionY;

            // has the current mouse location changed
            if ((mouseCurrentXCoord != mousePrevXCoord) || (mouseCurrentYCoord != mousePrevYCoord))
            {
                // get panelScreen's coords
                int panelScreenScreenXCoord = xCoordOfGUI + panelScreenXcoord;  // this.Location.X + panelScreen.Location.X
                int panelScreenScreenYCoord = yCordOfGUI + panelScreenYcoord;  // this.Location.Y + panelScreen.Location.Y
                // get gameWindow's coords
                int gameWindowScreenXCoord = panelScreenScreenXCoord + gameWindows.Location.X;
                int gameWindowScreenYCoord = panelScreenScreenYCoord + gameWindows.Location.Y;

                int panelScreenScreenRightBorder = panelScreenScreenXCoord + panelScreenWidth; // panelScreen.Width

                // store the X and Y coord difference of the current and previous mouse coords
                int mouseXCoordDiff = mouseCurrentXCoord - mousePrevXCoord;
                int mouseYCoordDiff = mouseCurrentYCoord - mousePrevYCoord;
                mousePrevXCoord = mouseCurrentXCoord;
                mousePrevYCoord = mouseCurrentYCoord;

                int newHeight = 0;
                int newWidth = 0;
                int newXCoord;
                int newYCoord;
                // which direction
                if (rightResizing)
                {
                    // calculate the new width by using the mouseXCoordDiff
                    newWidth = gameWindows.Width + mouseXCoordDiff;

                    // Make the changes
                    if (((gameWindowScreenXCoord + gameWindows.Width) + mouseXCoordDiff) < (panelScreenScreenXCoord + panelScreenWidth))
                        gameWindows.Width = newWidth;

                }
                else if (bottomResizing)
                {
                    // calculate the new width by using the mouseXCoordDiff
                    newHeight = gameWindows.Height + mouseYCoordDiff;

                    // Make the changes
                    // only resize the height if it does not pass the Y coord of the screen panel
                    if (((gameWindowScreenYCoord + gameWindows.Height) + mouseYCoordDiff) < (panelScreenScreenYCoord + panelScreenHeight))
                        gameWindows.Height = newHeight;

                }
                else if (bottomRightResizing)
                {
                    // calculate the new height by using the mouseXCoordDiff
                    newHeight = gameWindows.Height + mouseYCoordDiff;
                    // calculate the new width by using the mouseXCoordDiff
                    newWidth = gameWindows.Width + mouseXCoordDiff;

                    // Make the changes
                    // resize width check
                    if (((gameWindowScreenXCoord + gameWindows.Width) + mouseXCoordDiff) < (panelScreenScreenXCoord + panelScreenWidth))
                        gameWindows.Width = newWidth;
                    // resize height check
                    if (((gameWindowScreenYCoord + gameWindows.Height) + mouseYCoordDiff) < (panelScreenScreenYCoord + panelScreenHeight))
                        gameWindows.Height = newHeight;
                }
                else if (topResizing)
                {
                    // calculate the new width by using the mouseXCoordDiff
                    newHeight = gameWindows.Height - mouseYCoordDiff;
                    // calculate the new Y coordinate, as the game window is being stretched up
                    newXCoord = gameWindows.Location.X;
                    newYCoord = gameWindows.Location.Y + mouseYCoordDiff;

                    // Make the changes
                    // only resize the height if it does not pass the Y coord of the screen panel
                    if ((gameWindowScreenYCoord + mouseYCoordDiff) > (panelScreenScreenYCoord))
                    {
                        gameWindows.Height = newHeight;
                        gameWindows.Location = new Point(newXCoord, newYCoord);
                    }
                }
                else if (leftResizing)
                {
                    // calculate the new width by using the mouseXCoordDiff
                    newWidth = gameWindows.Width - mouseXCoordDiff;
                    // calculate the new X coordinate, as the game window is being stretched up
                    newXCoord = gameWindows.Location.X + mouseXCoordDiff;
                    newYCoord = gameWindows.Location.Y;

                    // Make the changes
                    if ((gameWindowScreenXCoord + mouseXCoordDiff) > (panelScreenScreenXCoord))
                    {
                        gameWindows.Width = newWidth;
                        gameWindows.Location = new Point(newXCoord, newYCoord);
                    }
                }
                else if (bottomLeftResizing)
                {
                    // alter the height by using the mouseYCoordDiff
                    newHeight = gameWindows.Height + mouseYCoordDiff;
                    // Make the changes
                    if (((gameWindowScreenYCoord + gameWindows.Height) + mouseYCoordDiff) < (panelScreenScreenYCoord + panelScreenHeight))
                        gameWindows.Height = newHeight;
                    // only resize the height if it does not pass the Y coord of the screen panel
                    if ((gameWindowScreenXCoord + mouseXCoordDiff) > (panelScreenScreenXCoord))
                    {
                        ;
                        // Make the changes
                        // alter the width by using the mouseXCoordDiff
                        newWidth = gameWindows.Width - mouseXCoordDiff;
                        gameWindows.Width = newWidth;
                        // now alter the X coordinate, as the game window is being stretched right to left and left to right
                        newXCoord = gameWindows.Location.X + mouseXCoordDiff;
                        newYCoord = gameWindows.Location.Y;
                        gameWindows.Location = new Point(newXCoord, newYCoord);
                    }
                }
                else if (topLeftResizing)
                {
                    // alter the height by using the mouseYCoordDiff
                    newHeight = gameWindows.Height - mouseYCoordDiff;
                    // only resize the height if it does not pass the Y coord of the screen panel
                    if ((gameWindowScreenYCoord + mouseYCoordDiff) > (panelScreenScreenYCoord))
                    {
                        newYCoord = gameWindows.Location.Y + mouseYCoordDiff;
                        gameWindows.Height = newHeight;
                    }
                    else
                        newYCoord = gameWindows.Location.Y; // do not allow the game window to move over the boundary


                    // alter the width by using the mouseXCoordDiff
                    newWidth = gameWindows.Width - mouseXCoordDiff;
                    if ((gameWindowScreenXCoord + mouseXCoordDiff) > (panelScreenScreenXCoord))
                    {
                        // now alter the X coordinate, as the game window is being stretched up
                        newXCoord = gameWindows.Location.X + mouseXCoordDiff;
                        gameWindows.Width = newWidth;
                    }
                    else
                        newXCoord = gameWindows.Location.X; // do not allow the game window to move over the boundary

                    // make the change
                    gameWindows.Location = new Point(newXCoord, newYCoord);
                }
                else if (topRightResizing)
                {
                    // alter the width by using the mouseXCoordDiff
                    newWidth = gameWindows.Width + mouseXCoordDiff;
                    // Make the changes
                    if (((gameWindowScreenXCoord + gameWindows.Width) + mouseXCoordDiff) < (panelScreenScreenXCoord + panelScreenWidth))
                        gameWindows.Width = newWidth;
                    // only resize the height if it does not pass the Y coord of the screen panel
                    if ((gameWindowScreenYCoord + mouseYCoordDiff) > (panelScreenScreenYCoord))
                    {
                        // Make the changes
                        // alter the height by using the mouseYCoordDiff
                        newHeight = gameWindows.Height - mouseYCoordDiff;
                        gameWindows.Height = newHeight;
                        // now alter the Y coordinate, as the game window is being stretched up
                        newXCoord = gameWindows.Location.X;
                        newYCoord = gameWindows.Location.Y + mouseYCoordDiff;
                        gameWindows.Location = new Point(newXCoord, newYCoord);
                    }
                }

            }

            // RETURN gameWindows  *****************************
            return gameWindows;

        } // END OF resizeObjectCheck()


        // ==============================================================
        // ==============   Moving The Game Window   ====================
        // ==============================================================

        /*
         * This method checks to see if we are attempting to move a game window, and if so moves it in relation to the mouse position on the
         * main screen
         */
        public static Panel moveObjectCheck(int xCoordOfGUI, int yCordOfGUI, int panelScreenXcoord, int panelScreenYcoord,
            int panelScreenWidth, int panelScreenHeight, Panel gameWindow)
        {
            // get mouse position
            int mouseCurrentXCoord = Cursor.Position.X;
            int mouseCurrentYCoord = Cursor.Position.Y;

            // has the current mouse location changed
            if ((mouseCurrentXCoord != mousePrevXCoord) || (mouseCurrentYCoord != mousePrevYCoord))
            {
                // get panelScreen's coords
                int panelScreenScreenXCoord = xCoordOfGUI + panelScreenXcoord + 8; // this.Location.X + panelScreen.Location.X 
                int panelScreenScreenYCoord = yCordOfGUI + panelScreenYcoord + 30;
                // get gameWindow's coords
                int gameWindowScreenXCoord = panelScreenScreenXCoord + gameWindow.Location.X;
                int gameWindowScreenYCoord = panelScreenScreenYCoord + gameWindow.Location.Y;

                int panelScreenScreenRightBorder = panelScreenScreenXCoord + panelScreenWidth;

                // store the X and Y coord difference of the current and previous mouse coords
                int mouseXCoordDiff = mouseCurrentXCoord - mousePrevXCoord;
                int mouseYCoordDiff = mouseCurrentYCoord - mousePrevYCoord;
                mousePrevXCoord = mouseCurrentXCoord;
                mousePrevYCoord = mouseCurrentYCoord;

                // have we moved beyond the boundaries of the virtual screen (panelScreen) ?
                // UP && DOWN && LEFT && RIGHT
                if (((gameWindowScreenXCoord + mouseXCoordDiff) > panelScreenScreenXCoord) // can go LEFT

                    && ((gameWindowScreenXCoord + gameWindow.Width + mouseXCoordDiff) < (panelScreenScreenXCoord + panelScreenWidth) // RIGHT?

                    && ((gameWindowScreenYCoord + mouseYCoordDiff) > panelScreenScreenYCoord) // can go UP

                    && ((gameWindowScreenYCoord + gameWindow.Height + mouseYCoordDiff) < (panelScreenScreenYCoord + panelScreenHeight)) )) // DOWN ?
                {
                    // move the game window

                    // change control's location
                    int X = gameWindow.Location.X + mouseXCoordDiff;
                    int Y = gameWindow.Location.Y + mouseYCoordDiff;
                    gameWindow.Location = new Point(X, Y);

                    /*
                    // update these details in the myScreen object
                    int idx = myScreens[currentScreen].getGameWindowScreenIdx(currentGameWindow);
                    myScreens[currentScreen].xCoord[idx] = X;
                    myScreens[currentScreen].yCoord[idx] = Y;
                     */

                }

            }
            return gameWindow;
        } // END OF moveObjectCheck()


        // ==============================================================
        // ===============     DELETE - Not Used     ====================
        // ==============================================================

        /*
         * this return the location of the cursor within a game window itself
         * 1 top, 2 bottom, 3 left, 4 right
         * 5 top-right, 6 top-left, 7 bottom-right, 8 bottom-left
         * 0 centre
         */
        private int getCursorGameWindowLocation(int currentGameWindow, Panel gameWindow, int xCoordOfGUI, int yCordOfGUI,
            int panelScreenXcoord, int panelScreenYcoord)
        {
            // make sure w don;t crash due to not finding the game window (returns 999)
            if (currentGameWindow != 999)
            {
                // get panelScreen's coords
                int panelScreenScreenXCoord = xCoordOfGUI + panelScreenXcoord + 8;  // this.Location.X + panelScreen.Location.X
                int panelScreenScreenYCoord = yCordOfGUI + panelScreenYcoord + 30;
                // get gameWindow's coords
                int gameWindowScreenXCoord = panelScreenScreenXCoord + gameWindow.Location.X;
                int gameWindowScreenYCoord = panelScreenScreenYCoord + gameWindow.Location.Y;


                // work out the bounds of the entry areas
                // BOTTON RIGHT CORNER
                bottomRightCornerXCoord = gameWindowScreenXCoord + (gameWindow.Width - 10);
                bottomRightCornerYCoord = gameWindowScreenYCoord + (gameWindow.Height - 10);
                // BOTTON LEFT CORNER
                bottomLeftCornerXCoord = gameWindowScreenXCoord;
                bottomLeftCornerYCoord = gameWindowScreenYCoord + (gameWindow.Height - 10);
                // TOP LEFT CORNER
                topLeftCornerXCoord = gameWindowScreenXCoord;
                topLeftCornerYCoord = gameWindowScreenYCoord;
                // TOP RIGHT CORNER
                topRightCornerXCoord = gameWindowScreenXCoord + (gameWindow.Width - 10);
                topRightCornerYCoord = gameWindowScreenYCoord;

                // TOP
                topXCoord = gameWindowScreenXCoord + 11;
                topYCoord = gameWindowScreenYCoord;
                // BOTTOM
                bottomXCoord = gameWindowScreenXCoord + 11;
                bottomYCoord = gameWindowScreenYCoord + (gameWindow.Height - 10);
                // LEFT
                leftXCoord = gameWindowScreenXCoord;
                leftYCoord = gameWindowScreenYCoord + 11;
                // RIGHT
                rightXCoord = gameWindowScreenXCoord + (gameWindow.Width - 10);
                rightYCoord = gameWindowScreenYCoord + 11;

                // Has the mouse entered the bottom right corner ?
                if ((Cursor.Position.X >= bottomRightCornerXCoord && Cursor.Position.X <= bottomRightCornerXCoord + 23) &&    // bottom right
                    (Cursor.Position.Y >= bottomRightCornerYCoord && Cursor.Position.Y <= bottomRightCornerYCoord + 23))    // bottom right
                {
                    // change cursor to resize from bottom corner
                    return 7;
                }
                else if ((Cursor.Position.X >= bottomLeftCornerXCoord && Cursor.Position.X <= bottomLeftCornerXCoord + 10) &&    // bottom left
                    (Cursor.Position.Y >= bottomLeftCornerYCoord && Cursor.Position.Y <= bottomLeftCornerYCoord + 23))    // bottom left
                {
                    return 8; // centre area (draggable) of the game window
                }
                else if ((Cursor.Position.X >= topLeftCornerXCoord && Cursor.Position.X <= topLeftCornerXCoord + 10) &&    // top left
                    (Cursor.Position.Y >= topLeftCornerYCoord && Cursor.Position.Y <= topLeftCornerYCoord + 10))    // top left
                {
                    return 5; // bottom left corner
                }
                else if ((Cursor.Position.X >= topRightCornerXCoord && Cursor.Position.X <= topRightCornerXCoord + 23) &&    // top right
                    (Cursor.Position.Y >= topRightCornerYCoord && Cursor.Position.Y <= topRightCornerYCoord + 10))    // top right
                {
                    return 6; // top right corner
                }
                else if ((Cursor.Position.X >= topXCoord && Cursor.Position.X <= topXCoord + (gameWindow.Width - 10)) &&    // top 
                    (Cursor.Position.Y >= topYCoord && Cursor.Position.Y <= topYCoord + 5))    // top 
                {
                    return 1; // top
                }
                else if ((Cursor.Position.X >= bottomXCoord && Cursor.Position.X <= bottomXCoord + (gameWindow.Width - 10)) &&    // bottom 
                    (Cursor.Position.Y >= bottomYCoord && Cursor.Position.Y <= bottomYCoord + 23))    // bottom 
                {
                    return 2; // bottom
                }
                else if ((Cursor.Position.X >= leftXCoord && Cursor.Position.X <= leftXCoord + 10) &&    // left 
                    (Cursor.Position.Y >= leftYCoord && Cursor.Position.Y <= leftYCoord + (gameWindow.Height - 10)))    // left 
                {
                    return 3; // left
                }
                else if ((Cursor.Position.X >= rightXCoord && Cursor.Position.X <= rightXCoord + 23) &&    // right 
                    (Cursor.Position.Y >= rightYCoord && Cursor.Position.Y <= rightYCoord + (gameWindow.Height - 10)))    // right 
                {
                    return 4; // right
                }

            }

            return 0; // draggable are of game window
        } // END OF getCursorGameWindowLocation()



    }
}

/*
 * This class deals with the screens in the real world, and the layout of the game clients.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices; // used for mouse detection and WINAPI functions
using System.Diagnostics; // required for access to the list of processes

namespace BoxerTasticMate
{
    class RealWorldScreenManipulation
    {
        public static IntPtr HWND_TOPMOST = (IntPtr)(-1);


        public const int SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        private extern static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        /*
         * Used to acquire the height of the game client as it would be on a screen other than the one it
         * is currently on.
         * 
         * gameWindowHeight - height of the screen representation within the config window
         * screenHeight - real world screen height
         * panelScreenHeight - height of the config area/screen within the GUI
         */
        private static int getScreenHeight(int gameWindowHeight, int screenHeight, int panelScreenHeight)
        {
            // calculate to actual height required for the game client
            double gameClientHeightPercentage = gameWindowHeight * 100;
            gameClientHeightPercentage = gameClientHeightPercentage / panelScreenHeight;
            double remainder = gameClientHeightPercentage % 1;

            double gameClientHeight = gameClientHeightPercentage * screenHeight;
            gameClientHeight = gameClientHeight / 100;

            if (remainder > 0)
            {
                gameClientHeight += 1;
            }

            return Convert.ToInt32(gameClientHeight);
        }
        /*
         * Used to acquire the width of the game client as it would be on a screen other than the one it
         * is currently on.
         */
        private static int getScreenWidth(int gameWindowWidth, int screenWidth, int panelScreenWidth)
        {
            // calculate to actual width required for the game client
            double gameClientWidthPercentage = gameWindowWidth * 100;
            gameClientWidthPercentage = gameClientWidthPercentage / panelScreenWidth;
            double remainder = gameClientWidthPercentage % 1;

            double gameClientWidth = gameClientWidthPercentage * screenWidth;
            gameClientWidth = gameClientWidth / 100;

            if (remainder > 0)
            {
                gameClientWidth += 1;
            }

            return Convert.ToInt32(gameClientWidth);
        }


        /*
         * This alters the diemensions and coordinates of game clients that are currently displayed on the screen
         */
        public static void alterGameClientLayout(MyScreen screenGameWindow)
        {
            for (int gameClientIdx = 0; gameClientIdx < screenGameWindow.getTotalNumberOfGameWindows(); gameClientIdx++)
            {
                Process gameClientProc = screenGameWindow.getProcess(gameClientIdx);

                // Now to change the location and size of the client window, if I can :-)
                if (gameClientProc.WaitForInputIdle(15000))
                    SetWindowPos(gameClientProc.MainWindowHandle, HWND_TOPMOST, screenGameWindow.getGameClientXCoord(gameClientIdx),
                        screenGameWindow.getGameClientYCoord(gameClientIdx), screenGameWindow.getGameClientWidth(gameClientIdx),
                        screenGameWindow.getGameClientHeight(gameClientIdx), SWP_SHOWWINDOW);
            }
        }

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
        private static int acquireRealWorldYCoordinate(MyScreen screenGameWindow, int gameClientIdx, int panelScreenHeight)
        {
            // work out % of screen across that the game window's Y coord is
            double percentageDown = screenGameWindow.yCoord[gameClientIdx] * 100;
            percentageDown = percentageDown / panelScreenHeight;
            double remainder = percentageDown % 1;

            // now figure out the equivalent on the real world screen
            double yCoordinate = percentageDown * screenGameWindow.screenHeight;
            yCoordinate = yCoordinate / 100;
            yCoordinate += screenGameWindow.screenYCoord;

            if (remainder > 0)
            {
                yCoordinate += 1;
            }

            return Convert.ToInt32(yCoordinate);
        }


        /*
         * This alters the diemensions and coordinates of game clients when they have been assigned to another screen
         */
        public static void moveGameClientLayout(MyScreen screenGameWindow, int panelScreenHeight, int panelScreenWidth)
        {
            for (int gameClientIdx = 0; gameClientIdx < screenGameWindow.getTotalNumberOfGameWindows(); gameClientIdx++)
            {
                // translate the old game client's dimensions and coordinates to the new screen
                // - HEIGHT & WIDTH
                int gameWindowHeight = getScreenHeight(screenGameWindow.height[gameClientIdx],
                    screenGameWindow.screenHeight, panelScreenHeight);
                int gameWindowWidth = getScreenWidth(screenGameWindow.width[gameClientIdx],
                    screenGameWindow.screenWidth, panelScreenWidth);
                screenGameWindow.setGameClientHeight(gameClientIdx, gameWindowHeight);
                screenGameWindow.setGameClientWidth(gameClientIdx, gameWindowWidth);
                // - X & Y COORDIATES
                // work out where the game window is, then translate that to the real world screen
                int gameClientXCoord = acquireRealWorldXCoordinate(screenGameWindow, gameClientIdx, panelScreenWidth);
                int gameClientYCoord = acquireRealWorldYCoordinate(screenGameWindow, gameClientIdx, panelScreenHeight);
                screenGameWindow.setGameClientXCoord(gameClientIdx, gameClientXCoord);
                screenGameWindow.setGameClientYCoord(gameClientIdx, gameClientYCoord);

                // Now to change the location and size of the client window, if I can :-)
                // only run this if game clients currently exist on the real world screen
                if (screenGameWindow.gameClientsExist)
                {
                    Process gameClientProc = screenGameWindow.getProcess(gameClientIdx);

                    if (gameClientProc.WaitForInputIdle(15000))
                        SetWindowPos(gameClientProc.MainWindowHandle, HWND_TOPMOST, screenGameWindow.getGameClientXCoord(gameClientIdx),
                            screenGameWindow.getGameClientYCoord(gameClientIdx), screenGameWindow.getGameClientWidth(gameClientIdx),
                            screenGameWindow.getGameClientHeight(gameClientIdx), SWP_SHOWWINDOW);
                }
            }
        }


        /*
         * This sets the dimensions and coordinates of game clients.
         * Used when the clients existed but were closed, but the game windows (config screen in GUI) still exist, therefore
         * the clients details (coords and dimensions must re reacquired based on the game windows.
         */
        public static MyScreen reacquireGameClientLayout(MyScreen screenGameWindow, int panelScreenHeight, int panelScreenWidth)
        {
            for (int gameClientIdx = 0; gameClientIdx < screenGameWindow.getTotalNumberOfGameWindows(); gameClientIdx++)
            {
                // translate the old game client's dimensions and coordinates to the new screen
                // - HEIGHT & WIDTH
                int gameWindowHeight = getScreenHeight(screenGameWindow.height[gameClientIdx],
                    screenGameWindow.screenHeight, panelScreenHeight);
                int gameWindowWidth = getScreenWidth(screenGameWindow.width[gameClientIdx],
                    screenGameWindow.screenWidth, panelScreenWidth);
                screenGameWindow.setGameClientHeight(gameClientIdx, gameWindowHeight);
                screenGameWindow.setGameClientWidth(gameClientIdx, gameWindowWidth);
                // - X & Y COORDIATES
                // work out where the game window is, then translate that to the real world screen
                int gameClientXCoord = acquireRealWorldXCoordinate(screenGameWindow, gameClientIdx, panelScreenWidth);
                int gameClientYCoord = acquireRealWorldYCoordinate(screenGameWindow, gameClientIdx, panelScreenHeight);
                screenGameWindow.setGameClientXCoord(gameClientIdx, gameClientXCoord);
                screenGameWindow.setGameClientYCoord(gameClientIdx, gameClientYCoord);
            }
            return screenGameWindow;
        }

        /*
         * This method acquires the current height and width of a specified game client, based on its corresponding game window
         */
        public static MyScreen reacquireGameClientDimensions(MyScreen screenGameWindow, int gameClientIdx, int panelScreenHeight, int panelScreenWidth)
        {
            // translate the old game client's dimensions and coordinates to the new screen
            // - HEIGHT & WIDTH
            int gameWindowHeight = getScreenHeight(screenGameWindow.height[gameClientIdx],
                screenGameWindow.screenHeight, panelScreenHeight);
            int gameWindowWidth = getScreenWidth(screenGameWindow.width[gameClientIdx],
                screenGameWindow.screenWidth, panelScreenWidth);
            screenGameWindow.setGameClientHeight(gameClientIdx, gameWindowHeight);
            screenGameWindow.setGameClientWidth(gameClientIdx, gameWindowWidth);
            // - X & Y COORDIATES
            // work out where the game window is, then translate that to the real world screen
            int gameClientXCoord = acquireRealWorldXCoordinate(screenGameWindow, gameClientIdx, panelScreenWidth);
            int gameClientYCoord = acquireRealWorldYCoordinate(screenGameWindow, gameClientIdx, panelScreenHeight);
            screenGameWindow.setGameClientXCoord(gameClientIdx, gameClientXCoord);
            screenGameWindow.setGameClientYCoord(gameClientIdx, gameClientYCoord);

            return screenGameWindow;
        }

        /*
         * This method alters the actual dimensions and coordinates of a game client 
         * This should be called after the corresponding game wind (config screen in GUI) has been chaned in any way.
         */
        public static void redisplayGameClient(MyScreen screenGameWindow, int gameClientIdx)
        {
            Process gameClientProc = screenGameWindow.getProcess(gameClientIdx);

            if (gameClientProc.WaitForInputIdle(15000))
                SetWindowPos(gameClientProc.MainWindowHandle, HWND_TOPMOST, screenGameWindow.getGameClientXCoord(gameClientIdx),
                    screenGameWindow.getGameClientYCoord(gameClientIdx), screenGameWindow.getGameClientWidth(gameClientIdx),
                    screenGameWindow.getGameClientHeight(gameClientIdx), SWP_SHOWWINDOW);
        }

        /*
         * This method takes all of the game windows, and places them in order on the screen panel
         */
        public static void displayGameWindowsOnCurrentScreen(MyScreen screenGameWindows)
        {
            // iterate through the game wisdows, displaying them on the screen
            int numberOfGameWindows = screenGameWindows.getTotalNumberOfGameWindows();
            for (int gameWindowIdx = 0; gameWindowIdx < numberOfGameWindows; gameWindowIdx++)
            {
                Process gameProcess;
                // open the game client
                gameProcess = new Process();
       //         gameProcess.StartInfo.FileName = gamePath;
                // need to make sure the game window has been assigned an exe path
                if (screenGameWindows.getExePath(gameWindowIdx) != null)
                {

                    gameProcess.StartInfo.FileName = screenGameWindows.getExePath(gameWindowIdx);
                    MessageBox.Show("Game window: " + gameWindowIdx + " exe path: " + screenGameWindows.getExePath(gameWindowIdx));
                    gameProcess.Start();

                    // store this client's process in myScreens object
                    screenGameWindows.setProcess(gameProcess, gameWindowIdx);

                    // Now to change the location and size of the client window, if I can :-)
                    if (gameProcess.WaitForInputIdle(15000))
                        SetWindowPos(gameProcess.MainWindowHandle, HWND_TOPMOST, screenGameWindows.getGameClientXCoord(gameWindowIdx),
                            screenGameWindows.getGameClientYCoord(gameWindowIdx), screenGameWindows.getGameClientWidth(gameWindowIdx),
                            screenGameWindows.getGameClientHeight(gameWindowIdx), SWP_SHOWWINDOW);
                }
                else // no exe path defined for the current game window
                    MessageBox.Show("ERROR, no exe path assign for game window # " + gameWindowIdx);

            }
        }


        public static void killAllClientsOnCurrentScreen(MyScreen currentGameWindowScreen)
        {
            // iterate through all game clients and kill their process
            for (int gameClientIdx = 0; gameClientIdx < currentGameWindowScreen.getTotalNumberOfGameWindows(); gameClientIdx++)
            {
                // guard against it trying to kill a window that does not exist
                try
                {
                    // close the client
                    currentGameWindowScreen.getProcess(gameClientIdx).CloseMainWindow();
                    currentGameWindowScreen.getProcess(gameClientIdx).WaitForExit();

                    // reset client details
                    currentGameWindowScreen.setGameClientHeight(gameClientIdx, 0);
                    currentGameWindowScreen.setGameClientWidth(gameClientIdx, 0);
                    currentGameWindowScreen.setGameClientXCoord(gameClientIdx, 0);
                    currentGameWindowScreen.setGameClientYCoord(gameClientIdx, 0);
                }
                catch
                {
                    // this will stop it from trying to kill a window that does not exist
                }
            }
            currentGameWindowScreen.gameClientsExist = false; // required for when we wish to redesign the game client layout
        }


    }
}


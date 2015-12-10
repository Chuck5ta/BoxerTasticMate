using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing; // Color
using System.Diagnostics; // Processes

namespace BoxerTasticMate
{
    public class MyScreen
    {
        // Real world screen related
        public int screenID; // 0 to the total number of screens - 1  ... this is used in the assigning and swallping of real world screens
                                // assigned to the config screens
        public string screenName;
        public int screenHeight;
        public int screenWidth;
        public int screenXCoord;
        public int screenYCoord;

        public int[] screenHeightPercentage; // the percentage of the screen taken up on the Y axis (height)
        public int[] screenWidthPercentage; // the percentage of the screen taken up on the X axis (width)

        // Game window related

        private int totalNumberOfGameWindows;

        public int[] gameWindow;
        public int[] xCoord;
        public int[] yCoord;
        public int[] height;
        public int[] width;

        public Color[] colour;

        public int[] positionIndex;

        // Acccount related
        private string[] accountName;
        private string[] accountPassword;

        // Game client related
        public bool gameClientsExist; // used to show if there are game clients open on the screen
        private int[] clientXCoordinate;
        private int[] clientYCoordinate;
        private int[] clientHeight;
        private int[] clientWidth;
        private string[] exePath;
        // Real world game client
        private Process[] gameClientProcess;
        // this states which game client is the one the user is playing on, as we do not want to broadcast keys to that one
        private bool[] primaryGameClient;


        /*
         * Class contructor
         */
        public MyScreen()
        {
            // Real world screen
            screenName = "";

            screenHeight= 0;
            screenWidth = 0;
            screenXCoord = 0;
            screenYCoord = 0;

            screenWidthPercentage = new int[100];
            screenHeightPercentage = new int[100];

   //        gameClientXCoord = new int[100];
   //         gameClientYCoord = new int[100];

            // Game window and config panle screen related
            gameWindow = new int[100]; // max of 100 game windows on 1 screen
            xCoord = new int[100]; // max of 100 game windows on 1 screen
            yCoord = new int[100]; // max of 100 game windows on 1 screen
            height = new int[100]; // max of 100 game windows on 1 screen
            width = new int[100]; // max of 100 game windows on 1 screen
            colour = new Color[100];
            positionIndex = new int[100];
            gameClientProcess = new Process[100];
            // game account related
            accountName = new string[100];
            accountPassword = new string[100];
            // game client related
            gameClientsExist = false;
            clientXCoordinate = new int[100];
            clientYCoordinate = new int[100];
            clientHeight = new int[100];
            clientWidth = new int[100];
            primaryGameClient = new bool[100];

            exePath = new string[100];

            initialise();
            totalNumberOfGameWindows = 0;

        }

        private void initialise()
        {
            for (int i = 0; i < 100; i++)
            {
                gameWindow[i] = 0;
                xCoord[i] = 0;
                yCoord[i] = 0;
                height[i] = 0;
                width[i] = 0;
                positionIndex[i] = -1;
                colour[i] = Color.Black;
            }
        }


        public void setTotalNumberOfGameWindows(int number)
        {
            totalNumberOfGameWindows = number;
        }
        public int getTotalNumberOfGameWindows()
        {
            return totalNumberOfGameWindows;
        }

        /*
         * This methods checks to see if a specified game window exists on a certian screen
         */
        public bool exists(int gameWindowNumber)
        {
            for (int i=0; i < totalNumberOfGameWindows; i++)
            {
                if (gameWindow[i] == gameWindowNumber)
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * This is used when the game windows are first created
         * The method adds a game window to the Positon Index, so that we can tell which game windows are
         * obscured or partially obscured
         */
        public void addToPositionIndex(int gameWindowIdx, int gameWindowNumber)
        {
            positionIndex[gameWindowIdx] = gameWindowNumber;
        }

        /*
         * This method places a game window to the top of the queue, and shifts all others down one
         */
        public void shiftGameWindowToTop(int gameWindowNumber)
        {
            bool swapping = false;
            for (int gameWindowIdx = totalNumberOfGameWindows; gameWindowIdx >= 0; gameWindowIdx--)
            {
                // only start swapping once we locate the current location of the game window in the Position Index
                if (positionIndex[gameWindowIdx] == gameWindowNumber)
                {
                    swapping = true;
                }
                if (swapping && gameWindowIdx !=0)
                    positionIndex[gameWindowIdx] = positionIndex[gameWindowIdx-1];
            }
            positionIndex[0] = gameWindowNumber;
        }

        /*
         * This method works out from a list of game windows, which is the top most of them (in front of)
         */
        public int getTopMostGameWindow(int[] currentLocationGameWindowList, int numberOfGameWindowsInCurrentLocation)
        {
            // locate the first of the game windows
            for (int gameWindowIdx = 0; gameWindowIdx < totalNumberOfGameWindows; gameWindowIdx++)
            {
                for (int gameWindowInCurrentLocation = 0; gameWindowInCurrentLocation < numberOfGameWindowsInCurrentLocation; gameWindowInCurrentLocation++)
                    if (currentLocationGameWindowList[gameWindowInCurrentLocation] == positionIndex[gameWindowIdx])
                        return currentLocationGameWindowList[gameWindowInCurrentLocation];
            }

            return currentLocationGameWindowList[0];
        }

        /*
         * This method works the screen index of the game window is
         * The game window passed to this method with be the overall index of the game window as part of all the game windows
         * on all the screens.
         * Here we wish to know where it exist index wise on a specific screen
         */
        public int getGameWindowScreenIdx(int gameWindowNo)
        {
            for (int gameWindowIndex = 0; gameWindowIndex < totalNumberOfGameWindows;gameWindowIndex++)
            {
                if (gameWindowNo == gameWindow[gameWindowIndex])
                    return gameWindowIndex;
            }
            return 0;
        }

        /*
         * Set/Get the game's process
         * - index is the game client's position in relation to all game clients and all screens
         */
        public void setProcess(Process proc, int index)
        {
            gameClientProcess[index] = proc;
        }
        public Process getProcess(int index)
        {
            return gameClientProcess[index];
        }

        /*
         * Set/Get the game's account name
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setAccountName(string accName, int gameClientIdx)
        {
            accountName[gameClientIdx] = accName;
        }
        public string getAccountName(int gameClientIdx)
        {
            return accountName[gameClientIdx];
        }

        /*
         * Set/Get the game's account password
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setAccountPassword(string password, int gameClientIdx)
        {
            accountPassword[gameClientIdx] = password;
        }
        public string getAccountPassword(int gameClientIdx)
        {
            return accountPassword[gameClientIdx];
        }

        /*
         *  Set/Get the game client's X coordinate
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setGameClientXCoord(int gameClientIdx, int xCoord)
        {
            clientXCoordinate[gameClientIdx] = xCoord;
        }
        public int getGameClientXCoord(int gameClientIdx)
        {
            return clientXCoordinate[gameClientIdx];
        }

        /*
         * Set/Get the game client's Y coordinate
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setGameClientYCoord(int gameClientIdx, int yCoord)
        {
            clientYCoordinate[gameClientIdx] = yCoord;
        }
        public int getGameClientYCoord(int gameClientIdx)
        {
            return clientYCoordinate[gameClientIdx];
        }

        /*
         * Set/Get the game client height
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setGameClientHeight(int gameClientIdx, int height)
        {
            clientHeight[gameClientIdx] = height;
        }
        public int getGameClientHeight(int gameClientIdx)
        {
            return clientHeight[gameClientIdx];
        }

        /*
         * Set/Get the game client width
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setGameClientWidth(int gameClientIdx, int width)
        {
            clientWidth[gameClientIdx] = width;
        }
        public int getGameClientWidth(int gameClientIdx)
        {
            return clientWidth[gameClientIdx];
        }

        /*
         * These methods set abd get the primary game client state of the game client.
         * If the game client is the one the use plays on, then we do not want to broadcast key to it.
         */
        public void setPrimaryGameClientState(int gameClientIdx, bool primaryGameClientState)
        {
            primaryGameClient[gameClientIdx] = primaryGameClientState;
        }
        public bool isThePrimaryGameClient(int gameClientIdx)
        {
            return primaryGameClient[gameClientIdx];
        }

        /*
         * Set/Get the game client's executable's path
         * - gameClientIdx is the game client's position in relation to all game clients and all screens
         */
        public void setExePath(string path, int gameClientIdx)
        {
            exePath[gameClientIdx] = path;
        }
        public string getExePath(int gameClientIdx)
        {
            return exePath[gameClientIdx];
        }

        /*
         * This is called after a game window in the config area is moved
         */
        public void upDateMyScreensObject(Panel gameWindow, int index)
        {
            xCoord[index] = gameWindow.Location.X;
            yCoord[index] = gameWindow.Location.Y;
            height[index] = gameWindow.Height;
            width[index] = gameWindow.Width;
        }


    }
}

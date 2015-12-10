/*
 * This class deals with the config screens and the layout of their game windows.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms; // Panel control

using System.Drawing; // Color

namespace BoxerTasticMate
{
    class ConfigurationScreen
    {

        public static int getStartingGameWindowIndex(int currentScreen, List<MyScreen> myScreens)
        {
            int startingGameWindowIndex = 0;
            for (int scrnIdx = 0; scrnIdx < currentScreen; scrnIdx++)
            {
                startingGameWindowIndex += myScreens[scrnIdx].getTotalNumberOfGameWindows();
            }
            return startingGameWindowIndex;
        }

        /*
         * This method is used to help in the generation of the panels used to represent the game windows within the comfig window
         */
        public static List<Panel> generateGameWindowObjects(List<Panel> gameWindows, MyScreen screenGameWindow, int startingGameWindowNumber,
            int rows, int columns)
        {
            int gameWindowNumber = startingGameWindowNumber;

            // 0 to number of game windows on a specific screen
            int gameWindowIdx = 0;

            // define the spec of the game windows (coord, dimensions, etc.)
            // --- include the colour being written to the object!!!!
            for (int column = 0; column < rows; column++)
            {
                for (int row = 0; row < columns; row++)
                {
                    // Create game windws
                    gameWindows[gameWindowNumber].Height = screenGameWindow.height[gameWindowIdx];
                    gameWindows[gameWindowNumber].Width = screenGameWindow.width[gameWindowIdx];
                    gameWindows[gameWindowNumber].Location = new Point(screenGameWindow.xCoord[gameWindowIdx],
                         screenGameWindow.yCoord[gameWindowIdx]);
                    gameWindows[gameWindowNumber].BackColor = screenGameWindow.colour[gameWindowIdx];
                    gameWindowNumber++;
                    gameWindowIdx++;
                }
            }
            // List<Panel>
            return gameWindows;
        }

        /*
         * This method works out all the coordinates, dimensions, etc. of the game windows on the current screen
         */
        public static MyScreen generateUserDefinedGameWindowLayout(MyScreen screenGameWindow, int startingGameWindowNumber,
            int rows, int columns, int panelScreenHeight, int panelScreenWidth)
        {
            int gameWindowNumber = startingGameWindowNumber;
            int gameWindowHeight = 0;
            int gameWindowWidth = 0;
            int gameClientHeight = 0;
            int gameClientWidth = 0;
            int totalNumberOfGamesAcross = 0;
            int totalNumberOfGamesDown = 0;

            // 0 to number of game windows on a specific screen
            int gameWindowIdx = 0;

            // this will deal with odd # of screens that can still be arranged in this type of order
            totalNumberOfGamesAcross = columns;
            totalNumberOfGamesDown = rows;
            gameWindowHeight = panelScreenHeight / totalNumberOfGamesDown;
            gameWindowWidth = panelScreenWidth / totalNumberOfGamesAcross;
            gameClientHeight = screenGameWindow.screenHeight / totalNumberOfGamesDown;
            gameClientWidth = screenGameWindow.screenWidth / totalNumberOfGamesAcross;

            // define the spec of the game windows (coord, dimensions, etc.)
            // --- include the colour being written to the object!!!!
            for (int column = 0; column < rows; column++)
            {
                for (int row = 0; row < columns; row++)
                {
                    screenGameWindow.height[gameWindowIdx] = gameWindowHeight;
                    screenGameWindow.width[gameWindowIdx] = gameWindowWidth;
                    screenGameWindow.xCoord[gameWindowIdx] = row * gameWindowWidth;
                    screenGameWindow.yCoord[gameWindowIdx] = column * gameWindowHeight;

                    // work out and store the real world game client details
                    screenGameWindow.setGameClientHeight(gameWindowIdx, gameClientHeight);
                    screenGameWindow.setGameClientWidth(gameWindowIdx, gameClientWidth);
                    screenGameWindow.setGameClientXCoord(gameWindowIdx, row * gameClientWidth + 
                        screenGameWindow.screenXCoord);
                    screenGameWindow.setGameClientYCoord(gameWindowIdx, column * gameClientHeight +
                        screenGameWindow.screenYCoord);

                    // stagger the colours of the game windows
                    if (column % 2 == 0)
                    {
                        if (row % 2 == 0)
                        {
                            screenGameWindow.colour[gameWindowIdx] = Color.Red;
                        }
                        else
                            screenGameWindow.colour[gameWindowIdx] = Color.Blue;
                    }
                    else
                    {
                        if (row % 2 == 0)
                        {
                            screenGameWindow.colour[gameWindowIdx] = Color.Green;
                        }
                        else
                            screenGameWindow.colour[gameWindowIdx] = Color.Purple;
                    }
                    gameWindowIdx++;
                }
            }

            if (screenGameWindow.gameClientsExist) // we need to change their layout
            {
                RealWorldScreenManipulation.alterGameClientLayout(screenGameWindow);
            }
            
            return screenGameWindow;
        } // END OF generateUserDefinedGameWindowLayout( ... )

        /*
         * 
         * ----------[]
         * |        |[]
         * |        |[]
         * ----------[]
         */
        public static MyScreen generate1BigManySmallRightSideGameWindowLayout(MyScreen screenGameWindow, int startingGameWindowNumber,
            int totalGameWindows, int panelScreenHeight, int panelScreenWidth)
        {
            // define dimension of the game windows to appear to the right hand side of the main game window
            int totalNumberOfGamesDown = totalGameWindows - 1; // one of the game windows will be the main (biggesr) one
            int gameWindowHeight = panelScreenHeight / totalNumberOfGamesDown;
            int gameWindowWidth = gameWindowHeight;
            int gameClientHeight = 0;
            int gameClientWidth = 0;
            gameClientHeight = screenGameWindow.screenHeight / totalNumberOfGamesDown;
            gameClientWidth = gameClientHeight;

            int gameWindowIdx = 0;

            for (int gameIdx = 1; gameIdx < totalGameWindows; gameIdx++)
            {
                screenGameWindow.height[gameIdx] = gameWindowHeight;
                screenGameWindow.width[gameIdx] = gameWindowWidth;
                screenGameWindow.xCoord[gameIdx] = panelScreenWidth - gameWindowWidth;
                screenGameWindow.yCoord[gameIdx] = (gameIdx - 1) * gameWindowHeight;

                // work out and store the real world game client details
                screenGameWindow.setGameClientHeight(gameIdx, gameClientHeight);
                screenGameWindow.setGameClientWidth(gameIdx, gameClientWidth);
                screenGameWindow.setGameClientXCoord(gameIdx, screenGameWindow.screenWidth - gameClientWidth +
                    screenGameWindow.screenXCoord);
                screenGameWindow.setGameClientYCoord(gameIdx, (gameIdx - 1) * gameClientHeight +
                    screenGameWindow.screenYCoord);

                // stagger the colours of the game windows
                if (gameIdx % 2 == 0)
                {
                    screenGameWindow.colour[gameIdx] = Color.Red;
                }
                else
                {
                    screenGameWindow.colour[gameIdx] = Color.Green;
                }
            }

            // now gen the main window
            screenGameWindow.height[gameWindowIdx] = panelScreenHeight;
            screenGameWindow.width[gameWindowIdx] = panelScreenWidth - gameWindowWidth;
            screenGameWindow.xCoord[gameWindowIdx] = 0;
            screenGameWindow.yCoord[gameWindowIdx] = 0;
            screenGameWindow.colour[gameWindowIdx] = Color.Blue;

            // work out and store the real world game client details
            screenGameWindow.setGameClientHeight(gameWindowIdx, screenGameWindow.screenHeight);
            screenGameWindow.setGameClientWidth(gameWindowIdx, screenGameWindow.screenWidth - gameClientWidth);
            screenGameWindow.setGameClientXCoord(gameWindowIdx, screenGameWindow.screenXCoord);
            screenGameWindow.setGameClientYCoord(gameWindowIdx, screenGameWindow.screenYCoord);

            if (screenGameWindow.gameClientsExist) // we need to change their layout
            {
                RealWorldScreenManipulation.alterGameClientLayout(screenGameWindow);
            }

            return screenGameWindow;
        }

        /*
         * 
         * []----------
         * []|        |
         * []|        |
         * []----------
         */
        public static MyScreen generate1BigManySmallLeftSideGameWindowLayout(MyScreen screenGameWindow, int startingGameWindowNumber,
            int totalGameWindows, int panelScreenHeight, int panelScreenWidth)
        {
            // define dimension of the game windows to appear to the right hand side of the main game window
            int totalNumberOfGamesDown = totalGameWindows - 1; // one of the game windows will be the main (biggesr) one
            int gameWindowHeight = panelScreenHeight / totalNumberOfGamesDown;
            int gameWindowWidth = gameWindowHeight;
            int gameClientHeight = 0;
            int gameClientWidth = 0;
            gameClientHeight = screenGameWindow.screenHeight / totalNumberOfGamesDown;
            gameClientWidth = gameClientHeight;

            int gameWindowIdx = 0;

            for (int gameIdx = 1; gameIdx < totalGameWindows; gameIdx++)
            {
                screenGameWindow.height[gameIdx] = gameWindowHeight;
                screenGameWindow.width[gameIdx] = gameWindowWidth;
                screenGameWindow.xCoord[gameIdx] = 0;
                screenGameWindow.yCoord[gameIdx] = (gameIdx - 1) * gameWindowHeight;
                // stagger the colours of the game windows

                // work out and store the real world game client details
                screenGameWindow.setGameClientHeight(gameIdx, gameClientHeight);
                screenGameWindow.setGameClientWidth(gameIdx, gameClientWidth);
                screenGameWindow.setGameClientXCoord(gameIdx, screenGameWindow.screenXCoord);
                screenGameWindow.setGameClientYCoord(gameIdx, (gameIdx - 1) * gameClientHeight +
                    screenGameWindow.screenYCoord);

                if (gameIdx % 2 == 0)
                {
                    screenGameWindow.colour[gameIdx] = Color.Red;
                }
                else
                {
                    screenGameWindow.colour[gameIdx] = Color.Green;
                }
            }

            // now gen the main window
            screenGameWindow.height[gameWindowIdx] = panelScreenHeight;
            screenGameWindow.width[gameWindowIdx] = panelScreenWidth - gameWindowWidth;
            screenGameWindow.xCoord[gameWindowIdx] = gameWindowWidth;
            screenGameWindow.yCoord[gameWindowIdx] = 0;
            screenGameWindow.colour[gameWindowIdx] = Color.Blue;

            // work out and store the real world game client details
            screenGameWindow.setGameClientHeight(gameWindowIdx, screenGameWindow.screenHeight);
            screenGameWindow.setGameClientWidth(gameWindowIdx, screenGameWindow.screenWidth - gameClientWidth);
            screenGameWindow.setGameClientXCoord(gameWindowIdx, gameClientWidth + screenGameWindow.screenXCoord);
            screenGameWindow.setGameClientYCoord(gameWindowIdx, screenGameWindow.screenYCoord);
            
            if (screenGameWindow.gameClientsExist) // we need to change their layout
            {
                RealWorldScreenManipulation.alterGameClientLayout(screenGameWindow);
            }

            return screenGameWindow;
        }








        /*
         * This method displays all game windows once the above relevant function defines their dimension, coords, and colours
         */
        public static List<Panel> displayGameWindowLayout(MyScreen screenGameWindow, List<Panel> gameWindows, int startingGameWindowNumber,
            int totalGameWindows, int panelScreenHeight, int panelScreenWidth)
        {
            int gameWindowNumber = startingGameWindowNumber;

            // 0 to number of game windows on a specific screen
            int gameWindowIdx = 0;

            for (int gameIdx = 0; gameIdx < totalGameWindows; gameIdx++)
            {
                // Create game windwws
                gameWindows[gameWindowNumber].Height = screenGameWindow.height[gameWindowIdx];
                gameWindows[gameWindowNumber].Width = screenGameWindow.width[gameWindowIdx];
                gameWindows[gameWindowNumber].Location = new Point(screenGameWindow.xCoord[gameWindowIdx],
                     screenGameWindow.yCoord[gameWindowIdx]);
                gameWindows[gameWindowNumber].BackColor = screenGameWindow.colour[gameWindowIdx];
                gameWindowNumber++;
                gameWindowIdx++;
            }

            return gameWindows;
        }

        /*
         * [][][][][][]
         * ----------[]
         * |        |[]
         * |        |[]
         * ----------[]
         */
        public static MyScreen generate1BigManySmallTopAndRightSideGameWindowLayout(MyScreen screenGameWindow, int startingGameWindowNumber,
            int totalGameWindows, int panelScreenHeight, int panelScreenWidth)
        {
            int gameWindow = startingGameWindowNumber;
            totalGameWindows = screenGameWindow.getTotalNumberOfGameWindows();
            // calculate total # of games across the top (1 less then going up on the right side)
            int totalNumberOfGamesAcross = Convert.ToInt32(totalGameWindows / 2);
            int totalNumberOfGamesDown = totalGameWindows - totalNumberOfGamesAcross;
            // calculate the height and width of each window
            int gameWindowHeight = panelScreenHeight / totalNumberOfGamesDown;
            int gameWindowWidth = panelScreenWidth / totalNumberOfGamesAcross;
            int gameClientHeight = 0;
            int gameClientWidth = 0;
            gameClientHeight = screenGameWindow.screenHeight / totalNumberOfGamesDown;
            gameClientWidth = screenGameWindow.screenWidth / totalNumberOfGamesAcross;

            // 0 to number of game windows on a specific screen
            int gameWindowIdx = 0;

            // Main Window - gameWindow 0
            // ==========================
            screenGameWindow.height[gameWindowIdx] = panelScreenHeight - gameWindowHeight;
            screenGameWindow.width[gameWindowIdx] = panelScreenWidth - gameWindowWidth;
            // coords of main widow
            int gameWindowYCoord = gameWindowHeight;
            screenGameWindow.xCoord[gameWindowIdx] = 0;
            screenGameWindow.yCoord[gameWindowIdx] = gameWindowHeight;
            screenGameWindow.colour[gameWindowIdx] = Color.Blue;

            // work out and store the real world game client details
            screenGameWindow.setGameClientHeight(gameWindowIdx, screenGameWindow.screenHeight - gameClientHeight);
            screenGameWindow.setGameClientWidth(gameWindowIdx, screenGameWindow.screenWidth - gameClientWidth);
            screenGameWindow.setGameClientXCoord(gameWindowIdx, screenGameWindow.screenXCoord);
            screenGameWindow.setGameClientYCoord(gameWindowIdx, gameClientHeight + screenGameWindow.screenYCoord);
            
            int gameXCoord = 0;
            int gameYCoord = 0;
            // All the other windows
            for (int i = 0; i < totalNumberOfGamesAcross; i++)
            {
                gameWindow++; // index of game window related to all game windows on all screens
                gameWindowIdx++; // index of game window within current screen
                // work out the X coord (Y will always be 0)
                gameXCoord = i * gameWindowWidth;
                screenGameWindow.height[gameWindowIdx] = gameWindowHeight;
                screenGameWindow.width[gameWindowIdx] = gameWindowWidth;
                screenGameWindow.xCoord[gameWindowIdx] = gameXCoord;
                screenGameWindow.yCoord[gameWindowIdx] = gameYCoord;

                // work out and store the real world game client details
                screenGameWindow.setGameClientHeight(gameWindowIdx, gameClientHeight);
                screenGameWindow.setGameClientWidth(gameWindowIdx, gameClientWidth);
                screenGameWindow.setGameClientXCoord(gameWindowIdx, i * gameClientWidth + screenGameWindow.screenXCoord);
                screenGameWindow.setGameClientYCoord(gameWindowIdx, screenGameWindow.screenYCoord);

                if (i % 2 == 0)
                {
                    screenGameWindow.colour[gameWindowIdx] = Color.Green;
                }
                else
                    screenGameWindow.colour[gameWindowIdx] = Color.Purple;
            }
            gameXCoord = screenGameWindow.width[0];
            for (int i = 1; i < totalNumberOfGamesDown; i++)
            {
                gameWindow++; // index of game window related to all game windows on all screens
                gameWindowIdx++; // index of game window within current screen
                gameYCoord = i * gameWindowHeight;
                screenGameWindow.height[gameWindowIdx] = gameWindowHeight;
                screenGameWindow.width[gameWindowIdx] = gameWindowWidth;
                screenGameWindow.xCoord[gameWindowIdx] = gameXCoord;
                screenGameWindow.yCoord[gameWindowIdx] = gameYCoord;

                // work out and store the real world game client details
                screenGameWindow.setGameClientHeight(gameWindowIdx, gameClientHeight);
                screenGameWindow.setGameClientWidth(gameWindowIdx, gameClientWidth);
                screenGameWindow.setGameClientXCoord(gameWindowIdx, screenGameWindow.screenWidth - gameClientWidth + screenGameWindow.screenXCoord);
                screenGameWindow.setGameClientYCoord(gameWindowIdx, i * gameClientHeight + screenGameWindow.screenYCoord);

                if (i % 2 == 0)
                {
                    screenGameWindow.colour[gameWindowIdx] = Color.Red;
                }
                else
                    screenGameWindow.colour[gameWindowIdx] = Color.Yellow;

            }

            if (screenGameWindow.gameClientsExist) // we need to change their layout
            {
                RealWorldScreenManipulation.alterGameClientLayout(screenGameWindow);
            }

            return screenGameWindow;
        }

        /*
         * [][][][][][]
         * []----------
         * []|        |
         * []|        |
         * []----------
         */
        public static MyScreen generate1BigManySmallTopAndLeftSideGameWindowLayout(MyScreen screenGameWindow, int startingGameWindowNumber,
            int totalGameWindows, int panelScreenHeight, int panelScreenWidth)
        {
            int gameWindow = startingGameWindowNumber;
            totalGameWindows = screenGameWindow.getTotalNumberOfGameWindows();
            // calculate total # of games across the top (1 less then going up on the right side)
            int totalNumberOfGamesAcross = Convert.ToInt32(totalGameWindows / 2);
            int totalNumberOfGamesDown = totalGameWindows - totalNumberOfGamesAcross;
            // calculate the height and width of each window
            int gameWindowHeight = panelScreenHeight / totalNumberOfGamesDown;
            int gameWindowWidth = panelScreenWidth / totalNumberOfGamesAcross;
            int gameClientHeight = 0;
            int gameClientWidth = 0;
            gameClientHeight = screenGameWindow.screenHeight / totalNumberOfGamesDown;
            gameClientWidth = screenGameWindow.screenWidth / totalNumberOfGamesAcross;

            // 0 to number of game windows on a specific screen
            int gameWindowIdx = 0;

            // Main Window - gameWindow 0
            // ==========================
            screenGameWindow.height[gameWindowIdx] = panelScreenHeight - gameWindowHeight;
            screenGameWindow.width[gameWindowIdx] = panelScreenWidth - gameWindowWidth;
            // coords of main widow
            int gameWindowYCoord = gameWindowHeight;
            screenGameWindow.xCoord[gameWindowIdx] = gameWindowWidth;
            screenGameWindow.yCoord[gameWindowIdx] = gameWindowHeight;
            screenGameWindow.colour[gameWindowIdx] = Color.Blue;

            // work out and store the real world game client details
            screenGameWindow.setGameClientHeight(gameWindowIdx, screenGameWindow.screenHeight - gameClientHeight);
            screenGameWindow.setGameClientWidth(gameWindowIdx, screenGameWindow.screenWidth - gameClientWidth);
            screenGameWindow.setGameClientXCoord(gameWindowIdx, gameClientWidth + screenGameWindow.screenXCoord);
            screenGameWindow.setGameClientYCoord(gameWindowIdx, gameClientHeight + screenGameWindow.screenYCoord);

            int gameXCoord = 0;
            int gameYCoord = 0;
            // All the other windows
            for (int i = 0; i < totalNumberOfGamesAcross; i++)
            {
                gameWindow++; // index of game window related to all game windows on all screens
                gameWindowIdx++; // index of game window within current screen
                // work out the X coord (Y will always be 0)
                gameXCoord = i * gameWindowWidth;
                screenGameWindow.height[gameWindowIdx] = gameWindowHeight;
                screenGameWindow.width[gameWindowIdx] = gameWindowWidth;
                screenGameWindow.xCoord[gameWindowIdx] = gameXCoord;
                screenGameWindow.yCoord[gameWindowIdx] = gameYCoord;

                // work out and store the real world game client details
                screenGameWindow.setGameClientHeight(gameWindowIdx, gameClientHeight);
                screenGameWindow.setGameClientWidth(gameWindowIdx, gameClientWidth);
                screenGameWindow.setGameClientXCoord(gameWindowIdx, i * gameClientWidth + screenGameWindow.screenXCoord);
                screenGameWindow.setGameClientYCoord(gameWindowIdx, screenGameWindow.screenYCoord);

                if (i % 2 == 0)
                {
                    screenGameWindow.colour[gameWindowIdx] = Color.Green;
                }
                else
                    screenGameWindow.colour[gameWindowIdx] = Color.Purple;
            }
            gameXCoord = 0;
            for (int i = 1; i < totalNumberOfGamesDown; i++)
            {
                gameWindow++; // index of game window related to all game windows on all screens
                gameWindowIdx++; // index of game window within current screen
                gameYCoord = i * gameWindowHeight;
                screenGameWindow.height[gameWindowIdx] = gameWindowHeight;
                screenGameWindow.width[gameWindowIdx] = gameWindowWidth;
                screenGameWindow.xCoord[gameWindowIdx] = gameXCoord;
                screenGameWindow.yCoord[gameWindowIdx] = gameYCoord;

                // work out and store the real world game client details
                screenGameWindow.setGameClientHeight(gameWindowIdx, gameClientHeight);
                screenGameWindow.setGameClientWidth(gameWindowIdx, gameClientWidth);
                screenGameWindow.setGameClientXCoord(gameWindowIdx, screenGameWindow.screenXCoord);
                screenGameWindow.setGameClientYCoord(gameWindowIdx, i * gameClientHeight + screenGameWindow.screenYCoord);

                if (i % 2 == 0)
                {
                    screenGameWindow.colour[gameWindowIdx] = Color.Red;
                }
                else
                    screenGameWindow.colour[gameWindowIdx] = Color.Yellow;

            }

            if (screenGameWindow.gameClientsExist) // we need to change their layout
            {
                RealWorldScreenManipulation.alterGameClientLayout(screenGameWindow);
            }

            return screenGameWindow;
        }

        /*
         * This method takes all of the game windows, and places them in order on the screen panel
         */
        public static MyScreen generateGameWindowsOnCurrentScreen(MyScreen screenGameWindow, int startingGameWindowNumber,
                        int panelScreenHeight, int panelScreenWidth)
        {
            MyScreen scrnGameWindow = null;
            if (screenGameWindow.getTotalNumberOfGameWindows() == 1)
            {
        //        MessageBox.Show("Displaying 1 screen");
                scrnGameWindow = generateUserDefinedGameWindowLayout(screenGameWindow, startingGameWindowNumber, 1, 1, 
                    panelScreenHeight, panelScreenWidth);

            }
            else if (screenGameWindow.getTotalNumberOfGameWindows() == 2)
            {
     //           MessageBox.Show("Displaying 2 screen");
                scrnGameWindow = generateUserDefinedGameWindowLayout(screenGameWindow, startingGameWindowNumber, 1, 2,
                    panelScreenHeight, panelScreenWidth);
            }
            else if (screenGameWindow.getTotalNumberOfGameWindows() == 3)
            {
     //           MessageBox.Show("Displaying 3 screen");
                scrnGameWindow = generate1BigManySmallLeftSideGameWindowLayout(screenGameWindow, startingGameWindowNumber, 3,
                    panelScreenHeight, panelScreenWidth);
            }
            else if (screenGameWindow.getTotalNumberOfGameWindows() == 4)
            {
      //          MessageBox.Show("Displaying 4 screen");
                scrnGameWindow = generateUserDefinedGameWindowLayout(screenGameWindow, startingGameWindowNumber, 2, 2,
                    panelScreenHeight, panelScreenWidth);
            }
            else
            {
                scrnGameWindow = generate1BigManySmallTopAndRightSideGameWindowLayout(screenGameWindow, startingGameWindowNumber, 
                    screenGameWindow.getTotalNumberOfGameWindows(), panelScreenHeight, panelScreenWidth);
            }

            return scrnGameWindow;

        }
        
        /*
         * This method takes all of the game windows, and places them in order on the screen panel
         */
        public static List<Panel> placeGameWindowsOnCurrentScreen(MyScreen screenGameWindow, List<Panel> gameWindows, int startingGameWindowNumber,
                        int panelScreenHeight, int panelScreenWidth, int totalGameWindows)
        {
            if (screenGameWindow.getTotalNumberOfGameWindows() == 1)
            {
                gameWindows = generateGameWindowObjects(gameWindows, screenGameWindow, startingGameWindowNumber, 1, 1);
            }
            else if (screenGameWindow.getTotalNumberOfGameWindows() == 2)
            {
                gameWindows = generateGameWindowObjects(gameWindows, screenGameWindow, startingGameWindowNumber, 1, 2);
            }
            else if (screenGameWindow.getTotalNumberOfGameWindows() == 3)
            {
                gameWindows = displayGameWindowLayout(screenGameWindow, gameWindows, startingGameWindowNumber,
                    3, panelScreenHeight, panelScreenWidth);
            }
            else if (screenGameWindow.getTotalNumberOfGameWindows() == 4)
            {
                gameWindows = generateGameWindowObjects(gameWindows, screenGameWindow, startingGameWindowNumber, 2, 2);
            }
            else
            {
                gameWindows = displayGameWindowLayout(screenGameWindow, gameWindows, startingGameWindowNumber,
                    totalGameWindows, panelScreenHeight, panelScreenWidth);
            }
            return gameWindows;
        }


        /*
         * This method takes all of the game windows, and places them in order on the screen panel
         */
        private void placeGameWindowsOnCurrentScreen(int startingGameWindowNumber)
        {
            /*

            if (myScreens[currentScreen].getTotalNumberOfGameWindows() == 1)
            {
                
                // fill screen with game window
                gameWindows[startingGameWindowNumber].Height = panelScreen.Height;
                gameWindows[startingGameWindowNumber].Width = panelScreen.Width;

                gameWindows[startingGameWindowNumber].BackColor = Color.Red;
                myScreens[currentScreen].colour[0] = Color.Red;
                gameWindows[startingGameWindowNumber].Location = new Point(0, 0);

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber]);

                // store in myScreens object
                myScreens[currentScreen].height[0] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[0] = gameWindows[startingGameWindowNumber].Width;
                myScreens[currentScreen].xCoord[0] = 0;
                myScreens[currentScreen].yCoord[0] = 0;
            }
            else if (myScreens[currentScreen].getTotalNumberOfGameWindows() == 2)
            { // display the screen side by side
                // game window 1
                for (int i = startingGameWindowNumber; i < (startingGameWindowNumber + 2); i++)
                {
                    gameWindows[i].Height = panelScreen.Height;
                    gameWindows[i].Width = panelScreen.Width / 2;
                }
                myScreens[currentScreen].height[0] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[0] = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber].BackColor = Color.Pink;
                myScreens[currentScreen].colour[0] = Color.Pink;
                gameWindows[startingGameWindowNumber].Location = new Point(0, 0);
                myScreens[currentScreen].xCoord[0] = 0;
                myScreens[currentScreen].yCoord[0] = 0;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber]);

                // game window 2
                gameWindows[startingGameWindowNumber + 1].BackColor = Color.Blue;
                myScreens[currentScreen].height[1] = gameWindows[startingGameWindowNumber + 1].Height;
                myScreens[currentScreen].width[1] = gameWindows[startingGameWindowNumber + 1].Width;
                myScreens[currentScreen].colour[1] = Color.Blue;
                int X = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber + 1].Location = new Point(X, 0);
                myScreens[currentScreen].xCoord[1] = X;
                myScreens[currentScreen].yCoord[1] = 0;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber + 1]);
            }
            else if (myScreens[currentScreen].getTotalNumberOfGameWindows() == 3)
            { // display main game window on 2/3 of screen, and the other 2 one on top of other on the right-hand side
                // game window 1
                gameWindows[startingGameWindowNumber].Height = panelScreen.Height;
                gameWindows[startingGameWindowNumber].Width = Convert.ToInt32(panelScreen.Width * 0.66);
                gameWindows[startingGameWindowNumber].Location = new Point(0, 0);
                gameWindows[startingGameWindowNumber].BackColor = Color.Blue;
                myScreens[currentScreen].colour[0] = Color.Blue;
                myScreens[currentScreen].height[0] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[0] = gameWindows[startingGameWindowNumber].Width;
                myScreens[currentScreen].xCoord[0] = 0;
                myScreens[currentScreen].yCoord[0] = 0;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber]);
                // game window 2
                gameWindows[startingGameWindowNumber + 1].Height = panelScreen.Height / 2;
                gameWindows[startingGameWindowNumber + 1].Width = panelScreen.Width - gameWindows[startingGameWindowNumber].Width;
                int X = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber + 1].Location = new Point(X, 0);
                gameWindows[startingGameWindowNumber + 1].BackColor = Color.Red;
                myScreens[currentScreen].colour[1] = Color.Red;
                myScreens[currentScreen].height[1] = gameWindows[startingGameWindowNumber + 1].Height;
                myScreens[currentScreen].width[1] = gameWindows[startingGameWindowNumber + 1].Width;
                myScreens[currentScreen].xCoord[1] = X;
                myScreens[currentScreen].yCoord[1] = 0;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber + 1]);
                // game window 3
                gameWindows[startingGameWindowNumber + 2].Height = panelScreen.Height / 2;
                gameWindows[startingGameWindowNumber + 2].Width = panelScreen.Width - gameWindows[startingGameWindowNumber].Width;
                X = gameWindows[startingGameWindowNumber].Width;
                int Y = gameWindows[startingGameWindowNumber + 1].Height;
                gameWindows[startingGameWindowNumber + 2].Location = new Point(X, Y);
                gameWindows[startingGameWindowNumber + 2].BackColor = Color.Green;
                myScreens[currentScreen].colour[2] = Color.Green;
                myScreens[currentScreen].height[2] = gameWindows[startingGameWindowNumber + 2].Height;
                myScreens[currentScreen].width[2] = gameWindows[startingGameWindowNumber + 2].Width;
                myScreens[currentScreen].xCoord[2] = X;
                myScreens[currentScreen].yCoord[2] = Y;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber + 2]);
            }
            else if (myScreens[currentScreen].getTotalNumberOfGameWindows() == 4)
            { // display main game windows as 2 up and 2 down
                // game window 1
                for (int i = startingGameWindowNumber; i < startingGameWindowNumber + 4; i++)
                {
                    gameWindows[i].Height = panelScreen.Height / 2;
                    gameWindows[i].Width = panelScreen.Width / 2;
                }
                myScreens[currentScreen].height[0] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[0] = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber].BackColor = Color.Blue;
                myScreens[currentScreen].colour[0] = Color.Blue;
                gameWindows[startingGameWindowNumber].Location = new Point(0, 0);
                myScreens[currentScreen].xCoord[0] = 0;
                myScreens[currentScreen].yCoord[0] = 0;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber]);
                // game window 2
                myScreens[currentScreen].height[1] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[1] = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber + 1].BackColor = Color.Red;
                myScreens[currentScreen].colour[1] = Color.Red;
                int X = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber + 1].Location = new Point(X, 0);
                myScreens[currentScreen].xCoord[1] = X;
                myScreens[currentScreen].yCoord[1] = 0;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber + 1]);
                // game window 3
                myScreens[currentScreen].height[2] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[2] = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber + 2].BackColor = Color.Orange;
                myScreens[currentScreen].colour[2] = Color.Orange;
                int Y = gameWindows[startingGameWindowNumber + 2].Height;
                gameWindows[startingGameWindowNumber + 2].Location = new Point(0, Y);
                myScreens[currentScreen].xCoord[2] = 0;
                myScreens[currentScreen].yCoord[2] = Y;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber + 2]);
                // game window 4
                myScreens[currentScreen].height[3] = gameWindows[startingGameWindowNumber].Height;
                myScreens[currentScreen].width[3] = gameWindows[startingGameWindowNumber].Width;
                gameWindows[startingGameWindowNumber + 3].BackColor = Color.Green;
                myScreens[currentScreen].colour[3] = Color.Green;
                X = gameWindows[startingGameWindowNumber + 3].Width;
                Y = gameWindows[startingGameWindowNumber + 3].Height;
                gameWindows[startingGameWindowNumber + 3].Location = new Point(X, Y);
                myScreens[currentScreen].xCoord[3] = X;
                myScreens[currentScreen].yCoord[3] = Y;

                this.panelScreen.Controls.Add(gameWindows[startingGameWindowNumber + 3]);
            }
            else if (Math.Sqrt(myScreens[currentScreen].getTotalNumberOfGameWindows()) % 1 == 0) // there must be no remainder
            {
                MessageBox.Show("SQR ROOT !!!!");
                generateScreenLayoutSQRroot(startingGameWindowNumber);
            }
            else
            {
                generateScreenLayout1Row1Column(startingGameWindowNumber);
            }
             */
        }



    }

}

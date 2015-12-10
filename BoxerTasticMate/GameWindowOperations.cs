/*
 * This class contains methods that deal with the game window/client within the MyScreen object.
 * 
 * e.g. associating the game client exe path of all game windows
 * 
 * Author: Chuck E
 * Date: 24th of August 2014
 * 
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BoxerTasticMate
{
    class GameWindowOperations
    {

        /*
         * This method associates a single exe path to all game windows
         */
        public static List<MyScreen> assignSameExeToAllGameWindows(string exePath, int totalScreens, List<MyScreen> myScreens)
        {
            int totalGameWindows = 0; // this will hold the total game windows on the current screen
            // iterate through all the screens
            for (int screenIdx = 0; screenIdx < totalScreens; screenIdx++)
            {
                totalGameWindows = myScreens[screenIdx].getTotalNumberOfGameWindows();
                // iterate through all game windows on the screen                
                for (int gameWindowIdx = 0; gameWindowIdx < totalScreens; gameWindowIdx++)
                {
                    // set the game window's exe path
                    myScreens[screenIdx].setExePath(exePath, gameWindowIdx);
                }

            }

            return myScreens;
        }

    }
}

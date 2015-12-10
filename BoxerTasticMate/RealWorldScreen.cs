/*
 * This class deals represents the screens in the real world.
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
    class RealWorldScreen
    {
        public string screenName;
        public int screenHeight;
        public int screenWidth;
        public int screenTopLeftXCoord;
        public int screenTopLeftYCoord;

        public int currentlyAssignedToConfigScreen; // the config screen that this real world screen is assigned to

        /*
         * Class contructor
         */
        public RealWorldScreen()
        {
            screenName = "";
            screenHeight = 0;
            screenWidth = 0;
            screenTopLeftXCoord = 0;
            screenTopLeftYCoord = 0;
        }


    }
}

/*
 * This is a list of all the keys and their key value
 * e.g. OEMCOMMA ,
 *         D1    1
 *          F    f
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxerTasticMate
{
    class KeyList
    {
        Dictionary<string, string> listOfKey;

        // class constructor
        public KeyList()
        {
            listOfKey = new Dictionary<string, string>();
        }


        // populate the list
        public void populateTheKeyList()
        {
            // 0 to 9
            listOfKey.Add("D0", "0");
            listOfKey.Add("D1", "1");
            listOfKey.Add("D2", "2");
            listOfKey.Add("D3", "3");
            listOfKey.Add("D4", "4");
            listOfKey.Add("D5", "5");
            listOfKey.Add("D6", "6");
            listOfKey.Add("D7", "7");
            listOfKey.Add("D8", "8");
            listOfKey.Add("D9", "9");
            // a to z
            listOfKey.Add("A", "a");
            listOfKey.Add("B", "b");
            listOfKey.Add("C", "c");
            listOfKey.Add("D", "d");
            listOfKey.Add("E", "e");
            listOfKey.Add("F", "f");
            listOfKey.Add("G", "g");
            listOfKey.Add("H", "h");
            listOfKey.Add("I", "i");
            listOfKey.Add("J", "j");
            listOfKey.Add("K", "k");
            listOfKey.Add("L", "l");
            listOfKey.Add("M", "m");
            listOfKey.Add("N", "n");
            listOfKey.Add("O", "o");
            listOfKey.Add("P", "p");
            listOfKey.Add("Q", "q");
            listOfKey.Add("R", "r");
            listOfKey.Add("S", "s");
            listOfKey.Add("T", "t");
            listOfKey.Add("U", "u");
            listOfKey.Add("V", "v");
            listOfKey.Add("W", "w");
            listOfKey.Add("X", "x");
            listOfKey.Add("Y", "y");
            listOfKey.Add("Z", "z");
            // symbols - , . ; + etc.
            // , . / ; ' # [ ] - = `
            listOfKey.Add("Oemcomma", ",");
            listOfKey.Add("OemPeriod", ".");
            listOfKey.Add("OemQuestion", "/");
            listOfKey.Add("Oem5", "\\");
            listOfKey.Add("Oem1", ";");
            listOfKey.Add("Oemtilde", "'");
            listOfKey.Add("Oem7", "#");
            listOfKey.Add("OemOpenBrackets", "[");
            listOfKey.Add("Oem6", "]");
            listOfKey.Add("OemMinus", "-");
            listOfKey.Add("Oemplus", "=");
            listOfKey.Add("Oem8", "`");
            // NUMPAD 0 to 9
            listOfKey.Add("Insert", "0");
            listOfKey.Add("End", "1");
            listOfKey.Add("Down", "2");
            listOfKey.Add("Next", "3");
            listOfKey.Add("Left", "4");
            listOfKey.Add("Clear", "5");
            listOfKey.Add("Right", "6");
            listOfKey.Add("Home", "7");
            listOfKey.Add("Up", "8");
            listOfKey.Add("PageUp", "9");
            // Arrow keys
            listOfKey.Add("Left", "Left Arrow");
            listOfKey.Add("Up", "Up Arrow");
            listOfKey.Add("Right", "Right Arrow");
            listOfKey.Add("Down", "Down Arrow");
            // Function keys
            listOfKey.Add("F1", "F1");
            listOfKey.Add("F2", "F2");
            listOfKey.Add("F3", "F3");
            listOfKey.Add("F4", "F4");
            listOfKey.Add("F5", "F5");
            listOfKey.Add("F6", "F6");
            listOfKey.Add("F7", "F7");
            listOfKey.Add("F8", "F8");
            listOfKey.Add("F9", "F9");
            listOfKey.Add("F10", "F10");
            listOfKey.Add("F11", "F11");
            listOfKey.Add("F12", "F12");
            // Others
            listOfKey.Add("NUMLOCK", "NUMLOCK");





        }


    }
}

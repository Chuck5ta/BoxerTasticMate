/*
 * This contains a list of all the keys that can be modified by modfier keys: CTRL, SHIFT, ALT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxerTasticMate
{
    class ModifiableKeys
    {
        Dictionary<string, string> modifiableKeyList;


        // class constructor
        public ModifiableKeys()
        {
            modifiableKeyList = new Dictionary<string, string>();
            populateTheModifiableKeyList();
        }

        private void populateTheModifiableKeyList()
        {
            // 0 to 9
            modifiableKeyList.Add("D0", "0");
            modifiableKeyList.Add("D1", "1");
            modifiableKeyList.Add("D2", "2");
            modifiableKeyList.Add("D3", "3");
            modifiableKeyList.Add("D4", "4");
            modifiableKeyList.Add("D5", "5");
            modifiableKeyList.Add("D6", "6");
            modifiableKeyList.Add("D7", "7");
            modifiableKeyList.Add("D8", "8");
            modifiableKeyList.Add("D9", "9");
            // a to z
            modifiableKeyList.Add("A", "a");
            modifiableKeyList.Add("B", "b");
            modifiableKeyList.Add("C", "c");
            modifiableKeyList.Add("D", "d");
            modifiableKeyList.Add("E", "e");
            modifiableKeyList.Add("F", "f");
            modifiableKeyList.Add("G", "g");
            modifiableKeyList.Add("H", "h");
            modifiableKeyList.Add("I", "i");
            modifiableKeyList.Add("J", "j");
            modifiableKeyList.Add("K", "k");
            modifiableKeyList.Add("L", "l");
            modifiableKeyList.Add("M", "m");
            modifiableKeyList.Add("N", "n");
            modifiableKeyList.Add("O", "o");
            modifiableKeyList.Add("P", "p");
            modifiableKeyList.Add("Q", "q");
            modifiableKeyList.Add("R", "r");
            modifiableKeyList.Add("S", "s");
            modifiableKeyList.Add("T", "t");
            modifiableKeyList.Add("U", "u");
            modifiableKeyList.Add("V", "v");
            modifiableKeyList.Add("W", "w");
            modifiableKeyList.Add("X", "x");
            modifiableKeyList.Add("Y", "y");
            modifiableKeyList.Add("Z", "z");
            // symbols - , . ; + etc.
            // , . / ; ' # [ ] - = `
            modifiableKeyList.Add("Oemcomma", ",");
            modifiableKeyList.Add("OemPeriod", ".");
            modifiableKeyList.Add("OemQuestion", "/");
            modifiableKeyList.Add("Oem5", "\\");
            modifiableKeyList.Add("Oem1", ";");
            modifiableKeyList.Add("Oemtilde", "'");
            modifiableKeyList.Add("Oem7", "#");
            modifiableKeyList.Add("OemOpenBrackets", "[");
            modifiableKeyList.Add("Oem6", "]");
            modifiableKeyList.Add("OemMinus", "-");
            modifiableKeyList.Add("Oemplus", "=");
            modifiableKeyList.Add("Oem8", "`");
            // Function keys
            modifiableKeyList.Add("F1", "F1");
            modifiableKeyList.Add("F2", "F2");
            modifiableKeyList.Add("F3", "F3");
            modifiableKeyList.Add("F4", "F4");
            modifiableKeyList.Add("F5", "F5");
            modifiableKeyList.Add("F6", "F6");
            modifiableKeyList.Add("F7", "F7");
            modifiableKeyList.Add("F8", "F8");
            modifiableKeyList.Add("F9", "F9");
            modifiableKeyList.Add("F10", "F10");
            modifiableKeyList.Add("F11", "F11");
            modifiableKeyList.Add("F12", "F12");
        }

        /*
         * This method checks to see if a key is one of the ones the multiboxer allows to be modified
         * e.g. CTRL+A - allowed
         *      CTRL+NumLock - not allowed
         */
        public bool isModifiableKey(string keyToSearchFor)
        {
            if (modifiableKeyList.ContainsValue(keyToSearchFor))
            {
                return true;
            }
            return false;
        }

    }
}

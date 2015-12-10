/*
 * This contains a list of keys, whose output needs to be converted in order to be able to process it.
 * e.g. 0 to 9, a to f
 *     D0 to D9, A to F
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxerTasticMate
{
    class KeysToConvert
    {
        Dictionary<string, string> keysToConvertList;


        // class constructor
        public KeysToConvert()
        {
            keysToConvertList = new Dictionary<string, string>();
            populateTheModifiableKeyList();
        }

        private void populateTheModifiableKeyList()
        {
            // 0 to 9
            keysToConvertList.Add("D0", "0");
            keysToConvertList.Add("D1", "1");
            keysToConvertList.Add("D2", "2");
            keysToConvertList.Add("D3", "3");
            keysToConvertList.Add("D4", "4");
            keysToConvertList.Add("D5", "5");
            keysToConvertList.Add("D6", "6");
            keysToConvertList.Add("D7", "7");
            keysToConvertList.Add("D8", "8");
            keysToConvertList.Add("D9", "9");
            // a to z
            keysToConvertList.Add("A", "a");
            keysToConvertList.Add("B", "b");
            keysToConvertList.Add("C", "c");
            keysToConvertList.Add("D", "d");
            keysToConvertList.Add("E", "e");
            keysToConvertList.Add("F", "f");
            keysToConvertList.Add("G", "g");
            keysToConvertList.Add("H", "h");
            keysToConvertList.Add("I", "i");
            keysToConvertList.Add("J", "j");
            keysToConvertList.Add("K", "k");
            keysToConvertList.Add("L", "l");
            keysToConvertList.Add("M", "m");
            keysToConvertList.Add("N", "n");
            keysToConvertList.Add("O", "o");
            keysToConvertList.Add("P", "p");
            keysToConvertList.Add("Q", "q");
            keysToConvertList.Add("R", "r");
            keysToConvertList.Add("S", "s");
            keysToConvertList.Add("T", "t");
            keysToConvertList.Add("U", "u");
            keysToConvertList.Add("V", "v");
            keysToConvertList.Add("W", "w");
            keysToConvertList.Add("X", "x");
            keysToConvertList.Add("Y", "y");
            keysToConvertList.Add("Z", "z");
            // symbols - , . ; + etc.
            // , . / ; ' # [ ] - = `
            keysToConvertList.Add("Oemcomma", ",");
            keysToConvertList.Add("OemPeriod", ".");
            keysToConvertList.Add("OemQuestion", "/");
            keysToConvertList.Add("Oem5", "\\");
            keysToConvertList.Add("Oem1", ";");
            keysToConvertList.Add("Oemtilde", "'");
            keysToConvertList.Add("Oem7", "#");
            keysToConvertList.Add("OemOpenBrackets", "[");
            keysToConvertList.Add("Oem6", "]");
            keysToConvertList.Add("OemMinus", "-");
            keysToConvertList.Add("Oemplus", "=");
            keysToConvertList.Add("Oem8", "`");
        }

        public string convertKey(string keyToSearchFor)
        {
            if (keysToConvertList.ContainsKey(keyToSearchFor))
            {
                string value = "";
                return value = keysToConvertList[keyToSearchFor];
            }

            return ""; // key is not one that needs converting
        }

    }
}

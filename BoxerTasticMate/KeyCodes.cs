using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxerTasticMate
{
    class KeyCodes
    {
        Dictionary<string, string> keyCodeList;


        // class constructor
        public KeyCodes()
        {
            keyCodeList = new Dictionary<string, string>();
            populateTheKeyCodesList();
        }


        // populate the list
        private void populateTheKeyCodesList()
        {
            // 0 to 9
            keyCodeList.Add("30", "0");
            keyCodeList.Add("31", "1");
            keyCodeList.Add("32", "2");
            keyCodeList.Add("33", "3");
            keyCodeList.Add("34", "4");
            keyCodeList.Add("35", "5");
            keyCodeList.Add("36", "6");
            keyCodeList.Add("37", "7");
            keyCodeList.Add("38", "8");
            keyCodeList.Add("39", "9");
            // a to z
            keyCodeList.Add("41", "a");
            keyCodeList.Add("42", "b");
            keyCodeList.Add("43", "c");
            keyCodeList.Add("44", "d");
            keyCodeList.Add("45", "e");
            keyCodeList.Add("46", "f");
            keyCodeList.Add("47", "g");
            keyCodeList.Add("48", "h");
            keyCodeList.Add("49", "i");
            keyCodeList.Add("4A", "j");
            keyCodeList.Add("4B", "k");
            keyCodeList.Add("4C", "l");
            keyCodeList.Add("4D", "m");
            keyCodeList.Add("4E", "n");
            keyCodeList.Add("4F", "o");
            keyCodeList.Add("50", "p");
            keyCodeList.Add("51", "q");
            keyCodeList.Add("52", "r");
            keyCodeList.Add("53", "s");
            keyCodeList.Add("54", "t");
            keyCodeList.Add("55", "u");
            keyCodeList.Add("56", "v");
            keyCodeList.Add("57", "w");
            keyCodeList.Add("58", "x");
            keyCodeList.Add("59", "y");
            keyCodeList.Add("5A", "z");
            // symbols - , . ; + etc.
            // , . / ; ' # [ ] - = `
            keyCodeList.Add("BC", ",");
            keyCodeList.Add("BE", ".");
            keyCodeList.Add("BF", "/"); // ???????
            keyCodeList.Add("DC", "\\");
            keyCodeList.Add("BA", ";");
            keyCodeList.Add("C0", "'"); // ???????
            keyCodeList.Add("DE", "#");
            keyCodeList.Add("DB", "[");  // ??????
            keyCodeList.Add("DD", "]");
            keyCodeList.Add("BD", "-"); // minus
            keyCodeList.Add("BB", "="); // underscore
            keyCodeList.Add("DF", "`");
            // NUMPAD 0 to 9
            keyCodeList.Add("60", "Numpad0");
            keyCodeList.Add("61", "Numpad1");
            keyCodeList.Add("62", "Numpad2");
            keyCodeList.Add("63", "Numpad3");
            keyCodeList.Add("64", "Numpad4");
            keyCodeList.Add("65", "Numpad5");
            keyCodeList.Add("66", "Numpad6");
            keyCodeList.Add("67", "Numpad7");
            keyCodeList.Add("68", "Numpad8");
            keyCodeList.Add("69", "Numpad9");
            // Arrow keys
            keyCodeList.Add("25", "Left");
            keyCodeList.Add("26", "Up");
            keyCodeList.Add("27", "Right");
            keyCodeList.Add("28", "Down");
            // Function keys
            keyCodeList.Add("70", "F1");
            keyCodeList.Add("71", "F2");
            keyCodeList.Add("72", "F3");
            keyCodeList.Add("73", "F4");
            keyCodeList.Add("74", "F5");
            keyCodeList.Add("75", "F6");
            keyCodeList.Add("76", "F7");
            keyCodeList.Add("77", "F8");
            keyCodeList.Add("78", "F9");
            keyCodeList.Add("79", "F10");
            keyCodeList.Add("7A", "F11");
            keyCodeList.Add("7B", "F12");
            // Others
            keyCodeList.Add("90", "NUMLOCK");
            keyCodeList.Add("20", "SPACEBAR");

        }


        /*
         * returns the key value, based on its KeyCode
         * If returns "", then they key sent is not supported by the multiboxer. IT should do nothing with it.
         */
        public string getKeyValue(string keyToSearchFor)
        {
            if (keyCodeList.ContainsKey(keyToSearchFor))
            {
                string value = "";
                return value = keyCodeList[keyToSearchFor];
            }

            return ""; // key is not one that needs converting
        }
        
    }
}

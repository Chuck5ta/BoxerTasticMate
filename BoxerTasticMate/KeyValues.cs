using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoxerTasticMate
{
    class KeyValues
    {
        Dictionary<string, string> keyValuesList;


        // class constructor
        public KeyValues()
        {
            keyValuesList = new Dictionary<string, string>();
            populateTheKeyValuesList();
        }


        // populate the list
        private void populateTheKeyValuesList()
        {
            // 0 to 9
            keyValuesList.Add("0", "30");
            keyValuesList.Add("1", "31");
            keyValuesList.Add("2", "32");
            keyValuesList.Add("3", "33");
            keyValuesList.Add("4", "34");
            keyValuesList.Add("5", "35");
            keyValuesList.Add("6", "36");
            keyValuesList.Add("7", "37");
            keyValuesList.Add("8", "38");
            keyValuesList.Add("9", "39");
            // a to z
            keyValuesList.Add("a", "41");
            keyValuesList.Add("b", "42");
            keyValuesList.Add("c", "43");
            keyValuesList.Add("d", "44");
            keyValuesList.Add("e", "45");
            keyValuesList.Add("f", "46");
            keyValuesList.Add("g", "47");
            keyValuesList.Add("h", "48");
            keyValuesList.Add("i", "49");
            keyValuesList.Add("j", "4A");
            keyValuesList.Add("k", "4B");
            keyValuesList.Add("l", "4C");
            keyValuesList.Add("m", "4D");
            keyValuesList.Add("n", "4E");
            keyValuesList.Add("o", "4F");
            keyValuesList.Add("p", "50");
            keyValuesList.Add("q", "51");
            keyValuesList.Add("r", "52");
            keyValuesList.Add("s", "53");
            keyValuesList.Add("t", "54");
            keyValuesList.Add("u", "55");
            keyValuesList.Add("v", "56");
            keyValuesList.Add("w", "57");
            keyValuesList.Add("x", "58");
            keyValuesList.Add("y", "59");
            keyValuesList.Add("z", "5A");
            // symbols - . ; + etc.
            // . / ; ' # [ ] - = `
            keyValuesList.Add(",", "BC");
            keyValuesList.Add(".", "BE");
            keyValuesList.Add("/", "BF"); // ???????
            keyValuesList.Add("\\", "DC");
            keyValuesList.Add(";", "BA");
            keyValuesList.Add("'", "C0"); // ???????
            keyValuesList.Add("#", "DE");
            keyValuesList.Add("[", "DB");  // ??????
            keyValuesList.Add("]", "DD");
            keyValuesList.Add("-", "BD");
            keyValuesList.Add("=", "BB");
            keyValuesList.Add("`", "DF");
            // NUMPAD 0 to 9
            keyValuesList.Add("Numpad0", "60");
            keyValuesList.Add("Numpad1", "61");
            keyValuesList.Add("Numpad2", "62");
            keyValuesList.Add("Numpad3", "63");
            keyValuesList.Add("Numpad4", "64");
            keyValuesList.Add("Numpad5", "65");
            keyValuesList.Add("Numpad6", "66");
            keyValuesList.Add("Numpad7", "67");
            keyValuesList.Add("Numpad8", "68");
            keyValuesList.Add("Numpad9", "69");
            // Arrow keys
            keyValuesList.Add("Left", "25");
            keyValuesList.Add("Up", "26");
            keyValuesList.Add("Right", "27");
            keyValuesList.Add("Down", "28");
            // Function keys
            keyValuesList.Add("F1", "70");
            keyValuesList.Add("F2", "71");
            keyValuesList.Add("F3", "72");
            keyValuesList.Add("F4", "73");
            keyValuesList.Add("F5", "74");
            keyValuesList.Add("F6", "75");
            keyValuesList.Add("F7", "76");
            keyValuesList.Add("F8", "77");
            keyValuesList.Add("F9", "78");
            keyValuesList.Add("F10", "79");
            keyValuesList.Add("F11", "7A");
            keyValuesList.Add("F12", "7B");
            // Others
            keyValuesList.Add("NUMLOCK", "90");
            keyValuesList.Add("SPACEBAR", "20");

        }


        /*
         * returns the key code, based on its value (which is held in the key field of the Dictionary data structure)
         * If returns "", then they key sent is not supported by the multiboxer. IT should do nothing with it.
         */
        public string getKeyCode(string keyValueToSearchFor)
        {
            if (keyValuesList.ContainsKey(keyValueToSearchFor))
            {
                string code = "";
                return code = keyValuesList[keyValueToSearchFor];
            }

            return ""; // key is not one that needs converting
        }
    }
}

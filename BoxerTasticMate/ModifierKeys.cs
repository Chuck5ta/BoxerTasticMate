using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BoxerTasticMate
{
    class ModifierKeys
    {
        Dictionary<string, string> modifierkeyList;


        // class constructor
        public ModifierKeys()
        {
            modifierkeyList = new Dictionary<string, string>();
            populateTheKeyValuesList();
        }

        // populate the list
        private void populateTheKeyValuesList()
        {
            modifierkeyList.Add("CONTROL", "UP");
            modifierkeyList.Add("SHIFT", "UP");
            modifierkeyList.Add("ALT", "UP");
        }

        // set key state
        public void setModifierKeyState(string modifierKey, string keyState)
        {
            if (modifierkeyList.ContainsKey(modifierKey))
            {
                modifierkeyList[modifierKey] = keyState;
            }
        }

        // get key state
        public string getModifierKeyState(string modifierKey)
        {
            if (modifierkeyList.ContainsKey(modifierKey))
            {
                return modifierkeyList[modifierKey];
            }
            MessageBox.Show("Modifier key not present!!!");
            return ""; // ERROR, key does not exist (CONTROL, SHIFT, ALT not sent to this method!)
        }


    }
}

using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Threading;


namespace BoxerTasticMate
{
    class InterceptKeys
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104; // for identifying the ALT key

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static BoxerTasticMate.Form1 copyOfmainForm;

        private static bool boxerTasticPaused = false;



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd); // for when we need to bring the game window forward


        public InterceptKeys(BoxerTasticMate.Form1 mainForm)
        {
            _hookID = SetHook(_proc);
            copyOfmainForm = mainForm;
        }

        ~InterceptKeys()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)) // WM_SYSKEYDOWN for the ALT key
            {
                int vkCode = Marshal.ReadInt32(lParam);

     //           MessageBox.Show("Key: " + vkCode);




                // CTRL+ALT+P = Pause the multiboxer. No more key broadcasting
                if (Control.ModifierKeys == (Keys.Control | Keys.Alt) && Keys.P == (Keys)vkCode)
                {
                    MessageBox.Show("PAUSED!");
                    boxerTasticPaused = !boxerTasticPaused;
                }


   //             MessageBox.Show("PAUSED state: " + boxerTasticPaused);
                // only action the keys when the system is not paused
                if (boxerTasticPaused == false)
                {
                    // check for multiple modifier keys

                    // TRIPLE
                    // ------
                    // CTRL SHIFT ALT
                    // CTRL ALT SHIFT
                    // SHIFT CTRL ALT
                    // SHIFT ALT CTRL
                    // ALT CTRL SHIFT
                    // ALT SHIFT CTRL

                    // DOUBLE
                    // ------
                    // CTRL SHIFT
                    // CTRL ALT
                    // SHIFT CTRL
                    // SHIFT ALT
                    // ALT CTRL
                    // ALT SHIFT

                    // SINGLE
                    // ------
                    // CTRL
                    // SHIFT
                    // ALT

                    // target key broadcast
                    // not a modifier key

                    // test for modifier keys + modifier key + target key (CTRL+ALT+key)
                    if (Control.ModifierKeys == (Keys.Control | Keys.Alt) && isTargetKey(vkCode))
                    {
                        copyOfmainForm.sendModifierKeysAndTargetKey(vkCode, "CONTROL", "ALT");
                    }
                    // test for modifier keys Keys.A
                    else if (Control.ModifierKeys == Keys.Control && isTargetKey(vkCode))
                    {
                        copyOfmainForm.sendModifierKeyAndTargetKey(vkCode, "CONTROL");
                    }
                    else if (Control.ModifierKeys == Keys.Shift && isTargetKey(vkCode))
                    {
                        copyOfmainForm.sendModifierKeyAndTargetKey(vkCode, "SHIFT");
                    }
                    else if (Control.ModifierKeys == Keys.Alt && isTargetKey(vkCode))
                    {
                        copyOfmainForm.sendModifierKeyAndTargetKey(vkCode, "ALT");
                    }
                    else // single key sent - no modifiers
                    {
                        copyOfmainForm.sendTargetKey(vkCode);
                    }
                }

            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /*
         * This method is used to see what key has been pressed along with a modifier ley (CTRL, SHIFT, ALT)
         */
        private static bool isTargetKey(int vkCode)
        {
            if ( ((Keys)vkCode >= Keys.A && (Keys)vkCode <= Keys.Z) // letters A to Z
                || ((Keys)vkCode >= Keys.D0 && (Keys)vkCode <= Keys.D9) // number 0 to 9
                || ((Keys)vkCode >= Keys.Oem1 && (Keys)vkCode <= Keys.Oem2) // ; to /
                || ((Keys)vkCode >= Keys.Oem3) // tilda
                || ((Keys)vkCode >= Keys.Oem4 && (Keys)vkCode <= Keys.Oem8) // [ to ` (top left key, below ESC)
                )
                return true;

            return false;
        }


        /*
         * This is called by the HookCallback method above, when a key is to be processed.
         * Using the method has reulted in the amount of code being reduced by 2/3
         */
        private static void broadcastTheKey(int vkCode)
        {
            copyOfmainForm.sendKey(vkCode);
        }

    }
}
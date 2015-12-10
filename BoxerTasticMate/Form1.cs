using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using System.IO; // for file handling

using System.Threading; // required in order to implement a pause (Sleep)
using System.Runtime.InteropServices; // used for mouse detection and WINAPI functions
using System.Diagnostics; // required for access to the list of processes

using System.Timers;

using System.Text.RegularExpressions;


namespace BoxerTasticMate
{
    public partial class Form1 : Form
    {
        Form popUpForm; // used in the testing to see if the user has the required real world screen

        public int totalNumberOfScreens = 0;
        private string[] screenName;
        private Rectangle[] screenBounds;
        private int currentScreen;

        public List<MyScreen> myScreens;
        private List<RealWorldScreen> realWorldScreens;

//        private bool userDefinedLayout; // used to signal if the game window layout is user defined

        private bool draggingObject;

        private List<Panel> gameWindows;

        private int totalNumberOfGameWindows;
        private int currentGameWindow;
        private int currentSelectedGameWindow; // active when left clicked on. Used in the setting up of account information

        // these ID the current client that is the primary client (the one the player is on)
        private int primaryGameWindowScreenIdx;
        private int primaryGameWindowIdx;


        // Account related variables
        bool validAccountName;
        bool validPassword;
        bool validPathToExe;
        bool validConfigFileName;


        // For displaying on the real world screen

        private string gamePath;
//        private int gameWindowHeight, gameWindowWidth;
//        private int gameWindowXCoord, gameWindowYCoord;

        private bool isValidRow;
        private bool isValidColumn;

        public const int SWP_SHOWWINDOW = 0x0040;

        public static IntPtr HWND_TOPMOST = (IntPtr)(-1);

        [DllImport("user32.dll")]
        private extern static bool MoveWindow(IntPtr hWnd, int X, int Y, int cx, int cy, bool uFlags);

        [DllImport("user32.dll")]
        private extern static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);


        //API Constants
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int VK_CONTROL = 0x11;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_CHAR = 0x102;
        public const int MK_LBUTTON = 0x01;

        public const int KEYEVENTF_EXTENDEDKEY = 0x0001; // Key down flag
        public const int KEYEVENTF_KEYUP = 0x0002; // Key up flag
        public const int VK_LCONTROL = 0xA2; // Left Control key code
        public const int VK_RCONTROL = 0xA3; // Right Control key code
        public const int VK_LSHIFT = 0xA0; // LEFT SHIFT key code
        public const int VK_RSHIFT = 0xA1; // Right SHIFT key code
        public const int VK_MENU = 0x12; // Right SHIFT key code

        // Spacebar and ESC
        public const int VK_BACK = 0x08; // Backspace key
        public const int VK_TAB = 0x09;
        public const int VK_RETURN = 0x0d;
        public const int VK_SHIFT = 0x10; //Shift key
        public const int VK_PAUSE = 0x13; // Pause/Break key
        public const int VK_CAPITAL = 0x14; // CAPS LOCK key
        public const int VK_ESCAPE = 0x1B; //ESC key
        public const int VK_SPACE = 0x20; //Spacebar
        public const int VK_PRIOR = 0x21; // PAGE UP key
        public const int VK_NEXT = 0x22; // PAGE DOWN key
        public const int VK_END = 0x23; // END key
        public const int VK_HOME = 0x24; // HOME key
        public const int VK_LEFT = 0x25; // LEFT ARROW key
        public const int VK_UP = 0x26; // UP ARROW key
        public const int VK_RIGHT = 0x27; // RIGHT ARROW key
        public const int VK_DOWN = 0x28; // DOWN ARROW key
        public const int VK_PRINT = 0x2A; // PRINT SCREEN key
        public const int VK_INSERT = 0x2D; // INSERT key
        public const int VK_DELETE = 0x2E; // INSERT key
        public const int VK_HELP = 0x2F; // INSERT key
        public const int VK_NUMLOCK = 0x90; // NUMLOCK key
        public const int VK_SCROLL = 0x91; // SCROLL LOCK key
        // Numpad
        public const int VK_NUMPAD0 = 0x60; // Numpad 0 key
        public const int VK_NUMPAD1 = 0x61; // Numpad 1 key
        public const int VK_NUMPAD2 = 0x62; // Numpad 2 key
        public const int VK_NUMPAD3 = 0x63; // Numpad 3 key
        public const int VK_NUMPAD4 = 0x64; // Numpad 4 key
        public const int VK_NUMPAD5 = 0x65; // Numpad 5 key
        public const int VK_NUMPAD6 = 0x66; // Numpad 6 key
        public const int VK_NUMPAD7 = 0x67; // Numpad 7 key
        public const int VK_NUMPAD8 = 0x68; // Numpad 8 key
        public const int VK_NUMPAD9 = 0x69; // Numpad 9 key
        // Symbols - + . / * etc
        public const int VK_MULTIPLY = 0x6A; // Multiply (*) key
        public const int VK_ADD = 0x6B; // Multiply (*) key
        public const int VK_SEPARATOR = 0x6C; // Multiply (*) key
        public const int VK_SUBTRACT = 0x6D; // Multiply (*) key
        public const int VK_DECIMAL = 0x6E; // Multiply (*) key
        public const int VK_DIVIDE = 0x6F; // Multiply (*) key

        // Keys 0 to 9
        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;

        // Keys A to Z
        public const int VK_A = 0x41; //A Control key code
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;
        public const int VK_D = 0x44;
        public const int VK_E = 0x45;
        public const int VK_F = 0x46;
        public const int VK_G = 0x47;
        public const int VK_H = 0x48;
        public const int VK_I = 0x49;
        public const int VK_J = 0x4A;
        public const int VK_K = 0x4B;
        public const int VK_L = 0x4C;
        public const int VK_M = 0x4D;
        public const int VK_N = 0x4E;
        public const int VK_O = 0x4F;
        public const int VK_P = 0x50;
        public const int VK_Q = 0x51;
        public const int VK_R = 0x52;
        public const int VK_S = 0x53;
        public const int VK_T = 0x54;
        public const int VK_U = 0x55;
        public const int VK_V = 0x56;
        public const int VK_W = 0x57;
        public const int VK_X = 0x58;
        public const int VK_Y = 0x59;
        public const int VK_Z = 0x5A;
        // Keys F1 to F12
        public const int VK_F1 = 0x70;
        public const int VK_F2 = 0x71;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;
        public const int VK_F9 = 0x78;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x80;
        public const int VK_F12 = 0x81;


        // Window resizing
        public const int GWL_STYLE = -16;
        public const int WS_OVERLAPPED = 0x00000000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_DISABLED = 0x08000000;
        public const int WS_CLIPSIBLINGS = 0x04000000;
        public const int WS_CLIPCHILDREN = 0x02000000;
        public const int WS_MAXIMIZE = 0x01000000;
        public const int WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const int WS_BORDER = 0x00800000;
        public const int WS_DLGFRAME = 0x00400000;
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;
        public const int WS_SYSMENU = 0x00080000;
        public const int WS_THICKFRAME = 0x00040000;
        public const int WS_GROUP = 0x00020000;
        public const int WS_TABSTOP = 0x00010000;

        public const int WS_MINIMIZEBOX = 0x00020000;
        public const int WS_MAXIMIZEBOX = 0x00010000;

        public const int WS_TILED = WS_OVERLAPPED;
        public const int WS_ICONIC = WS_MINIMIZE;
        public const int WS_SIZEBOX = WS_THICKFRAME;

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        // acquire the press/released state of a key as it is now (at the point of querying the key)
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetAsyncKeyState(int virtualKeyCode);

        // these show if the key is to be broadcast or not
        // Keys a to z
        public bool aKeyBroadcast, bKeyBroadcast, cKeyBroadcast, dKeyBroadcast, eKeyBroadcast, fKeyBroadcast, gKeyBroadcast, hKeyBroadcast, iKeyBroadcast, jKeyBroadcast, kKeyBroadcast;
        public bool lKeyBroadcast, mKeyBroadcast, nKeyBroadcast, oKeyBroadcast, pKeyBroadcast, qKeyBroadcast, rKeyBroadcast, sKeyBroadcast, tKeyBroadcast, uKeyBroadcast, vKeyBroadcast;
        public bool wKeyBroadcast, xKeyBroadcast, yKeyBroadcast, zKeyBroadcast;
        // Keys 0 to 9
        public bool oneKeyBroadcast, twoKeyBroadcast, threeKeyBroadcast, fourKeyBroadcast, fiveKeyBroadcast, sixKeyBroadcast, sevenKeyBroadcast, eightKeyBroadcast, nineKeyBroadcast, zeroKeyBroadcast; // keys 0 to 9
        // Keys F1 to F12
        public bool F1KeyBroadcast, F2KeyBroadcast, F3KeyBroadcast, F4KeyBroadcast, F5KeyBroadcast, F6KeyBroadcast;
        public bool F7KeyBroadcast, F8KeyBroadcast, F9KeyBroadcast, F10KeyBroadcast, F11KeyBroadcast, F12KeyBroadcast;
        // Numpad 0 to 9
        public bool numpad0KeyBroadcast, numpad1KeyBroadcast, numpad2KeyBroadcast, numpad3KeyBroadcast, numpad4KeyBroadcast, numpad5KeyBroadcast, numpad6KeyBroadcast;
        public bool numpad7KeyBroadcast, numpad8KeyBroadcast, numpad9KeyBroadcast;
        // Symbols - + . / * etc
        public bool multiplyKeyBroadcast, addKeyBroadcast, separatorKeyBroadcast, subtractKeyBroadcast, decimalKeyBroadcast, divideKeyBroadcast;
        // Other - (SPACEBAR, ESC, INSERT, Arrow Keys, NUMLOCK, etc)
        public bool backspaceKeyBroadcast, tabKeyBroadcast, returnKeyBroadcast, shiftKeyBroadcast, pauseKeyBroadcast, capsLockKeyBroadcast;
        public bool escapeKeyBroadcast, spacebarKeyBroadcast, pageUpKeyBroadcast, pageDownKeyBroadcast, endKeyBroadcast, homeKeyBroadcast;
        public bool leftArrowKeyBroadcast, upArrowKeyBroadcast, rightArrowKeyBroadcast, downArrowKeyBroadcast, printScreenKeyBroadcast, insertKeyBroadcast;
        public bool deleteKeyBroadcast, helpKeyBroadcast, numLockKeyBroadcast, scrollLockKeyBroadcast;


        
        
        // These show if the key is PRESSED or RELEASED
        // Keys a to z
        public static string aKeyState, bKeyState, cKeyState, dKeyState, eKeyState, fKeyState, gKeyState, hKeyState, iKeyState, jKeyState, kKeyState;
        public static string lKeyState, mKeyState, nKeyState, oKeyState, pKeyState, qKeyState, rKeyState, sKeyState, tKeyState, uKeyState, vKeyState;
        public static string wKeyState, xKeyState, yKeyState, zKeyState;
        // Keys 0 to 9
        public static string oneKeyState, twoKeyState, threeKeyState, fourKeyState, fiveKeyState, sixKeyState, sevenKeyState, eightKeyState, nineKeyState, zeroKeyState; // keys 0 to 9
        // Keys F1 to F12
        public static string F1KeyState, F2KeyState, F3KeyState, F4KeyState, F5KeyState, F6KeyState;
        public static string F7KeyState, F8KeyState, F9KeyState, F10KeyState, F11KeyState, F12KeyState;
        // Numpad 0 to 9
        public static string numpad0KeyState, numpad1KeyState, numpad2KeyState, numpad3KeyState, numpad4KeyState, numpad5KeyState, numpad6KeyState;
        public static string numpad7KeyState, numpad8KeyState, numpad9KeyState;
        // Symbols - + . / * etc
        public static string multiplyKeyState, addKeyState, separatorKeyState, subtractKeyState, decimalKeyState, divideKeyState;
        // Other - (SPACEBAR, ESC, INSERT, Arrow Keys, NUMLOCK, etc)
        public static string backspaceKeyState, tabKeyState, returnKeyState, shiftKeyState, pauseKeyState, capsLockKeyState;
        public static string escapeKeyState, spacebarKeyState, pageUpKeyState, pageDownKeyState, endKeyState, homeKeyState;
        public static string leftArrowKeyState, upArrowKeyState, rightArrowKeyState, downArrowKeyState, printScreenKeyState, insertKeyState;
        public static string deleteKeyState, helpKeyState, numLockKeyState, scrollLockKeyState;

        // used to allow a gap between key presses
        public static System.Timers.Timer keyBroadcastTimer; // From System.Timers


        // ===========================================
        //          Keyboard Capture
        // ===========================================

        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        //Declare the hook handle as an int.
//        static int hHook = 0;

        //Declare MouseHookProcedure as a HookProc type.
 //       HookProc KeyboardHookProcedure;


        InterceptKeys keyboardHooky;




        Dictionary<string, string> keyUpDownState;
        KeyCodes listOfKeyCodes;
        KeyValues listOfKeyValues;
        ModifierKeys listOfModifierKeyStates;

        bool bBroadcastKey;






        public Form1()
        {
            InitializeComponent();
        }

        private void createPopUpForm()
        {
            // display MessageBox to currently selected screen
            popUpForm = new Form();
            popUpForm.Height = 400;
            popUpForm.Width = 400;
            // place it in the centre of the screen

            // Create a button

            Button popUpButton = new Button();
            popUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            popUpButton.Location = new System.Drawing.Point(0, 0);
            popUpButton.Name = "btnPupUpExit";
            popUpButton.Size = new System.Drawing.Size(383, 383);
            //     popUpButton.TabIndex = 18;
            popUpButton.Text = "CLICK TO EXIT";
            popUpButton.UseVisualStyleBackColor = true;
            popUpButton.Click += new System.EventHandler(this.btnPoUpButtonExit_Click);
            popUpForm.Controls.Add(popUpButton);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // what screens do we have here ?
            acquireScreenDetails();

 //           userDefinedLayout = false; // set to not a user defined layout for the game windows

            gameWindows = new List<Panel>();

            // Resizing the game window related variables
            GameWindow.resizing = false;
            GameWindow.topLeftResizing = false;
            GameWindow.topRightResizing = false;
            GameWindow.bottomLeftResizing = false;
            GameWindow.bottomRightResizing = false;
            GameWindow.leftResizing = false;
            GameWindow.rightResizing = false;
            GameWindow.topResizing = false;
            GameWindow.bottomResizing = false;

            validAccountName = false;
            validPassword = false;
            validPathToExe = false;

            gamePath = txtExePath.Text;

            validConfigFileName = false;

            btnSaveConfigFile.Enabled = false; // disble this, until we have game windows in the config area

            // make sure the user cannot define their own layout unless game windows already exist
            txtRows.Enabled = false;
            txtColumns.Enabled = false;

            isValidRow = false;
            isValidColumn = false;

            // create a pop up form
            createPopUpForm();

            // Set the selection mode to multiple and extended.
            // - http://msdn.microsoft.com/en-us/library/system.windows.forms.listbox(v=vs.110).aspx
            lbListOfKeys.SelectionMode = SelectionMode.MultiExtended;
            lbKeysToBeBroadcast.SelectionMode = SelectionMode.MultiExtended;

            this.Width = 941; // set the width of the form so that it hides the Key Config area



            // key broadcasting - initialise flags (all in key UP/RELEASED state)
            // = keys 0 to 9
            zeroKeyState = "UP";
            oneKeyState = "UP";
            twoKeyState = "UP";
            threeKeyState = "UP";
            fourKeyState = "UP";
            fiveKeyState = "UP";
            sixKeyState = "UP";
            sevenKeyState = "UP";
            eightKeyState = "UP";
            nineKeyState = "UP";
            // - keys a to z
            aKeyState = "UP";
            dKeyState = "UP";
            sKeyState = "UP";
            wKeyState = "UP";
            // specebar and ESC
            spacebarKeyState = "UP";

            // key broadcasting - set keys to not to be broadcast by default
            // = keys 0 to 9

            // - keys a to z
            aKeyBroadcast = false;
            dKeyBroadcast = false;
            sKeyBroadcast = false;
            wKeyBroadcast = false;
            // specebar and ESC
            spacebarKeyBroadcast = false;


            // =========================================
            //           New Key Broadcasting
            // =========================================

            // contains a list of keycode and their name (value printed out, e.g. keyCode:65 value:a)
            listOfKeyCodes = new KeyCodes(); // used for locating the key value fast
            // contains a list of key values and their codes. Used for locating the key cade fast
            listOfKeyValues = new KeyValues();
            // this will hold the key state (UP/DOWN) of all keys to be broadcast (in the value field)
            keyUpDownState = new Dictionary<string, string>();
            // this will hold the key state (UP/DOWN) of all modifer keys (CONTRO, ALT, SHIFT)
            listOfModifierKeyStates = new ModifierKeys();

            bBroadcastKey = false; // disable key broadcasting

            // disable certain controls
            btnAddKey.Enabled = false; // nothing to add to the Keys to be Broadcast list, as yet
        }

        private void acquireScreenDetails()
        {
            totalNumberOfScreens = 0;
            txtSceensInfo.Text = "";
            currentScreen = 0;
            currentSelectedGameWindow = 0;
            draggingObject = false;
            // acquire and output the info on all connected screens
            foreach (var screen in Screen.AllScreens)
            {
                totalNumberOfScreens++; // increment the total # of screens
            }

            lblScreenNumber.Text = "Screen 1 of " + totalNumberOfScreens;

            // These 2 hold details of the real world screens/monitors
            screenName = new string[totalNumberOfScreens];
            screenBounds = new Rectangle[totalNumberOfScreens];
            // This will hold details of each screen layout on the screen panel within the GUI
            // This does not translate the details to that of the real world screens
            myScreens = new List<MyScreen>();

            realWorldScreens = new List<RealWorldScreen>();

            int index = 0;
            foreach (var screen in Screen.AllScreens)
            {

                // For each screen, add the screen properties to a list box.
                txtSceensInfo.Text += "Device Name: " + screen.DeviceName + "\r\n";
                txtSceensInfo.Text += "Bounds: " + screen.Bounds.ToString() + "\r\n";
                txtSceensInfo.Text += "Type: " + screen.GetType().ToString() + "\r\n";
                txtSceensInfo.Text += "Working Area: " + screen.WorkingArea.ToString() + "\r\n";
                txtSceensInfo.Text += "Primary Screen: " + screen.Primary.ToString() + "\r\n";

                screenName[index] = screen.DeviceName;
                screenBounds[index] = screen.Bounds;

                txtSceensInfo.Text += "Screen " + index;
                txtSceensInfo.Text += "=========\r\n";
                txtSceensInfo.Text += "X Coord " + screenBounds[index].X + "\r\n";
                txtSceensInfo.Text += "Y Coord " + screenBounds[index].Y + "\r\n";
                txtSceensInfo.Text += "Height " + screenBounds[index].Height + "\r\n";
                txtSceensInfo.Text += "Width " + screenBounds[index].Width + "\r\n";
                txtSceensInfo.Text += "\r\n\r\n";

                // these represent the actual screen dimensions
                // used to work out which config screen represent what real world screen
                RealWorldScreen realScreen = new RealWorldScreen();
                realWorldScreens.Add(realScreen);

                realWorldScreens[index].screenName = screen.DeviceName;
                realWorldScreens[index].screenHeight = screenBounds[index].Height;
                realWorldScreens[index].screenWidth = screenBounds[index].Width;
                realWorldScreens[index].screenTopLeftXCoord = screenBounds[index].X;
                realWorldScreens[index].screenTopLeftYCoord = screenBounds[index].Y;
                realWorldScreens[index].currentlyAssignedToConfigScreen = index;

                // generate a screen object for each real world screen
                // this holds data on the the real world screen, including the details of the game clients
                MyScreen screenie = new MyScreen();
                myScreens.Add(screenie);
                // save the screen details to the myScreens object
                myScreens[index].screenID = index; // relates the the combobox index of the list of screens and the RealWorldScreen list
                                                    // required in the assigning and swapping of real screens to config screens
                myScreens[index].screenName = screen.DeviceName;
                // acquire the rest of the screen's info
                myScreens[index].screenHeight = screenBounds[index].Height;
                myScreens[index].screenWidth = screenBounds[index].Width;
                myScreens[index].screenXCoord = screenBounds[index].X;
                myScreens[index].screenYCoord = screenBounds[index].Y;

                // add screen to ComboBox
                cmbListOfScreens.Items.Add(myScreens[index].screenName);

                realWorldScreens[index].currentlyAssignedToConfigScreen = index;
                myScreens[index].screenID = index;

                index++;
            }
            // make the one show for the first config screen
            cmbListOfScreens.Text = myScreens[0].screenName;

     //       cbmListOfScreens.Items.Add("AHHHHHHHH");
        } // END OF acquireScreenDetails()

        /*
         * This method only shows the game windows on the current screen. All other game windows are hidden
         */
        private void showHideGameWindows(int screenie, bool booleanState)
        {
	        int totalGameWindowsOnThisScreen = myScreens[screenie].getTotalNumberOfGameWindows();
	        for (int i=0; i < totalGameWindowsOnThisScreen; i++)
            {
                int gameWindowNumber = myScreens[screenie].gameWindow[i];
                gameWindows[gameWindowNumber].Visible = booleanState;
                gameWindows[gameWindowNumber].Enabled = booleanState;
                if (booleanState)
                {
                    gameWindows[gameWindowNumber].BringToFront(); // if we are showing the game window, then bring it forward
                }
                else
                    gameWindows[gameWindowNumber].SendToBack();
            }
        }


        // =========================================================================
        // ======================  Screen Layout Generation  =======================
        // =========================================================================


        /*
         * This method creates an object that represents each game window and assigns it to a screen
         */
        private void generateAllGameWindows()
        {

            // destroy old gameWindows object
            try
            {
                gameWindows = new List<Panel>();
                this.panelScreen.Controls.Clear();
            }
            catch
            {
            }
            // generate and store all of the game windows
            for (int i = 0; i < totalNumberOfGameWindows; i++)
            {
                Panel gameWindow = new Panel();

                gameWindow.BackColor = Color.Red;

                // add evets
                gameWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseDown);
                gameWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseUp);
                gameWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseMove);
                gameWindow.MouseEnter += new System.EventHandler(this.gameWindow_MouseEnter);
                // store the game window
                gameWindows.Add(gameWindow);
            }

            int gameWindowCount = 0;
            // cycle through the screens assigning the game windows
            for (int scrn = 0; scrn < totalNumberOfScreens; scrn++)
            {
                for (int gameWindow = 0; gameWindow < myScreens[scrn].getTotalNumberOfGameWindows(); gameWindow++)
                {
                    // assign game window to screen 
                    myScreens[scrn].gameWindow[gameWindow] = gameWindowCount;
                    gameWindowCount++;
                }
            }
        }


        /*
         * This method creates the game window object (panels) on each screen
         */
        private void updateGameWindowLayout()
        {
            gameWindows = new List<Panel>();
            this.panelScreen.Controls.Clear();
            // set current screen to 0, sothat we start everything from default
            currentScreen = 0;

            int totalGameWindowsIndex = 0;
            // iterate through the screens
            for (int screenIdx = 0; screenIdx < totalNumberOfScreens; screenIdx++)
            {
                // iterate through the game windows
                for (int gameWindowIdx = 0; gameWindowIdx < myScreens[screenIdx].getTotalNumberOfGameWindows(); gameWindowIdx++)
                {
                    Panel gameWindow = new Panel();

                    gameWindow.BackColor = Color.Red;

                    // add events
                    gameWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseDown);
                    gameWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseUp);
                    gameWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseMove);
                    gameWindow.MouseEnter += new System.EventHandler(this.gameWindow_MouseEnter);
                    gameWindow.MouseLeave += new System.EventHandler(this.gameWindow_MouseLeave);
                    // store the game window
                    gameWindows.Add(gameWindow);

                    // add game window to screen control panel
                    this.panelScreen.Controls.Add(gameWindows[totalGameWindowsIndex]);

                    // set game window's dimensions and coordinates
                    gameWindows[totalGameWindowsIndex].Location = new Point(myScreens[screenIdx].xCoord[gameWindowIdx], myScreens[screenIdx].yCoord[gameWindowIdx]);
                    gameWindows[totalGameWindowsIndex].Height = myScreens[screenIdx].height[gameWindowIdx];
                    gameWindows[totalGameWindowsIndex].Width = myScreens[screenIdx].width[gameWindowIdx];
                    gameWindows[totalGameWindowsIndex].BackColor = myScreens[screenIdx].colour[gameWindowIdx];

                    // Show/Hide game windows
                    if (screenIdx == 0) // we only want the game windows on the first screen to be visible
                    {
                        gameWindows[totalGameWindowsIndex].Visible = true;
                        gameWindows[totalGameWindowsIndex].Enabled = true;
                    }
                    else
                    {
                        gameWindows[totalGameWindowsIndex].Visible = false;
                        gameWindows[totalGameWindowsIndex].Enabled = false;
                    }

                    totalGameWindowsIndex++;
                    totalNumberOfGameWindows++;
                }
                txtNumberOfGamesOnCurrentScreen.Text = myScreens[screenIdx].getTotalNumberOfGameWindows().ToString();
            }
            // change the total on the GUI
            txtNumberOfGames.Text = totalNumberOfGameWindows.ToString();
            // set the variables and other things to reflect the loaded config file's data
            lblScreenNumber.Text = "Screen " + currentScreen + " of " + totalNumberOfScreens;
            cmbListOfScreens.Text = myScreens[currentScreen].screenName;
            txtAccountName.Text = "";
            txtAccountPassword.Text = "";
            txtExePath.Text = "";

        }



        private void newUpdateGameWindowLayout(int startingGameWindowNumber, int oldGameWindowTotal, int newGameWindowTotalOnScreen)
        {
            if (totalNumberOfGameWindows >= 0)
            {
                // update total # of game windows
                totalNumberOfGameWindows = totalNumberOfGameWindows - oldGameWindowTotal + newGameWindowTotalOnScreen;
                // change the total on the GUI
                txtNumberOfGames.Text = totalNumberOfGameWindows.ToString();

                // store the total # of game windows for the current screen
                myScreens[currentScreen].setTotalNumberOfGameWindows(newGameWindowTotalOnScreen);

                // build new game windows object
                // =============================
                // destroy old gameWindows object
                try
                {
                    gameWindows = new List<Panel>();
                    this.panelScreen.Controls.Clear();
                }
                catch
                {
                }

                // rebuild gameWindows
                for (int gameWindowIndex = 0; gameWindowIndex < totalNumberOfGameWindows; gameWindowIndex++)
                {
                    Panel gameWindow = new Panel();

                    gameWindow.BackColor = Color.Red;

                    // add events
                    gameWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseDown);
                    gameWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseUp);
                    gameWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gameWindow_MouseMove);
                    gameWindow.MouseEnter += new System.EventHandler(this.gameWindow_MouseEnter);
                    gameWindow.MouseLeave += new System.EventHandler(this.gameWindow_MouseLeave);
                    // store the game window
                    gameWindows.Add(gameWindow);

                    // add game window to screen control panel
                    this.panelScreen.Controls.Add(gameWindows[gameWindowIndex]);
                }

                // assign game windows to screens
                int currScrn = 0;
                for (int gameWindowIndex = 0; gameWindowIndex < totalNumberOfGameWindows; gameWindowIndex++)
                {
                    if (gameWindowIndex == startingGameWindowNumber)
                    { // we are dealing with the current screen
                        // check that we are on the correct screen
                        while (currentScreen != currScrn)
                            currScrn++;
                        for (int gameWindowOnCurrentScrnIndex = 0; gameWindowOnCurrentScrnIndex < newGameWindowTotalOnScreen; gameWindowOnCurrentScrnIndex++)
                        {
                            myScreens[currScrn].gameWindow[gameWindowOnCurrentScrnIndex] = gameWindowIndex;
                            // add game window to Position Index (so that we know if it is on top of or below anything)
                            myScreens[currScrn].addToPositionIndex(gameWindowOnCurrentScrnIndex, gameWindowIndex);
                            gameWindowIndex++;
                        }
                        gameWindowIndex--;
                    }
                    else
                    {
                        // retrieve total # of game windows on screen
                        int totalGameWindowsOnCurrentScrn = myScreens[currScrn].getTotalNumberOfGameWindows();
                        for (int gameWindowOnCurrentScrnIndex = 0; gameWindowOnCurrentScrnIndex < totalGameWindowsOnCurrentScrn; gameWindowOnCurrentScrnIndex++)
                        {
                            myScreens[currScrn].gameWindow[gameWindowOnCurrentScrnIndex] = gameWindowIndex;
                            // add game window to Position Index (so that we know if it is on top of or below anything)
                            myScreens[currScrn].addToPositionIndex(gameWindowOnCurrentScrnIndex, gameWindowIndex); // store the number of the game window related to overal #
                            gameWindowIndex++;
                        }
                        gameWindowIndex--;
                    }
                    currScrn++;
                }


                // set the game windows' X, Y, Height, and Width
                // generating anew the ones for the current screen
                int totalGameWindowsIndex = 0;
                for (int scrn = 0; scrn < totalNumberOfScreens; scrn++)
                {
                    if (scrn == currentScreen)
                    {
                        if (txtNumberOfGamesOnCurrentScreen.Text != "0") // make sure there are game windows to create
                        {
                            // need to gen a new game window layout
                            myScreens[currentScreen] = ConfigurationScreen.generateGameWindowsOnCurrentScreen(myScreens[currentScreen], 
                                startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width);
                            // need to alter the gameWindow as per its new settings
                            gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                                startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width, 
                                    Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));
                            totalGameWindowsIndex += newGameWindowTotalOnScreen;
                        }
                    }
                    else // transfer existing ones
                    {
                        // get total # of game windows on the screen (scrn)
                        int totalGameWindowsOnScrn = myScreens[scrn].getTotalNumberOfGameWindows();
                        for (int gameWindowIndx = 0; gameWindowIndx < totalGameWindowsOnScrn; gameWindowIndx++)
                        {
                            gameWindows[totalGameWindowsIndex].Location = new Point(myScreens[scrn].xCoord[gameWindowIndx], myScreens[scrn].yCoord[gameWindowIndx]);
                            gameWindows[totalGameWindowsIndex].Height = myScreens[scrn].height[gameWindowIndx];
                            gameWindows[totalGameWindowsIndex].Width = myScreens[scrn].width[gameWindowIndx];
                            gameWindows[totalGameWindowsIndex].BackColor = myScreens[scrn].colour[gameWindowIndx];
                            totalGameWindowsIndex++;
                        }
                    }
                }
            }

            // 
            int totalShown = 0;
            // Now show only the ones on the current screens
            for (int gameIdx = 0; gameIdx < totalNumberOfGameWindows; gameIdx++)
            {
                if (myScreens[currentScreen].exists(gameIdx))
                { // show the game window
                    totalShown++;
                    gameWindows[gameIdx].Enabled = true;
                    gameWindows[gameIdx].Visible = true;
                }
                else // hide the game window
                {
                    gameWindows[gameIdx].Enabled = false;
                    gameWindows[gameIdx].Visible = false;
                }
            }
        }



        /*
         * This method will display the game clients as per the user entries for the # of rows and columns
         * 
         */
        public void displayUserDefinedLayout(int startingGameWindowNumber, int rows, int columns)
        {
            int gameWindowNumber = startingGameWindowNumber;
            int gameWindowHeight = 0;
            int gameWindowWidth = 0;
            int totalNumberOfGamesAcross = 0;
            int totalNumberOfGamesDown = 0;
            int gameWindowXCoord = 0;
            int gameWindowYCoord = 0;

            // 0 to number of game windows on a specific screen

            // this will deal with odd # of screens that can still be arranged in this type of order
            totalNumberOfGamesAcross = columns;
            totalNumberOfGamesDown = rows;

            gameWindowHeight = Screen.PrimaryScreen.Bounds.Height / totalNumberOfGamesDown;
            gameWindowWidth = Screen.PrimaryScreen.Bounds.Width / totalNumberOfGamesAcross;

            gameWindowXCoord = Screen.PrimaryScreen.Bounds.X;
            gameWindowYCoord = Screen.PrimaryScreen.Bounds.Y;

            // display the game clients on the screen
            for (int row = 0; row < totalNumberOfGamesAcross; row++)
            {
                for (int column = 0; column < totalNumberOfGamesDown; column++)
                {
                    Process gameProcess;
                    // open the game client
                    gameProcess = new Process();
                    gameProcess.StartInfo.FileName = gamePath;
                    gameProcess.Start();

                    // store this client's process in myScreens object
                    myScreens[currentScreen].setProcess(gameProcess, startingGameWindowNumber + gameWindowNumber);

                    gameWindowXCoord = row * gameWindowWidth;
                    gameWindowYCoord = column * gameWindowHeight;

                    // store this client's coordinates, height, and width
                    myScreens[currentScreen].setGameClientXCoord(startingGameWindowNumber + gameWindowNumber, gameWindowXCoord);
                    myScreens[currentScreen].setGameClientYCoord(startingGameWindowNumber + gameWindowNumber, gameWindowYCoord);
                    myScreens[currentScreen].setGameClientHeight(startingGameWindowNumber + gameWindowNumber, gameWindowHeight);
                    myScreens[currentScreen].setGameClientWidth(startingGameWindowNumber + gameWindowNumber, gameWindowWidth);

                    // Now to change the location and size of the client window, if I can :-)
                    if (gameProcess.WaitForInputIdle(15000))
                        SetWindowPos(gameProcess.MainWindowHandle, this.Handle, gameWindowXCoord, gameWindowYCoord, gameWindowWidth, gameWindowHeight, SWP_SHOWWINDOW);

                    gameWindowNumber++;
                }
            }

        } // END OF displayUserDefinedLayout(int startingGameWindowNumber, int rows, int columns)


        #region Control events

        // ========================================================================
        // ============================   EVENTS  =================================
        // ========================================================================

        private void txtNumberOfGamesOnCurrentScreen_Leave(object sender, EventArgs e)
        {
            // make sure the value entered is valid (integer 1 to 100)
            try
            {
                int valueEntered = Convert.ToInt32(txtNumberOfGamesOnCurrentScreen.Text);

                if (valueEntered < 0 || valueEntered > 100)
                {
                    if (myScreens[currentScreen].getTotalNumberOfGameWindows() > 0)
                        txtNumberOfGamesOnCurrentScreen.Text = myScreens[currentScreen].getTotalNumberOfGameWindows().ToString();
                    else
                        txtNumberOfGamesOnCurrentScreen.Text = "0";
                }
                else // value entry :-)
                {

                    // only update the game windows if the # of game windows on the current screen has changed
                    if (valueEntered != myScreens[currentScreen].getTotalNumberOfGameWindows())
                    {
                        // KIll all the game clients, if any exist
                        if (myScreens[currentScreen].gameClientsExist)
                        {
                            RealWorldScreenManipulation.killAllClientsOnCurrentScreen(myScreens[currentScreen]);
                            myScreens[currentScreen].gameClientsExist = false; // no more game clients displayed on the screen
                        }
                        int oldGameWindowTotal = 0;
                        int startingGameWindowNumber = 0;
                        // check that the totalNumberOfGameWindows is not 0, as this will mean the myScreens object will not have been created
                        if (totalNumberOfGameWindows > 0)
                        {
                            oldGameWindowTotal = myScreens[currentScreen].getTotalNumberOfGameWindows();
                            startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                        }
                        myScreens[currentScreen].setTotalNumberOfGameWindows(valueEntered);
                        newUpdateGameWindowLayout(startingGameWindowNumber, oldGameWindowTotal, valueEntered);

                    }
                }
            }
            catch (Exception frack)
            {
                MessageBox.Show("Exception caught: " + frack);
                // invalid value entered
                txtNumberOfGamesOnCurrentScreen.Text = "0";
            }
            if (txtNumberOfGamesOnCurrentScreen.Text != "0")
            {
                // enable the user defined game window layout area (rows and columns input boxes)
                txtRows.Enabled = true;
                txtColumns.Enabled = true;
            }
            else
            {
                txtRows.Enabled = false;
                txtColumns.Enabled = false;
            }

            // set the primary game window
            primaryGameWindowIdx = 0;
            primaryGameWindowScreenIdx = 0;
            myScreens[0].setPrimaryGameClientState(0, true);
            // iterate through the screens
            for (int screenIdx = 0; screenIdx < totalNumberOfScreens; screenIdx++)
            {
                // iterate through the game windows
                int totalGameWindows = myScreens[screenIdx].getTotalNumberOfGameWindows();
                for (int gameWindowIdx = 1; gameWindowIdx < totalGameWindows; gameWindowIdx++)
                {
                    // set game window to NOT primary
                    myScreens[screenIdx].setPrimaryGameClientState(gameWindowIdx, false);
                }
            }

            MessageBox.Show("New primary client is on screen : " + primaryGameWindowScreenIdx + " and is game window : " + primaryGameWindowScreenIdx);
        }

        private void panelScreen_MouseUp(object sender, MouseEventArgs e)
        {
            draggingObject = false;
            GameWindow.resizing = false;
        }

        private void btnLoginToAllGames_Click(object sender, EventArgs e)
        {
            ClientInteraction.loginToClient(myScreens[currentScreen], currentGameWindow);

            /*
            // Recurse through all games and login to them
            for (int gameClientIdx = 0; gameClientIdx < totalNumberOfGameWindows; gameClientIdx++)
            {
                // close the client
                string ID = txtAccountName.Text;
                string password = txtAccountPassword.Text;


                // bring window forward
                SetForegroundWindow(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle);
                Thread.Sleep(1000); // wait 10 seconds

                SendKeys.Send("Dd9i1E3nB813o");
                Thread.Sleep(1000); // wait 10 seconds
                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYDOWN, VK_RETURN, 0);
                Thread.Sleep(1000); // wait 10 seconds
                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYUP, VK_RETURN, 0);
                Thread.Sleep(5000); // wait 10 seconds
                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYDOWN, VK_RETURN, 0);
                Thread.Sleep(1000); // wait 10 seconds
                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYUP, VK_RETURN, 0);


//                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, 0x000C, 0, Marshal.StringToHGlobalAuto("Hello"));

 //               SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, 0x000C, 0, Marshal.StringToHGlobalAuto("Hello"));

   //             SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, "Fred", VK_ESCAPE, 0);
                // send the data to the window
     //           SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYDOWN, VK_ESCAPE, 0);
     //           SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYUP, VK_ESCAPE, 0);
//                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYDOWN, VK_F, 0); 
//                SendMessage(myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle, WM_KEYUP, VK_F, 0);

        //        myScreens[currentScreen].getProcess(gameClientIdx).MainWindowHandle;
//                myScreens[currentScreen].getProcess(gameClientIdx).CloseMainWindow();
 //               myScreens[currentScreen].getProcess(gameClientIdx).WaitForExit();
            }
            */

        }

        private void checkPasswordEntry_CheckedChanged(object sender, EventArgs e)
        {
            // disable the password input box, as the user does not want the bot to enter this information
            if (checkPasswordEntry.Checked)
                txtAccountPassword.Enabled = false;
            else // allow the bot to enter the password
                txtAccountPassword.Enabled = true;
        }

        private void btnSaveConfigFile_Click(object sender, EventArgs e)
        {
            // Update the saved data
            // make sure the filename has been entered
            if (validConfigFileName)
            {
                if (File.Exists(txtConfigFileName.Text))
                {
                    // delete it then recreate it
                    File.Delete(txtConfigFileName.Text);
                }
                // save all data
                FileHandling.saveDataToFile(txtConfigFileName.Text, myScreens, totalNumberOfScreens);
                txtSceensInfo.Text = "Data has been saved to file";
            }
            else
                MessageBox.Show("A valid file name needs to be entered!");
        }

        private void btnLoadConfigFile_Click(object sender, EventArgs e)
        {

            // Update the saved data
            // make sure the filename has been entered
            if (validConfigFileName)
            {
                if (File.Exists(txtConfigFileName.Text))
                {
                    // save all data
                    myScreens = FileHandling.loadDataFromFile(txtConfigFileName.Text, myScreens);
                    txtSceensInfo.Text = "Data has been loaded from file";

                    // TEST THE LOADED STUFF
                    txtSceensInfo.Text = "ID: " + myScreens[0].screenID.ToString();
                    txtSceensInfo.Text += "\r\n Screen Name: " + myScreens[0].screenName;
                    txtSceensInfo.Text += "\r\n Height: " + myScreens[0].screenHeight;
                    txtSceensInfo.Text += "\r\n Width: " + myScreens[0].screenWidth;
                    txtSceensInfo.Text += "\r\n X coord: " + myScreens[0].screenXCoord;
                    txtSceensInfo.Text += "\r\n Y coord:" + myScreens[0].screenYCoord;
                    txtSceensInfo.Text += "\r\n Total game windows: " + myScreens[0].getTotalNumberOfGameWindows();

                    // gen the game windows
                    updateGameWindowLayout();
                }
                else
                    MessageBox.Show("File cannot be found!");
            }
            else
                MessageBox.Show("A valid file name needs to be entered!");
        }

        /*
         * This event kicks off the reconfiguring and displaying of the game windows in the design the user has requested
         */
        private void btnSetNewLayout_Click(object sender, EventArgs e)
        {
            // Make sure the input values are valid
            // a number
            // the row x column = total number of game windows on the current screen
            if (isValidRow && isValidColumn)
            {
                // make sure their product == total # of game windows on the screen
                if (Int32.Parse(txtRows.Text) * Int32.Parse(txtColumns.Text) == myScreens[currentScreen].getTotalNumberOfGameWindows())
                {
                    int rows = Int32.Parse(txtRows.Text);
                    int columns = Int32.Parse(txtColumns.Text);
                    int startingGameWindowNumber = 0;
                    for (int scrn = 0; scrn < currentScreen; scrn++)
                    {
                        startingGameWindowNumber += myScreens[scrn].getTotalNumberOfGameWindows();
                    }

                    ConfigurationScreen.generateUserDefinedGameWindowLayout(myScreens[currentScreen], startingGameWindowNumber, rows, columns,
                        panelScreen.Height, panelScreen.Width);

                    // need to alter the gameWindow as per its new settings
                    gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                        startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                            Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

  //                  userDefinedLayout = true;
                }
                else
                {
  //                  userDefinedLayout = false;
                    MessageBox.Show("ERROR - rows x columns is not equal to the number of game windows on the current screen!");
                }
            }
            else
                txtSceensInfo.Text = "Please enter the number of game windows to appear on this screen";
        }

        private void txtRows_Leave(object sender, EventArgs e)
        {
            isValidRow = true;
            // Make sure the input values are valid
            // a number
            // the row x column = total number of game windows on the current screen
            try
            {
                int value = Int32.Parse(txtRows.Text);
            }
            catch
            {
                // not a number
                MessageBox.Show("Invlid entry - it must be an integer!");
                txtRows.Text = "";
                isValidRow = false;
            }
        }

        private void txtColumns_Leave(object sender, EventArgs e)
        {
            isValidColumn = true;
            // Make sure the input values are valid
            // a number
            // the row x column = total number of game windows on the current screen
            try
            {
                int value = Int32.Parse(txtColumns.Text);
            }
            catch
            {
                // not a number
                MessageBox.Show("Invlid entry - it must be an integer!");
                txtColumns.Text = "";
                isValidColumn = false;
            }
        }

        // *******************************************************************
        //              Account Details Section
        // *******************************************************************
        #region AccountDetails

        /*
         * Game client and account related control.
         * This controls the editing of the path to the executable.
         * If checked, then the path entered will be used for all game clients.
         */
        private void checkUseExePathForAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUseExePathForAll.Checked)
            {
                // make sure path has been entered
                if (!File.Exists(txtExePath.Text))
                {
                    MessageBox.Show("You must enter a valid path!");
                }
                else // path has been entered
                {
                    txtExePath.Enabled = false;
                    MessageBox.Show("PATH: " + txtExePath.Text);

                    // assign path to all game clients
                    GameWindowOperations.assignSameExeToAllGameWindows(txtExePath.Text, totalNumberOfScreens, myScreens);


                }
            }
            else
            {
                // allow the user to edit the path
                txtExePath.Enabled = true;
            }
        }

        private void txtExePath_Leave(object sender, EventArgs e)
        {
            // make sure path has been entered
            if (!File.Exists(txtExePath.Text))
            {
                validPathToExe = false;
                txtSceensInfo.Text = "INAVID PATH TO EXE!";
                // disable the save button
                btnSaveConfigFile.Enabled = false;
            }
            else // path has been entered
            {
                validPathToExe = true;
                if (validAccountName && validPassword)
                {
                    // enable the save button
                    btnSaveConfigFile.Enabled = true;
                }
                myScreens[currentScreen].setExePath(txtExePath.Text, currentSelectedGameWindow);

                gamePath = txtExePath.Text;
            }
        }

        private void txtAccountName_Leave(object sender, EventArgs e)
        {
            // make sure path has been entered
            if (txtAccountName.Text == "" || txtAccountName.Text == "Account name")
            {
                validAccountName = false;
                MessageBox.Show("Please enter an account name!");
                // disable the save button
                btnSaveConfigFile.Enabled = false;
            }
            else // path has been entered
            {
                validAccountName = true;
                if (validPassword && validPathToExe)
                {
                    // enable the save button
                    btnSaveConfigFile.Enabled = true;
                    // disable the save button
                    btnSaveConfigFile.Enabled = false;
                }
                myScreens[currentScreen].setAccountName(txtAccountName.Text, currentSelectedGameWindow);
            }
        }

        private void txtAccountPassword_Leave(object sender, EventArgs e)
        {
            // make sure path has been entered
            if (txtAccountPassword.Text == "" || txtAccountPassword.Text == "Account name")
            {
                validPassword = false;
                MessageBox.Show("Please enter an account password!");
                // disable the save button
                btnSaveConfigFile.Enabled = false;
            }
            else // path has been entered
            {
                validPassword = true;
                if (validAccountName && validPathToExe)
                {
                    // enable the save button
                    btnSaveConfigFile.Enabled = true;
                }
                myScreens[currentScreen].setAccountPassword(txtAccountPassword.Text, currentSelectedGameWindow);
            }
        }

        private void txtConfigFileName_Leave(object sender, EventArgs e)
        {
            validConfigFileName = true;
            try
            {
                if (File.Exists(txtConfigFileName.Text))
                {
    //                File.Delete(txtConfigFileName.Text);
                    txtSceensInfo.Text = "File name appears to be valid";
                }
            }
            catch
            {
                MessageBox.Show("Invalid filename!");
                txtConfigFileName.Text = "";
                validConfigFileName = false;
            }
        }

        #endregion Accounts


        private void btnNextScreen_Click(object sender, EventArgs e)
        {
            if ((currentScreen + 1) < totalNumberOfScreens)
            {
                showHideGameWindows(currentScreen, false); // clear the previous screen's game windows
                currentScreen++;
                showHideGameWindows(currentScreen, true); // show the current screen's game windows
                // alter screen detail if we are not already at the last screen
                // change screen #
                lblScreenNumber.Text = "Screen " + (currentScreen + 1) + " of " + totalNumberOfScreens;
                // update # of game in input box
                txtNumberOfGamesOnCurrentScreen.Text = Convert.ToString(myScreens[currentScreen].getTotalNumberOfGameWindows());
                // change the Combobox details to the real world screen the current config screen relates to
                cmbListOfScreens.Text = myScreens[currentScreen].screenName;
            }
        }

        private void btnPrevScreen_Click(object sender, EventArgs e)
        {
            int screenNo = currentScreen;
            currentScreen--;

            // alter screen detail if we are not already on the first screen
            if (currentScreen >= 0)
            {
                showHideGameWindows(currentScreen + 1, false); // clear the previous screen's game windows
                // change screen #
                showHideGameWindows(currentScreen, true); // show the current screen's game windows
                // update # of game in input box
                txtNumberOfGamesOnCurrentScreen.Text = Convert.ToString(myScreens[currentScreen].getTotalNumberOfGameWindows());
                // change screen #
                lblScreenNumber.Text = "Screen " + (currentScreen + 1) + " of " + totalNumberOfScreens;
                // change the Combobox details to the real world screen the current config screen relates to
                cmbListOfScreens.Text = myScreens[currentScreen].screenName;

            }
            else
                currentScreen = screenNo; // restore current screen number, as we were unable to change screens
        }

        private void gameWindow_MouseDown(object sender, MouseEventArgs e)
        {
            currentSelectedGameWindow = GameWindow.whichGameWindow(myScreens[currentScreen], this.Location.X, this.Location.Y, panelScreen.Location.X, panelScreen.Location.Y);
            
            gameWindows[currentGameWindow].BringToFront();
            // move game window to the top of the Position Index list
            myScreens[currentScreen].shiftGameWindowToTop(currentGameWindow);
            
            GameWindow.mousePrevXCoord = Cursor.Position.X;
            GameWindow.mousePrevYCoord = Cursor.Position.Y;

            if (GameWindow.topLeftResizing || GameWindow.topRightResizing || GameWindow.bottomLeftResizing || GameWindow.bottomRightResizing ||
                GameWindow.leftResizing || GameWindow.rightResizing || GameWindow.topResizing || GameWindow.bottomResizing)
            {
                GameWindow.resizing = true;
                GameWindow.changeToAppropriateCursor(draggingObject, currentGameWindow, this.Location.X, this.Location.Y,
                        panelScreen.Location.X, panelScreen.Location.Y, gameWindows);
            }
            else
            {
                draggingObject = true; // dragging
                GameWindow.changeToAppropriateCursor(draggingObject, currentGameWindow, this.Location.X, this.Location.Y,
                        panelScreen.Location.X, panelScreen.Location.Y, gameWindows);
            }

            // Make the account input area reflect the currently selected game window
            lblGameClient.Text = "Game client " + currentSelectedGameWindow;
            txtAccountName.Text = myScreens[currentScreen].getAccountName(currentSelectedGameWindow);
            txtAccountPassword.Text = myScreens[currentScreen].getAccountPassword(currentSelectedGameWindow);
            txtExePath.Text = myScreens[currentScreen].getExePath(currentSelectedGameWindow);
        }

        private void gameWindow_MouseUp(object sender, MouseEventArgs e)
        {
            draggingObject = false;
            GameWindow.resizing = false;
        }

        private void gameWindow_MouseMove(object sender, MouseEventArgs e)
        {
            // Only process this if we are not dragging something
            if (!draggingObject)
            {
                currentGameWindow = GameWindow.whichGameWindow(myScreens[currentScreen], this.Location.X, this.Location.Y, panelScreen.Location.X, panelScreen.Location.Y);
                // MyScreen currentScreen, int xCoordOfGUI, int yCordOfGUI, int panelScreenXcoord, int panelScreenYcoord
                // check where we are 
                GameWindow.changeToAppropriateCursor(draggingObject, currentGameWindow, this.Location.X, this.Location.Y,
                        panelScreen.Location.X, panelScreen.Location.Y, gameWindows);
            }

            // are we dragging this control ?
            if (draggingObject)
            {
                //               txtSceensInfo.Text += "DRAGGING \r\n";
                // are we attempting to move a game window ?
                // if so, then move it
                gameWindows[currentGameWindow] = GameWindow.moveObjectCheck(this.Location.X, this.Location.Y, panelScreen.Location.X,
                    panelScreen.Location.Y, panelScreen.Width, panelScreen.Height, gameWindows[currentGameWindow]);
                // now update the corresponding myScreens object (myScreens - used for game window layout config and saving to file)
                myScreens[currentScreen].upDateMyScreensObject(gameWindows[currentGameWindow], myScreens[currentScreen].getGameWindowScreenIdx(currentGameWindow));
                // now update the corresponding client if it exists
                if (myScreens[currentScreen].gameClientsExist)
                {
                    RealWorldScreenManipulation.reacquireGameClientDimensions(myScreens[currentScreen], currentGameWindow, panelScreen.Height,
                        panelScreen.Width);
                    RealWorldScreenManipulation.redisplayGameClient(myScreens[currentScreen], currentGameWindow);
                }
            }
            else if (GameWindow.resizing)
            {
                //               txtSceensInfo.Text += "RESIZING \r\n";
                // which direction  // this.Location.X + panelScreen.Location.X
                gameWindows[currentGameWindow] = GameWindow.resizeObjectCheck(currentGameWindow, this.Location.X, this.Location.Y,
                     panelScreen.Location.X, panelScreen.Location.Y, panelScreen.Width, panelScreen.Height, gameWindows[currentGameWindow],
                     Cursor.Position.X, Cursor.Position.Y);
                // now update the corresponding myScreens object (myScreens - used for game window layout config and saving to file)
                myScreens[currentScreen].upDateMyScreensObject(gameWindows[currentGameWindow], myScreens[currentScreen].getGameWindowScreenIdx(currentGameWindow));
                // now update the corresponding client if it exists
                if (myScreens[currentScreen].gameClientsExist)
                {
                    RealWorldScreenManipulation.reacquireGameClientDimensions(myScreens[currentScreen], currentGameWindow, panelScreen.Height,
                        panelScreen.Width);
                    RealWorldScreenManipulation.redisplayGameClient(myScreens[currentScreen], currentGameWindow);
                }
            }
            else
            {
                //                txtSceensInfo.Text += "NOT RESIZING \r\n";
                // we are moving across the game window
                // display the relevant cursor depending on our location within the game window
                GameWindow.changeToAppropriateCursor(draggingObject, currentGameWindow, this.Location.X, this.Location.Y,
                        panelScreen.Location.X, panelScreen.Location.Y, gameWindows);
            }
        } // END OF gameWindow_MouseMove(...)

        private void gameWindow_MouseEnter(object sender, EventArgs e)
        {
            // Only process this if we are not dragging something

            // what part of the game window have we crossed into ?
            // -- CORNER - top-right, top-left, bottom-right, bottom-left
            // -- SIDE - top, bottom, left, right

            // work out which game windiow we have entered

            if (!draggingObject)
            {
                currentGameWindow = GameWindow.whichGameWindow(myScreens[currentScreen], this.Location.X, this.Location.Y, panelScreen.Location.X, panelScreen.Location.Y);

                if (currentGameWindow != 999)
                {
                    // store old colour
                    //               originalColour = myScreens[currentScreen].colour[currentGameWindow];
                    // change colour to white
                    //               gameWindows[currentGameWindow].BackColor = Color.Wheat; 

                }

                // what area have we entered  // this.Location.X + panelScreen.Location.X
                int gameWindowEntryPoint = GameWindow.getGameWindowEntryPoint(currentGameWindow, this.Location.X, this.Location.Y,
                        panelScreen.Location.X, panelScreen.Location.Y, gameWindows);
            }
        }

        private void gameWindow_MouseLeave(object sender, EventArgs e)
        {
            //          gameWindows[currentGameWindow].BackColor = originalColour; 
        }

        private void panelScreen_MouseMove(object sender, MouseEventArgs e)
        {
            //            txtSceensInfo.Text += "ANYTHING HAPPENING NOW, BIATCH? \r\n";
        }

        // ****************************************************
        //     Change the layout design of the game windows
        // ****************************************************
        private void btnLayout1_Click(object sender, EventArgs e)
        {
            // only allow this if the total # of game windows on the current screen is 1
            if (myScreens[currentScreen].getTotalNumberOfGameWindows() == 1)
            {
                int startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                // action this to display the game window over the entire screen (cover the screen)
                // need to gen a new game window layout
                //             int rows, int columns, int panelScreenHeight, int panelScreenWidth)
                myScreens[currentScreen] = ConfigurationScreen.generateGameWindowsOnCurrentScreen(myScreens[currentScreen],
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width);
                // need to alter the gameWindow as per its new settings
                gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                        Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

                // disable this button, and enable all the others

                btnLayout1.Enabled = false;
                btnLayout2.Enabled = true;
                btnLayout3.Enabled = true;
                btnLayout4.Enabled = true;
                btnLayout5.Enabled = true;
                btnLayout6.Enabled = true;
                this.btnLayout1.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout1b;
                this.btnLayout2.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout2;
                this.btnLayout3.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout3;
                this.btnLayout4.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout4;
                this.btnLayout5.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout5;
                this.btnLayout6.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout6;

            }
            else
            {
                // error, 0 or more than 1 game window!
                txtSceensInfo.Text = "ERROR - Unable to do that, because the total number of game windows on this screen is not 1!";
            }

        }

        private void btnLayout2_Click(object sender, EventArgs e)
        {
            // only allow this if the total # of game windows on the current screen is 1
            if ((Math.Sqrt(myScreens[currentScreen].getTotalNumberOfGameWindows()) % 1) == 0)
            {
                int rowColumn = Convert.ToInt32(Math.Sqrt(myScreens[currentScreen].getTotalNumberOfGameWindows()));
                int startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                // action this to display the game window over the entire screen (cover the screen)
                // need to gen a new game window layout
                //             int rows, int columns, int panelScreenHeight, int panelScreenWidth)
                myScreens[currentScreen] = ConfigurationScreen.generateUserDefinedGameWindowLayout(myScreens[currentScreen],
                    startingGameWindowNumber, rowColumn, rowColumn, this.panelScreen.Height, this.panelScreen.Width);

                // need to alter the gameWindow as per its new settings
                gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                        Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

                // disable this button, and enable all the others

                btnLayout1.Enabled = true;
                btnLayout2.Enabled = false;
                btnLayout3.Enabled = true;
                btnLayout4.Enabled = true;
                btnLayout5.Enabled = true;
                btnLayout6.Enabled = true;
                this.btnLayout1.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout1;
                this.btnLayout2.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout2b;
                this.btnLayout3.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout3;
                this.btnLayout4.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout4;
                this.btnLayout5.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout5;
                this.btnLayout6.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout6;
            }
            else
            {
                // error, not just 1 game window
                txtSceensInfo.Text = "ERROR - That number of game windows cannot be displayed in the requests pattern!";
            }
        }

        private void btnLayout3_Click(object sender, EventArgs e)
        {
            // only allow this if the total # of game windows on the current screen is 1
            if (myScreens[currentScreen].getTotalNumberOfGameWindows() > 5)
            {
                int startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                // action this to display the game window over the entire screen (cover the screen)
                // need to gen a new game window layout
                //             int rows, int columns, int panelScreenHeight, int panelScreenWidth)
                myScreens[currentScreen] = ConfigurationScreen.generate1BigManySmallTopAndRightSideGameWindowLayout(myScreens[currentScreen],
                    startingGameWindowNumber, myScreens[currentScreen].getTotalNumberOfGameWindows(), this.panelScreen.Height, this.panelScreen.Width);

                // need to alter the gameWindow as per its new settings
                gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                        Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

                // disable this button, and enable all the others

                btnLayout1.Enabled = true;
                btnLayout2.Enabled = true;
                btnLayout3.Enabled = false;
                btnLayout4.Enabled = true;
                btnLayout5.Enabled = true;
                btnLayout6.Enabled = true;
                this.btnLayout1.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout1;
                this.btnLayout2.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout2;
                this.btnLayout3.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout3b;
                this.btnLayout4.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout4;
                this.btnLayout5.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout5;
                this.btnLayout6.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout6;

            }
            else
            {
                // error, not just 1 game window
                txtSceensInfo.Text = "ERROR - Unable to do that, because the total number of game windows on this screen is not 1!";
            }
        }

        private void btnLayout4_Click(object sender, EventArgs e)
        {
            // only allow this if the total # of game windows is greater than 2
            if (myScreens[currentScreen].getTotalNumberOfGameWindows() > 2)
            {
                int startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                // action this to display the game window over the entire screen (cover the screen)
                // need to gen a new game window layout
                //             int rows, int columns, int panelScreenHeight, int panelScreenWidth)
                myScreens[currentScreen] = ConfigurationScreen.generate1BigManySmallRightSideGameWindowLayout(myScreens[currentScreen], 
                    startingGameWindowNumber, myScreens[currentScreen].getTotalNumberOfGameWindows(), panelScreen.Height, panelScreen.Width);
                // need to alter the gameWindow as per its new settings
                gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                        Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

                // disable this button, and enable all the others

                btnLayout1.Enabled = true;
                btnLayout2.Enabled = true;
                btnLayout3.Enabled = true;
                btnLayout4.Enabled = false;
                btnLayout5.Enabled = true;
                btnLayout6.Enabled = true;
                this.btnLayout1.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout1;
                this.btnLayout2.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout2;
                this.btnLayout3.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout3;
                this.btnLayout4.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout4b;
                this.btnLayout5.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout5;
                this.btnLayout6.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout6;

            }
            else
            {
                // error, not just 1 game window
                txtSceensInfo.Text = "ERROR - Unable to do that, because the total number of game windows on this screen is not 1!";
            }
        }

        private void btnLayout5_Click(object sender, EventArgs e)
        {
            // only allow this if the total # of game windows is greater than 2
            if (myScreens[currentScreen].getTotalNumberOfGameWindows() > 2)
            {
                int startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                // action this to display the game window over the entire screen (cover the screen)
                // need to gen a new game window layout
                //             int rows, int columns, int panelScreenHeight, int panelScreenWidth)
                myScreens[currentScreen] = ConfigurationScreen.generate1BigManySmallLeftSideGameWindowLayout(myScreens[currentScreen],
                    startingGameWindowNumber, myScreens[currentScreen].getTotalNumberOfGameWindows(), panelScreen.Height, panelScreen.Width);
                // need to alter the gameWindow as per its new settings
                gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                        Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

                // disable this button, and enable all the others

                btnLayout1.Enabled = true;
                btnLayout2.Enabled = true;
                btnLayout3.Enabled = true;
                btnLayout4.Enabled = true;
                btnLayout5.Enabled = false;
                btnLayout6.Enabled = true;
                this.btnLayout1.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout1;
                this.btnLayout2.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout2;
                this.btnLayout3.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout3;
                this.btnLayout4.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout4;
                this.btnLayout5.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout5b;
                this.btnLayout6.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout6;

            }
            else
            {
                // error, not just 1 game window
                txtSceensInfo.Text = "ERROR - Unable to do that, because the total number of game windows on this screen is not 1!";
            }
        }

        private void btnLayout6_Click(object sender, EventArgs e)
        {
            // only allow this if the total # of game windows on the current screen is 1
            if (myScreens[currentScreen].getTotalNumberOfGameWindows() > 5)
            {
                int startingGameWindowNumber = ConfigurationScreen.getStartingGameWindowIndex(currentScreen, myScreens);
                // action this to display the game window over the entire screen (cover the screen)
                // need to gen a new game window layout
                //             int rows, int columns, int panelScreenHeight, int panelScreenWidth)
                myScreens[currentScreen] = ConfigurationScreen.generate1BigManySmallTopAndLeftSideGameWindowLayout(myScreens[currentScreen],
                    startingGameWindowNumber, myScreens[currentScreen].getTotalNumberOfGameWindows(), this.panelScreen.Height, this.panelScreen.Width);

                // need to alter the gameWindow as per its new settings
                gameWindows = ConfigurationScreen.placeGameWindowsOnCurrentScreen(myScreens[currentScreen], gameWindows,
                    startingGameWindowNumber, this.panelScreen.Height, this.panelScreen.Width,
                        Int32.Parse(txtNumberOfGamesOnCurrentScreen.Text));

                // disable this button, and enable all the others

                btnLayout1.Enabled = true;
                btnLayout2.Enabled = true;
                btnLayout3.Enabled = true;
                btnLayout4.Enabled = true;
                btnLayout5.Enabled = true;
                btnLayout6.Enabled = false;
                this.btnLayout1.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout1;
                this.btnLayout2.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout2;
                this.btnLayout3.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout3;
                this.btnLayout4.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout4;
                this.btnLayout5.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout5;
                this.btnLayout6.BackgroundImage = global::BoxerTasticMate.Properties.Resources.layout6b;

            }
            else
            {
                // error, not just 1 game window
                txtSceensInfo.Text = "ERROR - Unable to do that, because the total number of game windows on this screen is not 1!";
            }
        }

        private void cmbListOfScreens_SelectedIndexChanged(object sender, EventArgs e)
        {
            // SWAP AROUND SCREENS

            // okay, so setting a new screen for this config window means that it nicks it from another config wondow.
            // so we need to assign this config wndow's previous real world screen to the aggrieved config wondow

            int indexOfConfigWindowWeAreSteelingFrom = realWorldScreens[cmbListOfScreens.SelectedIndex].currentlyAssignedToConfigScreen;
            int previousRealWorldScreen = myScreens[currentScreen].screenID;

            // Back up the current details
            int screneID = myScreens[currentScreen].screenID;
            string screenName = myScreens[currentScreen].screenName;
            int screenHeight = myScreens[currentScreen].screenHeight;
            int screenWidth = myScreens[currentScreen].screenWidth;
            int screenXCoord = myScreens[currentScreen].screenXCoord;
            int screenYCoord = myScreens[currentScreen].screenYCoord;

            myScreens[currentScreen].screenID = myScreens[indexOfConfigWindowWeAreSteelingFrom].screenID;
            myScreens[currentScreen].screenName = myScreens[indexOfConfigWindowWeAreSteelingFrom].screenName;
            myScreens[currentScreen].screenHeight = myScreens[indexOfConfigWindowWeAreSteelingFrom].screenHeight;
            myScreens[currentScreen].screenWidth = myScreens[indexOfConfigWindowWeAreSteelingFrom].screenWidth;
            myScreens[currentScreen].screenXCoord = myScreens[indexOfConfigWindowWeAreSteelingFrom].screenXCoord;
            myScreens[currentScreen].screenYCoord = myScreens[indexOfConfigWindowWeAreSteelingFrom].screenYCoord;

            myScreens[indexOfConfigWindowWeAreSteelingFrom].screenID = screneID;
            myScreens[indexOfConfigWindowWeAreSteelingFrom].screenName = screenName;
            myScreens[indexOfConfigWindowWeAreSteelingFrom].screenHeight = screenHeight;
            myScreens[indexOfConfigWindowWeAreSteelingFrom].screenWidth = screenWidth;
            myScreens[indexOfConfigWindowWeAreSteelingFrom].screenXCoord = screenXCoord;
            myScreens[indexOfConfigWindowWeAreSteelingFrom].screenYCoord = screenYCoord;

            int previousConfigScreen = realWorldScreens[cmbListOfScreens.SelectedIndex].currentlyAssignedToConfigScreen;
            // set the currently selected real world screen to point to the current config screen
            realWorldScreens[cmbListOfScreens.SelectedIndex].currentlyAssignedToConfigScreen = currentScreen;
            // set the previous screen of the current config screen to point to the selected screen's previous owner/user
            realWorldScreens[screneID].currentlyAssignedToConfigScreen = previousConfigScreen;

            // Generate a new layout for the game clients if they exist...
            RealWorldScreenManipulation.moveGameClientLayout(myScreens[currentScreen], panelScreen.Height, panelScreen.Width);

        }

        /*
         * Event for the exit button on the pop up window, that appears when user tests to see if they have the correct
         * real world screen
         */
        private void btnPoUpButtonExit_Click(object sender, EventArgs e)
        {
            popUpForm.Hide();
        }

        /*
         * This will send somthing to the currently selected real world screen (ComboBox), so that the user can be sure
         * that the screen so the one they want to use
         */
        private void btnTestRealWorldScreen_Click(object sender, EventArgs e)
        {
            int xCoord = myScreens[currentScreen].screenXCoord + (myScreens[currentScreen].screenWidth / 2) - 200;
            int yCoord = myScreens[currentScreen].screenYCoord + (myScreens[currentScreen].screenHeight / 2) - 200;
            popUpForm.Show();
            popUpForm.ControlBox = false;
            popUpForm.Location = new Point(xCoord, yCoord); // this has to be set after the Show command, or it will not display the
                                                            // window in the correct location when first called.
        }

        /*
         * This method closes all games clients
         */
        private void btnCloseAllGames_Click(object sender, EventArgs e)
        {
            // Iterate through the screens, then the game clients
            for (int screenIdx = 0; screenIdx < totalNumberOfScreens; screenIdx++)
            {
                // iterate through the clients on the screen
                RealWorldScreenManipulation.killAllClientsOnCurrentScreen(myScreens[screenIdx]);
                myScreens[screenIdx].gameClientsExist = false; // required for when we wish to redesign the game client layout
            }
        }

        private void btnLaunchAllGames_Click(object sender, EventArgs e)
        {
            for (int screenIdx=0; screenIdx < totalNumberOfScreens; screenIdx++)
            {
                if (myScreens[screenIdx].getGameClientHeight(0) == 0) // if height is 0, then the clients' deatils need to be generated
                    myScreens[screenIdx] = RealWorldScreenManipulation.reacquireGameClientLayout(myScreens[screenIdx], panelScreen.Height, panelScreen.Width);
                RealWorldScreenManipulation.displayGameWindowsOnCurrentScreen(myScreens[screenIdx]);
                myScreens[screenIdx].gameClientsExist = true; // required for when we wish to redesign the game client layout
            }
        }

        private void btnCloseGamesOnCurrentScreen_Click(object sender, EventArgs e)
        {
            // make sure there are clients on the screen
            if (myScreens[currentScreen].gameClientsExist)
                RealWorldScreenManipulation.killAllClientsOnCurrentScreen(myScreens[currentScreen]);
            myScreens[currentScreen].gameClientsExist = false; // required for when we wish to redesign the game client layout
        }


        private void btnLaunchGamesOnCurrentScreen_Click(object sender, EventArgs e)
        {
            if (myScreens[currentScreen].getGameClientHeight(0) == 0) // if height is 0, then the clients' deatils need to be generated
                myScreens[currentScreen] = RealWorldScreenManipulation.reacquireGameClientLayout(myScreens[currentScreen], panelScreen.Height, panelScreen.Width);
            RealWorldScreenManipulation.displayGameWindowsOnCurrentScreen(myScreens[currentScreen]);

            myScreens[currentScreen].gameClientsExist = true; // required for when we wish to redesign the game client layout
        }

      
        #endregion Control events





        private void button2_Click(object sender, EventArgs e)
        {
            // TEST AUTO LOGIN

            // move to account name entry box
            int accountNameTopLeftXcoordinate = 797;
            int accountNameTopLeftYcoordinate = 822;
            int accountNameBottomLeftXcoordinate = 890;
            int accountNameBottomLeftYcoordinate = 830;

            // randomize the actual X and Y coordinate we will use
            Random randomNumber = new Random();
            int X = randomNumber.Next(accountNameTopLeftXcoordinate, accountNameBottomLeftXcoordinate);
            int Y = randomNumber.Next(accountNameTopLeftYcoordinate, accountNameBottomLeftYcoordinate);

            // Move mouse to Account name entry box
            MouseHandling.mouseMove(X, Y);

            // Select the account name entry box
            MouseHandling.leftClick(X, Y);
            // Highlight the text area - send CTRL+A
            Thread.Sleep(1000);
            KeyboardHandling.keyDown(KeyboardHandling.VK_LCONTROL);
            Thread.Sleep(1000);
            KeyboardHandling.keyDown(KeyboardHandling.VK_A);
            Thread.Sleep(1000);
            KeyboardHandling.keyUp(KeyboardHandling.VK_A);
            Thread.Sleep(1000);
            KeyboardHandling.keyUp(KeyboardHandling.VK_LCONTROL);

            // Send the account name
            Thread.Sleep(1000);
            KeyboardHandling.keyDown(KeyboardHandling.VK_A);
            Thread.Sleep(1000);
            KeyboardHandling.keyUp(KeyboardHandling.VK_A);
            Thread.Sleep(1000);
        }





        /*
         * This adds all the keys in the Keys to be Broadcast list into the keyUpDownState Dictionary, so that
         * we can set and monitor the PRESSED and RELEASED state of the keys
         */
        private void populateKeyStateList()
        {
            int totalKeysInList = lbKeysToBeBroadcast.Items.Count;
            // add the keys

            foreach (string lbItem in lbKeysToBeBroadcast.Items)
            {
                // need to make sure the target key is not already part of the list (re key combinations)
                string [] keyCombination;
                keyCombination = lbItem.Split('+');
                int totalKeysInKeyCombination = keyCombination.Length;
                if (totalKeysInKeyCombination > 1)
                {
                    // add the key combination
                    keyUpDownState.Add(lbItem, "UP");
                    // test to see if the target key is already in the list
                    if (!keyUpDownState.ContainsKey(keyCombination[totalKeysInKeyCombination - 1]))
                    {
                        keyUpDownState.Add(keyCombination[totalKeysInKeyCombination - 1], "UP");
                    }
                }
                else // single key broadcasting
                    keyUpDownState.Add(lbItem, "UP");

            }
        }


        /*
         * This starts the multi-boxer client off. 
         * It will start listenning for key entires by the user, and will broadcast them to the game clients
         */
        private void btnActivateBoxer_Click(object sender, EventArgs e)
        { 
            // this will hold the key state (UP/DOWN) of all keys to be broadcast (in the value field)
            keyUpDownState.Clear();
            // add the keys + state of UP to the keyUpDownState list
            populateKeyStateList();

            // NEED TO START / STOP THIS

            // activate keyboard hook, so that we can monitor the keyboard
            keyboardHooky = new InterceptKeys(this);

            // set up a timer
            keyBroadcastTimer = new System.Timers.Timer(20);
            // Hook up the Elapsed event for the timer. 
            keyBroadcastTimer.Elapsed += OnTimedEvent;
 //           keyBroadcastTimer.Start();
            btnActivateBoxer.Text = "Boxer Activated!";
            btnActivateBoxer.Enabled = false; // disable the button

        }



        //
        //              ACCESSOR METHODS 
        //              ================
        /*
        // total number of screens
        public int getTotalNumberOfScreens()
        {
            return totalNumberOfScreens;
        }

        // total number of screens
        public List<MyScreen> getMyScreens()
        {
            return myScreens;
        }

        */










        // ******************************************************************************
        //                           KEY BROADCASTING
        // ******************************************************************************
        #region KeyBroadcasting

        public void stopTimer()
        {
            keyBroadcastTimer.Stop();
        }

        public static void initialiseKeyFlags()
        {
            // keys 0 to 9
            zeroKeyState = "UP";
            oneKeyState = "UP";
            twoKeyState = "UP";
            threeKeyState = "UP";
            fourKeyState = "UP";
            fiveKeyState = "UP";
            sixKeyState = "UP";
            sevenKeyState = "UP";
            eightKeyState = "UP";
            nineKeyState = "UP";
            // keys a to z
            aKeyState = "UP";
            bKeyState = "UP";
            cKeyState = "UP";
            dKeyState = "UP";
            eKeyState = "UP";
            fKeyState = "UP";
            gKeyState = "UP";
            hKeyState = "UP";
            iKeyState = "UP";
            jKeyState = "UP";
            kKeyState = "UP";
            lKeyState = "UP";
            mKeyState = "UP";
            nKeyState = "UP";
            oKeyState = "UP";
            pKeyState = "UP";
            qKeyState = "UP";
            rKeyState = "UP";
            sKeyState = "UP";
            tKeyState = "UP";
            uKeyState = "UP";
            vKeyState = "UP";
            wKeyState = "UP";
            xKeyState = "UP";
            yKeyState = "UP";
            zKeyState = "UP";
            // keys F1 to F12
            F1KeyState = "UP";
            F2KeyState = "UP";
            F3KeyState = "UP";
            F4KeyState = "UP";
            F5KeyState = "UP";
            F6KeyState = "UP";
            F7KeyState = "UP";
            F8KeyState = "UP";
            F9KeyState = "UP";
            F10KeyState = "UP";
            F11KeyState = "UP";
            F12KeyState = "UP";
            // keys Numpad 0 to 9
            numpad0KeyState = "UP";
            numpad1KeyState = "UP";
            numpad2KeyState = "UP";
            numpad3KeyState = "UP";
            numpad4KeyState = "UP";
            numpad5KeyState = "UP";
            numpad6KeyState = "UP";
            numpad7KeyState = "UP";
            numpad8KeyState = "UP";
            numpad9KeyState = "UP";
            // keys Symbols - + . / * etc
            multiplyKeyState = "UP";
            addKeyState = "UP";
            separatorKeyState = "UP";
            subtractKeyState = "UP";
            decimalKeyState = "UP";
            divideKeyState = "UP";
            // keys Other - (SPACEBAR, ESC, INSERT, Arrow Keys, NUMLOCK, etc)
            backspaceKeyState = "UP";
            tabKeyState = "UP";
            returnKeyState = "UP";
            shiftKeyState = "UP";
            pauseKeyState = "UP";
            capsLockKeyState = "UP";
            escapeKeyState = "UP";
            spacebarKeyState = "UP";
            pageUpKeyState = "UP";
            pageDownKeyState = "UP";
            endKeyState = "UP";
            homeKeyState = "UP";
            leftArrowKeyState = "UP";
            upArrowKeyState = "UP";
            rightArrowKeyState = "UP";
            downArrowKeyState = "UP";
            printScreenKeyState = "UP";
            insertKeyState = "UP";
            deleteKeyState = "UP";
            helpKeyState = "UP";
            numLockKeyState = "UP";
            scrollLockKeyState = "UP";
        }

        /*
         * This method sets the flag associated with the key passed to it, to "DOWN"
         * This signlas that the key is pressed down
         */
        private static void setKeyFlagState(int keyCode)
        {
            switch (keyCode)
            {
                // keys 0 to 9
                case 0x30: // 0
                    zeroKeyState = "DOWN";
                    break;
                case 0x31: // 1
                    oneKeyState = "DOWN";
                    break;
                case 0x32: // 2
                    twoKeyState = "DOWN";
                    break;
                case 0x33: // 3
                    threeKeyState = "DOWN";
                    break;
                case 0x34: // 4
                    fourKeyState = "DOWN";
                    break;
                case 0x35: // 5
                    fiveKeyState = "DOWN";
                    break;
                case 0x36: // 6
                    sixKeyState = "DOWN";
                    break;
                case 0x37: // 7
                    sevenKeyState = "DOWN";
                    break;
                case 0x38: // 8
                    eightKeyState = "DOWN";
                    break;
                case 0x39: // 9
                    nineKeyState = "DOWN";
                    break;

                // keys a to z
                case 0x41: // A
                    aKeyState = "DOWN";
                    break;
                case 0x42: // B
                    bKeyState = "DOWN";
                    break;
                case 0x43: // C
                    cKeyState = "DOWN";
                    break;
                case 0x44: // D
                    dKeyState = "DOWN";
                    break;
                case 0x45: // E
                    eKeyState = "DOWN";
                    break;
                case 0x46: // F
                    fKeyState = "DOWN";
                    break;
                case 0x47: // G
                    gKeyState = "DOWN";
                    break;
                case 0x48: // H
                    hKeyState = "DOWN";
                    break;
                case 0x49: // I
                    iKeyState = "DOWN";
                    break;
                case 0x4A: // J
                    jKeyState = "DOWN";
                    break;
                case 0x4B: // K
                    kKeyState = "DOWN";
                    break;
                case 0x4C: // L
                    lKeyState = "DOWN";
                    break;
                case 0x4D: // M
                    mKeyState = "DOWN";
                    break;
                case 0x4E: // N
                    nKeyState = "DOWN";
                    break;
                case 0x4F: // O
                    oKeyState = "DOWN";
                    break;
                case 0x50: // P
                    pKeyState = "DOWN";
                    break;
                case 0x51: // Q
                    qKeyState = "DOWN";
                    break;
                case 0x52: // R
                    rKeyState = "DOWN";
                    break;
                case 0x53: // S
                    sKeyState = "DOWN";
                    break;
                case 0x54: // T
                    tKeyState = "DOWN";
                    break;
                case 0x55: // U
                    uKeyState = "DOWN";
                    break;
                case 0x56: // V
                    vKeyState = "DOWN";
                    break;
                case 0x57: // W
                    wKeyState = "DOWN";
                    break;
                case 0x58: // X
                    xKeyState = "DOWN";
                    break;
                case 0x59: // Y
                    yKeyState = "DOWN";
                    break;
                case 0x5A: // Z
                    zKeyState = "DOWN";
                    break;

                // keys F1 to F12
                case 0x70: // F1
                    F1KeyState = "DOWN";
                    break;
                case 0x71: // F2
                    F2KeyState = "DOWN";
                    break;
                case 0x72: // F3
                    F3KeyState = "DOWN";
                    break;
                case 0x73: // F4
                    F4KeyState = "DOWN";
                    break;
                case 0x74: // F5
                    F5KeyState = "DOWN";
                    break;
                case 0x75: // F6
                    F6KeyState = "DOWN";
                    break;
                case 0x76: // F7
                    F7KeyState = "DOWN";
                    break;
                case 0x77: // F8
                    F8KeyState = "DOWN";
                    break;
                case 0x78: // F9
                    F9KeyState = "DOWN";
                    break;
                case 0x79: // F10
                    F10KeyState = "DOWN";
                    break;
                case 0x80: // F11
                    F11KeyState = "DOWN";
                    break;
                case 0x81: // F12
                    F12KeyState = "DOWN";
                    break;

                // keys Numpad 0 to 9
                case 0x60: // Numpad 0
                    numpad0KeyState = "DOWN";
                    break;
                case 0x61: // Numpad 1
                    numpad1KeyState = "DOWN";
                    break;
                case 0x62: // Numpad 2
                    numpad2KeyState = "DOWN";
                    break;
                case 0x63: // Numpad 3
                    numpad3KeyState = "DOWN";
                    break;
                case 0x64: // Numpad 4
                    numpad4KeyState = "DOWN";
                    break;
                case 0x65: // Numpad 5
                    numpad5KeyState = "DOWN";
                    break;
                case 0x66: // Numpad 6
                    numpad6KeyState = "DOWN";
                    break;
                case 0x67: // Numpad 7
                    numpad7KeyState = "DOWN";
                    break;
                case 0x68: // Numpad 8
                    numpad8KeyState = "DOWN";
                    break;
                case 0x69: // Numpad 9
                    numpad9KeyState = "DOWN";
                    break;

                // keys Symbols - + . / * etc
                case 0x6A: // Multiply
                    multiplyKeyState = "DOWN";
                    break;
                case 0x6B: // Add
                    addKeyState = "DOWN";
                    break;
                case 0x6C: // Separator
                    multiplyKeyState = "DOWN";
                    break;
                case 0x6D: // Subtract
                    separatorKeyState = "DOWN";
                    break;
                case 0x6E: // Decimal
                    decimalKeyState = "DOWN";
                    break;
                case 0x6F: // Divide
                    divideKeyState = "DOWN";
                    break;

                // keys Other - (SPACEBAR, ESC, INSERT, Arrow Keys, NUMLOCK, etc)
                case 0x08: // Backspace
                    backspaceKeyState = "DOWN";
                    break;
                case 0x09: // Tab
                    tabKeyState = "DOWN";
                    break;
                case 0x0d: // Return
                    returnKeyState = "DOWN";
                    break;
                case 0x10: // Shift
                    shiftKeyState = "DOWN";
                    break;
                case 0x13: // Pause
                    pauseKeyState = "DOWN";
                    break;
                case 0x14: // Caps Lock
                    capsLockKeyState = "DOWN";
                    break;
                case 0x1B: // ESC
                    escapeKeyState = "DOWN";
                    break;
                case 0x20: // Space Bar
                    spacebarKeyState = "DOWN";
                    break;
                case 0x21: // Page Up
                    pageUpKeyState = "DOWN";
                    break;
                case 0x22: // Page Down
                    pageDownKeyState = "DOWN";
                    break;
                case 0x23: // End
                    endKeyState = "DOWN";
                    break;
                case 0x24: // Home
                    homeKeyState = "DOWN";
                    break;
                case 0x25: // Left Arrow
                    leftArrowKeyState = "DOWN";
                    break;
                case 0x26: // Up Arrow
                    upArrowKeyState = "DOWN";
                    break;
                case 0x27: // Right Arrow
                    rightArrowKeyState = "DOWN";
                    break;
                case 0x28: // Down Arrow
                    downArrowKeyState = "DOWN";
                    break;
                case 0x2A: // Print Screen
                    printScreenKeyState = "DOWN";
                    break;
                case 0x2D: // Insert
                    insertKeyState = "DOWN";
                    break;
                case 0x2E: // Delete
                    deleteKeyState = "DOWN";
                    break;
                case 0x2F: // Help
                    helpKeyState = "DOWN";
                    break;
                case 0x90: // Num Lock
                    numLockKeyState = "DOWN";
                    break;
                case 0x91: // Scroll Lock
                    scrollLockKeyState = "DOWN";
                    break;

                default: // 
                    MessageBox.Show("No key pressed!");
                    break;
            }
        }


        /*
         * This will be used to process all key broadcasts
         */
        public void sendKey(int keyCode)
        {
            setKeyFlagState(keyCode); // find out what the flag is that goes with this key, then set it to "DOWN" (pressed)

            //     MessageBox.Show("Broadcast W key from the start");
            broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
        }


        /*
         * This method deals with key that are to be broadcast the the game windows prior to actually sending them.
         * This method reults in reducing the amount of code required in the OnTimedEvent mthod.
         */
        private bool checkStateOfKey_OLD(int keyCode, string keyState)
        {
            bool keyIsCurrentlyPressed = false;
            if (GetAsyncKeyState(keyCode) == 0) // key is up
            {
                if (string.Compare(keyState, "UP") != 0)
                    broadcastKeyToGameWindows(keyCode, WM_KEYUP);
            }
            else // send the key
            {
                keyIsCurrentlyPressed = true;
                if (string.Compare(keyState, "DOWN") != 0)
                {
                    broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
                }
            }
            return keyIsCurrentlyPressed;
        }


        #endregion KeyBroadcasting





        // ************************************************************************
        //                      Key Broadcasting Configuration
        // ************************************************************************
        #region Key Broadcasting Configuration

        /*
         * This event copies the keys the user wishes to have broadcast to the game clients, from the selection ListBox to the 
         * Keys To Be Broadcast ListBox.
         */
        private void btnCopyKeysOver_Click(object sender, EventArgs e)
        {
            // clear current contents of the Keys To Broadcast ListBox
            lbKeysToBeBroadcast.Items.Clear();

            // add currently selected key to the Keys To Broadcast ListBox
            foreach (string lbItem in lbListOfKeys.SelectedItems)
            {
                lbKeysToBeBroadcast.Items.Add(lbItem);
                // set corresponding flag to show the key is to be broadcast
                switch (lbItem)
                {
                    // keys 0 to 9
                    case "0":
                        zeroKeyBroadcast = true;
                        break;
                    case "1":
                        oneKeyBroadcast = true;
                        break;
                    case "2":
                        twoKeyBroadcast = true;
                        break;
                    case "3":
                        threeKeyBroadcast = true;
                        break;
                    case "4":
                        fourKeyBroadcast = true;
                        break;
                    case "5":
                        fiveKeyBroadcast = true;
                        break;
                    case "6":
                        sixKeyBroadcast = true;
                        break;
                    case "7":
                        sevenKeyBroadcast = true;
                        break;
                    case "8":
                        eightKeyBroadcast = true;
                        break;
                    case "9":
                        nineKeyBroadcast = true;
                        break;

                    // keys a to z
                    case "a":
                        aKeyBroadcast = true;
                        break;
                    case "b":
                        bKeyBroadcast = true;
                        break;
                    case "c":
                        cKeyBroadcast = true;
                        break;
                    case "d":
                        dKeyBroadcast = true;
                        break;
                    case "e":
                        eKeyBroadcast = true;
                        break;
                    case "f":
                        fKeyBroadcast = true;
                        break;
                    case "g":
                        gKeyBroadcast = true;
                        break;
                    case "h":
                        hKeyBroadcast = true;
                        break;
                    case "i":
                        iKeyBroadcast = true;
                        break;
                    case "j":
                        jKeyBroadcast = true;
                        break;
                    case "k":
                        kKeyBroadcast = true;
                        break;
                    case "l":
                        lKeyBroadcast = true;
                        break;
                    case "m":
                        mKeyBroadcast = true;
                        break;
                    case "n":
                        nKeyBroadcast = true;
                        break;
                    case "o":
                        oKeyBroadcast = true;
                        break;
                    case "p":
                        pKeyBroadcast = true;
                        break;
                    case "q":
                        qKeyBroadcast = true;
                        break;
                    case "r":
                        rKeyBroadcast = true;
                        break;
                    case "s":
                        sKeyBroadcast = true;
                        break;
                    case "t":
                        tKeyBroadcast = true;
                        break;
                    case "u":
                        uKeyBroadcast = true;
                        break;
                    case "v":
                        vKeyBroadcast = true;
                        break;
                    case "w":
                        wKeyBroadcast = true;
                        break;
                    case "x":
                        wKeyBroadcast = true;
                        break;
                    case "y":
                        wKeyBroadcast = true;
                        break;
                    case "z":
                        wKeyBroadcast = true;
                        break;

                    // keys F1 to F12
                    case "F1":
                        F1KeyBroadcast = true;
                        break;
                    case "F2":
                        F2KeyBroadcast = true;
                        break;
                    case "F3":
                        F3KeyBroadcast = true;
                        break;
                    case "F4":
                        F4KeyBroadcast = true;
                        break;
                    case "F5":
                        F5KeyBroadcast = true;
                        break;
                    case "F6":
                        F6KeyBroadcast = true;
                        break;
                    case "F7":
                        F7KeyBroadcast = true;
                        break;
                    case "F8":
                        F8KeyBroadcast = true;
                        break;
                    case "F9":
                        F9KeyBroadcast = true;
                        break;
                    case "F10":
                        F10KeyBroadcast = true;
                        break;
                    case "F11":
                        F11KeyBroadcast = true;
                        break;
                    case "F12":
                        F12KeyBroadcast = true;
                        break;

                    // keys Numpad 0 to 9
                    case "Numpad0":
                        numpad0KeyBroadcast = true;
                        break;
                    case "Numpad1":
                        numpad1KeyBroadcast = true;
                        break;
                    case "Numpad2":
                        numpad2KeyBroadcast = true;
                        break;
                    case "Numpad3":
                        numpad3KeyBroadcast = true;
                        break;
                    case "Numpad4":
                        numpad4KeyBroadcast = true;
                        break;
                    case "Numpad5":
                        numpad5KeyBroadcast = true;
                        break;
                    case "Numpad6":
                        numpad6KeyBroadcast = true;
                        break;
                    case "Numpad7":
                        numpad7KeyBroadcast = true;
                        break;
                    case "Numpad8":
                        numpad8KeyBroadcast = true;
                        break;
                    case "Numpad9":
                        numpad9KeyBroadcast = true;
                        break;                            
                    
                    // keys Symbols - + . / * etc
                    case "Multiply":
                        multiplyKeyBroadcast = true;
                        break;
                    case "Add":
                        addKeyBroadcast = true;
                        break;
                    case "Separator":
                        separatorKeyBroadcast = true;
                        break;
                    case "Subtract":
                        subtractKeyBroadcast = true;
                        break;
                    case "Decimal":
                        decimalKeyBroadcast = true;
                        break;
                    case "Divide":
                        divideKeyBroadcast = true;
                        break;

                    // keys Other - (SPACEBAR, ESC, INSERT, Arrow Keys, NUMLOCK, etc)
                    case "Backspace":
                        backspaceKeyBroadcast = true;
                        break;
                    case "Tab":
                        tabKeyBroadcast = true;
                        break;
                    case "Return":
                        returnKeyBroadcast = true;
                        break;
                    case "Shift":
                        shiftKeyBroadcast = true;
                        break;
                    case "Pause":
                        pauseKeyBroadcast = true;
                        break;
                    case "Capslock":
                        capsLockKeyBroadcast = true;
                        break;
                    case "SPACEBAR":
                        spacebarKeyBroadcast = true;
                        break;
                    case "ESCAPE":
                        escapeKeyBroadcast = true;
                        break;
                    case "PageUp":
                        pageUpKeyBroadcast = true;
                        break;
                    case "PageDown":
                        pageDownKeyBroadcast = true;
                        break;
                    case "End":
                        endKeyBroadcast = true;
                        break;
                    case "Home":
                        homeKeyBroadcast = true;
                        break;
                    case "LeftArrowKey":
                        leftArrowKeyBroadcast = true;
                        break;
                    case "UpArrowKey":
                        upArrowKeyBroadcast = true;
                        break;
                    case "RightArrowKey":
                        rightArrowKeyBroadcast = true;
                        break;
                    case "DownArrowKey":
                        downArrowKeyBroadcast = true;
                        break;
                    case "PrintScreen":
                        printScreenKeyBroadcast = true;
                        break;
                    case "Insert":
                        insertKeyBroadcast = true;
                        break;
                    case "Delete":
                        deleteKeyBroadcast = true;
                        break;
                    case "Help":
                        helpKeyBroadcast = true;
                        break;
                    case "NumLock":
                        numLockKeyBroadcast = true;
                        break;
                    case "ScrollLock":
                        scrollLockKeyBroadcast = true;
                        break;
                }
            }
        }

        /*
         * This event removes keys from the Keys To Be Broadcast ListBox.
         * If no keys are selected, it will result in all keys eing removed!
         */
        private void btnRemoveKeys_Click(object sender, EventArgs e)
        {
            if (lbKeysToBeBroadcast.SelectedItems.Count == 0)
            {
                // no keys selected, remove all!
                lbKeysToBeBroadcast.Items.Clear();
            }
            else
            {
                int totalKeyToRemove = lbKeysToBeBroadcast.SelectedItems.Count;
                for (int i = 0; i < totalKeyToRemove; i++)
                {
                    // remove a key from the Keys To Broadcast ListBox
                    foreach (string lbItem in lbKeysToBeBroadcast.SelectedItems)
                    {
                        lbKeysToBeBroadcast.Items.Remove(lbItem);
                        // set corresponding flag to show the key is NOT to be broadcast
                        switch (lbItem)
                        {
                            // keys 0 to 9
                            case "0":
                                zeroKeyBroadcast = false;
                                break;
                            case "1":
                                oneKeyBroadcast = false;
                                break;
                            case "2":
                                twoKeyBroadcast = false;
                                break;
                            case "3":
                                threeKeyBroadcast = false;
                                break;
                            case "4":
                                fourKeyBroadcast = false;
                                break;
                            case "5":
                                fiveKeyBroadcast = false;
                                break;
                            case "6":
                                sixKeyBroadcast = false;
                                break;
                            case "7":
                                sevenKeyBroadcast = false;
                                break;
                            case "8":
                                eightKeyBroadcast = false;
                                break;
                            case "9":
                                nineKeyBroadcast = false;
                                break;
                            // keys a to z
                            case "a":
                                aKeyBroadcast = false;
                                break;
                            case "b":
                                bKeyBroadcast = false;
                                break;
                            case "c":
                                cKeyBroadcast = false;
                                break;
                            case "d":
                                dKeyBroadcast = false;
                                break;
                            case "e":
                                eKeyBroadcast = false;
                                break;
                            case "f":
                                fKeyBroadcast = false;
                                break;
                            case "g":
                                gKeyBroadcast = false;
                                break;
                            case "h":
                                hKeyBroadcast = false;
                                break;
                            case "i":
                                iKeyBroadcast = false;
                                break;
                            case "j":
                                jKeyBroadcast = false;
                                break;
                            case "k":
                                kKeyBroadcast = false;
                                break;
                            case "l":
                                lKeyBroadcast = false;
                                break;
                            case "m":
                                mKeyBroadcast = false;
                                break;
                            case "n":
                                nKeyBroadcast = false;
                                break;
                            case "o":
                                oKeyBroadcast = false;
                                break;
                            case "p":
                                pKeyBroadcast = false;
                                break;
                            case "q":
                                qKeyBroadcast = false;
                                break;
                            case "r":
                                rKeyBroadcast = false;
                                break;
                            case "s":
                                sKeyBroadcast = false;
                                break;
                            case "t":
                                tKeyBroadcast = false;
                                break;
                            case "u":
                                uKeyBroadcast = false;
                                break;
                            case "v":
                                vKeyBroadcast = false;
                                break;
                            case "w":
                                wKeyBroadcast = false;
                                break;
                            case "x":
                                wKeyBroadcast = false;
                                break;
                            case "y":
                                wKeyBroadcast = false;
                                break;
                            case "z":
                                wKeyBroadcast = false;
                                break;
                            // keys F1 to F12
                            case "F1":
                                F1KeyBroadcast = false;
                                break;
                            case "F2":
                                F2KeyBroadcast = false;
                                break;
                            case "F3":
                                F3KeyBroadcast = false;
                                break;
                            case "F4":
                                F4KeyBroadcast = false;
                                break;
                            case "F5":
                                F5KeyBroadcast = false;
                                break;
                            case "F6":
                                F6KeyBroadcast = false;
                                break;
                            case "F7":
                                F7KeyBroadcast = false;
                                break;
                            case "F8":
                                F8KeyBroadcast = false;
                                break;
                            case "F9":
                                F9KeyBroadcast = false;
                                break;
                            case "F10":
                                F10KeyBroadcast = false;
                                break;
                            case "F11":
                                F11KeyBroadcast = false;
                                break;
                            case "F12":
                                F12KeyBroadcast = false;
                                break;
                            // keys Numpad 0 to 9
                            case "Numpad0":
                                numpad0KeyBroadcast = false;
                                break;
                            case "Numpad1":
                                numpad1KeyBroadcast = false;
                                break;
                            case "Numpad2":
                                numpad2KeyBroadcast = false;
                                break;
                            case "Numpad3":
                                numpad3KeyBroadcast = false;
                                break;
                            case "Numpad4":
                                numpad4KeyBroadcast = false;
                                break;
                            case "Numpad5":
                                numpad5KeyBroadcast = false;
                                break;
                            case "Numpad6":
                                numpad6KeyBroadcast = false;
                                break;
                            case "Numpad7":
                                numpad7KeyBroadcast = false;
                                break;
                            case "Numpad8":
                                numpad8KeyBroadcast = false;
                                break;
                            case "Numpad9":
                                numpad9KeyBroadcast = false;
                                break;

                            // keys Symbols - + . / * etc
                            case "Multiply":
                                multiplyKeyBroadcast = false;
                                break;
                            case "Add":
                                addKeyBroadcast = false;
                                break;
                            case "Separator":
                                separatorKeyBroadcast = false;
                                break;
                            case "Subtract":
                                subtractKeyBroadcast = false;
                                break;
                            case "Decimal":
                                decimalKeyBroadcast = false;
                                break;
                            case "Divide":
                                divideKeyBroadcast = false;
                                break;

                            // keys Other - (SPACEBAR, ESC, INSERT, Arrow Keys, NUMLOCK, etc)
                            case "Backspace":
                                backspaceKeyBroadcast = false;
                                break;
                            case "Tab":
                                tabKeyBroadcast = false;
                                break;
                            case "Return":
                                returnKeyBroadcast = false;
                                break;
                            case "Shift":
                                shiftKeyBroadcast = false;
                                break;
                            case "Pause":
                                pauseKeyBroadcast = false;
                                break;
                            case "Capslock":
                                capsLockKeyBroadcast = false;
                                break;
                            case "SPACEBAR":
                                spacebarKeyBroadcast = false;
                                break;
                            case "ESCAPE":
                                escapeKeyBroadcast = false;
                                break;
                            case "PageUp":
                                pageUpKeyBroadcast = false;
                                break;
                            case "PageDown":
                                pageDownKeyBroadcast = false;
                                break;
                            case "End":
                                endKeyBroadcast = false;
                                break;
                            case "Home":
                                homeKeyBroadcast = false;
                                break;
                            case "LeftArrowKey":
                                leftArrowKeyBroadcast = false;
                                break;
                            case "UpArrowKey":
                                upArrowKeyBroadcast = false;
                                break;
                            case "RightArrowKey":
                                rightArrowKeyBroadcast = false;
                                break;
                            case "DownArrowKey":
                                downArrowKeyBroadcast = false;
                                break;
                            case "PrintScreen":
                                printScreenKeyBroadcast = false;
                                break;
                            case "Insert":
                                insertKeyBroadcast = false;
                                break;
                            case "Delete":
                                deleteKeyBroadcast = false;
                                break;
                            case "Help":
                                helpKeyBroadcast = false;
                                break;
                            case "NumLock":
                                numLockKeyBroadcast = false;
                                break;
                            case "ScrollLock":
                                scrollLockKeyBroadcast = false;
                                break;

                        }
                        break; // we have to break out here, or otherwise it will try an work on a modified list, which it won't like
                    }
                }
            }
        }

        private bool keyConfigAreaOpen = false;
        /*
         * This event opens and closes the key configuration area
         */
        private void btnKeyConfig_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            if (!keyConfigAreaOpen) // open the key config area
            {
                this.Width = 1315;
                btnKeyConfig.Text = "< Close";
                keyConfigAreaOpen = true;
            }
            else // close the key config area
            {
                this.Width = 941;
                btnKeyConfig.Text = "Key Config >";
                keyConfigAreaOpen = false;
            }
        }

        /*
         * This event kicks off the saving of the Key Configuration file
         * default name: keyConfigFile
         */
        private void btnSaveKeyConfigFile_Click(object sender, EventArgs e)
        {
            string fileName = "";
            if (String.Compare(txtKeyConfigFileName.Text, "") == 0 || String.Compare(txtKeyConfigFileName.Text, "Key config file name") == 0)
            {
                fileName = "keyConfigFile";
            }
            else
                fileName = txtKeyConfigFileName.Text;

            string [] listOfKeys = new string[100];
            // need to store the keys into an array
            int count = 0;
            foreach (string lbItem in lbKeysToBeBroadcast.Items)
            {
                listOfKeys[count] = lbItem;
                count++;
            }
            FileHandling.saveToKeyConfigFile(fileName, listOfKeys, count);
        }

        /*
         * This event kicks off the loading of the Key Configuration file
         * default name: keyConfigFile
         */
        private void btnLoadKeyConfigFile_Click(object sender, EventArgs e)
        {
            string fileName = "";
            if (String.Compare(txtKeyConfigFileName.Text, "") == 0 || String.Compare(txtKeyConfigFileName.Text, "Key config file name") == 0)
            {
                fileName = "keyConfigFile";
            }
            else
                fileName = txtKeyConfigFileName.Text;

            // load the config file data
            string[] listOfKeys = new string[100];
            listOfKeys = FileHandling.loadKeyConfigFile(fileName);

            // store skey into the Keys To Be Broadcast ListBox
            int count = 0;
            while (listOfKeys[count] != null)
            {
                lbKeysToBeBroadcast.Items.Add(listOfKeys[count]);
                count++;
            }
        }


        #endregion Key Broadcasting Configuration
        

        // ********************************************************************************************************************************
        //                                              User Defined Key Configuration
        // ********************************************************************************************************************************
        #region User Defined Key Configuration

        private bool keyCombinationDoesNotExist()
        {
            string keyCombination = keyModifiers + targetKey;
            // see if the current key combination in the TextBox exists in the Keys to be Broadcast list
            int totalKeyToRemove = lbKeysToBeBroadcast.SelectedItems.Count;
            for (int i = 0; i < totalKeyToRemove; i++)
            {
                // remove a key from the Keys To Broadcast ListBox
                foreach (string lbItem in lbKeysToBeBroadcast.SelectedItems)
                {
                    lbKeysToBeBroadcast.Items.Remove(lbItem);
                    // is this key the same?
                    if (string.Compare(keyCombination, lbItem) == 0)
                    {
                        return false; // key found, so it does exist!
                    }
                }
            }


            return true;
        }



        KeysToConvert listOfKeysToConvert = new KeysToConvert();
        ModifiableKeys listOfModifiableKeys = new ModifiableKeys();
        string keyModifiers = "";
        string targetKey = "";
        bool targetKeyAdded = false;
        bool CTRLKeyAdded = false;
        bool SHIFTKeyAdded = false;
        bool ALTKeyAdded = false;
        bool keepKeyCombination = false;
        private void txtAddKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
    //        MessageBox.Show("Key: " + e.KeyData);


            string[] keyBeingProcessed = new string[3]; // only allow a total key combination of 3 - e.g. CTRL+ALT+P
            keyBeingProcessed = e.KeyData.ToString().Split(',');

            // convert keys that need it - 0 - 9, a to z
            string keyConversion = listOfKeysToConvert.convertKey(keyBeingProcessed[0]);
            if (keyConversion != "")
            {
                keyBeingProcessed[0] = keyConversion;
            }

            if (e.Control && !CTRLKeyAdded)
            {
                txtAddKey.Text = "";
                keyModifiers = "CONTROL+" + keyModifiers;
                CTRLKeyAdded = true;
            }
            else if (e.Shift && !SHIFTKeyAdded)
            {
                txtAddKey.Text = "";
                keyModifiers += "SHIFT+";
                SHIFTKeyAdded = true;
            }
            else if (e.Alt && !ALTKeyAdded)
            {
                txtAddKey.Text = "";
                keyModifiers += "ALT+";
                ALTKeyAdded = true;
            }
            else if (!targetKeyAdded && ((keyModifiers != "" && listOfModifiableKeys.isModifiableKey(keyBeingProcessed[0]) || keyModifiers == ""))) // && CTRLKeyAdded && SHIFTKeyAdded && ALTKeyAdded
            {
                if (keyBeingProcessed[0] != "ControlKey" && keyBeingProcessed[0] != "Control" && keyBeingProcessed[0] != "Shift" && keyBeingProcessed[0] != "ShiftKey" && keyBeingProcessed[0] != "Menu")
                {
                    txtAddKey.Text = "";
                    targetKey = keyBeingProcessed[0];
                    targetKeyAdded = true;
                    // check if this key combination exists already in the Keys to be Broadcast list
                    // if not, then set this key combination to not be deleated by any key release situation
                    // - user will have to press the delete button to remove the key combination.
                    if (keyCombinationDoesNotExist())
                    {
                        keepKeyCombination = true;
                        // enable the Addkey button, as we can post this key combination to the Keys to be Broadcast, if we want to
                        btnAddKey.Enabled = true;
                    }
                    else
                        keepKeyCombination = false;
                }
            }

            txtAddKey.Text = keyModifiers + targetKey;

        }

        private void txtAddKey_KeyUp(object sender, KeyEventArgs e)
        {
            // only execute this if the current key combination is not fied in place 
            string[] keyBeingProcessed = new string[3]; // only akllow a total key combination of 3 - e.g. CTRL+ALT+P
            keyBeingProcessed = e.KeyData.ToString().Split(',');


            // remove CTRL+ if key up
            if (keyBeingProcessed[0] == "ControlKey" && !keepKeyCombination)
            {
                keyModifiers = keyModifiers.Replace("CONTROL+", "");
                CTRLKeyAdded = false;
            }
            else if (keyBeingProcessed[0] == "ShiftKey" && !keepKeyCombination)
            {
                keyModifiers = keyModifiers.Replace("SHIFT+", "");
                SHIFTKeyAdded = false;
            }
            else if (keyBeingProcessed[0] == "Menu" && !keepKeyCombination)
            {
                keyModifiers = keyModifiers.Replace("ALT+", "");
                ALTKeyAdded = false;
                e.Handled = true;
            }
            else if (targetKeyAdded && !keepKeyCombination)
            {
                targetKey = targetKey.Replace(targetKey, "");
                targetKeyAdded = false;
            }

            txtAddKey.Text = keyModifiers + targetKey;

        }

        /*
         * This method clears the key combination that is currently in the input box.
         * Used if the user decides not to use that key combination.
         */
        private void btnDeleteKeyCombination_Click(object sender, EventArgs e)
        {
            keepKeyCombination = false;
            targetKeyAdded = false;
            CTRLKeyAdded = false;
            SHIFTKeyAdded = false;
            ALTKeyAdded = false;
            keyModifiers = "";
            targetKey = "";
            txtAddKey.Text = "";
        }

        /*
         * This event adds the key combination currently in the TextBox to the Keys to be Broadcast list
         */
        private void btnAddKey_Click(object sender, EventArgs e)
        {
            // add the key combination to the list (ComboBox)
            lbKeysToBeBroadcast.Items.Add(txtAddKey.Text);
            // disable this button
            btnAddKey.Enabled = false;
            // clear the input box
            txtAddKey.Text = "";
        }


        #endregion User Defined Key Configuration







        // ********************************************************************************************************************************
        //                                                      New Broadcast Key
        // ********************************************************************************************************************************
        #region New Broadcast Key 
        
        
        


        /*
         * GetAsyncKeyState - is the key pressed or released?
         * This method deals with key that are to be broadcast the the game windows prior to actually sending them.
         * This method reults in reducing the amount of code required in the OnTimedEvent mthod.
         */
        private bool checkStateOfKey(int keyCode, string keyToBroadcast)
        {
            bool keyIsCurrentlyPressed = false;
            // get the key's state (UP/DOWN?)
            if (keyUpDownState.ContainsKey(keyToBroadcast))
            {
     //           MessageBox.Show("Processing target key!");
                if (GetAsyncKeyState(keyCode) == 0) // key is up
                {
                    if (string.Compare(keyUpDownState[keyToBroadcast], "UP") != 0)
                    {
     //                   MessageBox.Show("Target key: " + keyToBroadcast + " is UP - key code: " + keyCode);
                        broadcastKeyToGameWindows(keyCode, WM_KEYUP);
                    }
                }
                else // send the key
                {
                    keyIsCurrentlyPressed = true;
                    if (string.Compare(keyUpDownState[keyToBroadcast], "DOWN") != 0)
                    {
                        broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
                    }
                }
            }

            return keyIsCurrentlyPressed;
        }


        /*
         * GetAsyncKeyState - is the key pressed or released?
         * This method deals with a modifer key that is to be broadcast the the game windows prior to actually sending them.
         * This method reults in reducing the amount of code required in the OnTimedEvent mthod.
         */
        private bool checkStateOfModifierKey(int keyCode, string keyToBroadcast)
        {
            bool keyIsCurrentlyPressed = false;
            if (GetAsyncKeyState(keyCode) == 0) // key is up
            {
                if (string.Compare(listOfModifierKeyStates.getModifierKeyState(keyToBroadcast), "UP") != 0)
                {
                    broadcastKeyToGameWindows(keyCode, WM_KEYUP);
                }
            }
            else // send the key
            {
                keyIsCurrentlyPressed = true;
                if (string.Compare(listOfModifierKeyStates.getModifierKeyState(keyToBroadcast), "DOWN") != 0)
                {
                    broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
                }
            }

            return keyIsCurrentlyPressed;
        }


        /*
         * http://msdn.microsoft.com/en-gb/library/windows/desktop/ms646293(v=vs.85).aspx
         * http://stackoverflow.com/questions/10790502/c-getkeystate-has-to-run-once
         */
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            bool keyIsCurrentlyPressed = false;

            // iterate through all of the keys in the Keys to be Broadcast list

            string[] keyCombination;
            int totalKeysInList = lbKeysToBeBroadcast.Items.Count;
            foreach (string item in lbKeysToBeBroadcast.Items)
            {  
                // split the contents of item
                // then check each part :-)
                keyCombination = item.Split('+');
                for (int i = 0; i < keyCombination.Length; i++)
                {
                    // get the keycode

                    if (keyCombination[i] == "CONTROL" || keyCombination[i] == "SHIFT" || keyCombination[i] == "ALT")
                    {
                        // process modifier key
                        if (checkStateOfModifierKey(VK_CONTROL, "CONTROL"))
                            keyIsCurrentlyPressed = true;
                        if (checkStateOfModifierKey(VK_SHIFT, "SHIFT"))
                            keyIsCurrentlyPressed = true;
                        if (checkStateOfModifierKey(VK_MENU, "ALT"))
                            keyIsCurrentlyPressed = true;
                    }
                    else // process target key
                    {
                        int keyCode = Convert.ToInt32(listOfKeyValues.getKeyCode(keyCombination[i]), 16);
                        if (checkStateOfKey(keyCode, keyCombination[i]))
                            keyIsCurrentlyPressed = true;
                    }

                }
            }

            if (!keyIsCurrentlyPressed)
            {
                initialiseKeyFlags();
                keyBroadcastTimer.Stop();
            }
        }

        /*
         * This method checks to see if the key passed to it is in the Keys to be broadcast list
         */
        private bool isBroadcastableKey(string keyToSearchFor)
        {
            int totalKeysInList = lbKeysToBeBroadcast.Items.Count;
            foreach (string item in lbKeysToBeBroadcast.Items)
            {
                if (keyToSearchFor == item)
                    return true; // key is one of the ones to be broadcast
            }

            return false; // key is NOT one of the ones to be broadcast 
        }

        /*
         * This method checks to see if the key combination passed to it is in the Keys to be broadcast list
         * CONTROL+ALT+key, SHIFT+ALT+key, etc.
         */
        private bool isBroadcastableKey(string modifierKey1, string modifierKey2, string targetKey)
        {
            string[] itemContents;
            int totalKeysInList = lbKeysToBeBroadcast.Items.Count;
            foreach (string item in lbKeysToBeBroadcast.Items)
            {
                itemContents = item.Split('+');
                // need to check for modifier hesy, so must splt the contents of item
                if (itemContents.Length == 3) // we only want to check key combination of three keys (2x modifiers and 1x target)
                {
                    if (modifierKey1 == itemContents[0] && modifierKey2 == itemContents[1] && targetKey == itemContents[2])
                    {
                        return true; // key is one of the ones to be broadcast
                    }
                }
            }

            return false; // key is NOT one of the ones to be broadcast 
        }

        /*
         * This method checks to see if the key combination passed to it is in the Keys to be broadcast list
         * CONTROL+key, SHIFT+key, ALT+key
         */
        private bool isBroadcastableKey(string modifierKey, string targetKey)
        {
            string [] itemContents;
            int totalKeysInList = lbKeysToBeBroadcast.Items.Count;
            foreach (string item in lbKeysToBeBroadcast.Items)
            {
                itemContents = item.Split('+');
                // need to check for modifier hesy, so must splt the contents of item
                if (itemContents.Length == 2) // we only want to check key combination of two keys
                {
                    if (modifierKey == itemContents[0] && targetKey == itemContents[1])
                    {
                        return true; // key is one of the ones to be broadcast
                    }
                }
            }

            return false; // key is NOT one of the ones to be broadcast 
        }

        /*
         * This method checks to see if the key passed to it is in the Keys to be broadcast list
         */
        public bool isBroadcastableKey(int keyCodeToSearchFor)
        {
            string keyToSearchFor = "";
            // get the code via the name
            keyToSearchFor = listOfKeyCodes.getKeyValue(keyCodeToSearchFor.ToString());

            int totalKeysInList = lbKeysToBeBroadcast.Items.Count;
            foreach (string item in lbKeysToBeBroadcast.Items)
            {
                if (keyToSearchFor == item)
                    return true; // key is one of the ones to be broadcast
            }

            return false; // key is NOT one of the ones to be broadcast 
        }
       
        /*
         * This method sends the key to all the game clients
         * keyCode: w=57
         * keyStateCode: WM_KEYDOWN, WM_KEYUP
         */
        private void broadcastKeyToGameWindows(int keyCode, uint keyStateCode)
        {
            if (bBroadcastKey) // only broadcast keys when we want to 
            {
                // iterate through all of the screens
                for (int screenIdx = 0; screenIdx < totalNumberOfScreens; screenIdx++)
                {
                    int totalGameWindows = myScreens[screenIdx].getTotalNumberOfGameWindows();
                    // iterate through all of the game windows of the current screen
                    for (int gameWindowIdx = 0; gameWindowIdx < totalGameWindows; gameWindowIdx++)
                    {
                        // only broadcast key if the game window is not the primary game window (the one the player/user is playing on)
                        if (!myScreens[screenIdx].isThePrimaryGameClient(gameWindowIdx))
                        {
                            PostMessage(myScreens[screenIdx].getProcess(gameWindowIdx).MainWindowHandle, keyStateCode, keyCode, 0);
                        }
                    }
                }
            }
        }        

        /*
         * This method sets the flag associated with the key passed to it, to "DOWN"
         * This signlas that the key is pressed down
         */
        private void setTargetKeyFlagState(string keyToBroadcast)
        {
            // locate keyCode
            if (keyUpDownState.ContainsKey(keyToBroadcast))
            {
                keyUpDownState[keyToBroadcast] = "DOWN";
            }
        }

        /*
         * This will be used to process single key entries, such as the "w" key to move the toon forwards
         */
        public void sendModifierKeysAndTargetKey(int keyCode, string modifierKeyName1, string modifierKeyName2)
        {
            // get the keyCode value, e.g. 65 = "a"
            string hexValue = keyCode.ToString("X");
            keyToBroadcast = listOfKeyCodes.getKeyValue(hexValue);

            // check if the key combination is in the Keys to be Broadcast list

            if (isBroadcastableKey(modifierKeyName1, modifierKeyName2, keyToBroadcast)) // the key is one of the ones we wish to process/broadcast
            {
                // make sure the timer is not already running
                if (keyBroadcastTimer.Enabled)
                    keyBroadcastTimer.Stop();
                keyBroadcastTimer.Start();
                // set the MODIFER keys state to DOWN
                listOfModifierKeyStates.setModifierKeyState(modifierKeyName1, "DOWN");
                broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
                listOfModifierKeyStates.setModifierKeyState(modifierKeyName2, "DOWN");
                switch (modifierKeyName1)
                {
                    case "CONTROL":
                        broadcastKeyToGameWindows(VK_CONTROL, WM_KEYDOWN);
                        break;
                    case "ALT":
                        broadcastKeyToGameWindows(VK_MENU, WM_KEYDOWN);
                        break;
                    default: // SHIFT
                        broadcastKeyToGameWindows(VK_SHIFT, WM_KEYDOWN);
                        break;
                }
                switch (modifierKeyName2)
                {
                    case "CONTROL":
                        broadcastKeyToGameWindows(VK_CONTROL, WM_KEYDOWN);
                        break;
                    case "ALT":
                        broadcastKeyToGameWindows(VK_MENU, WM_KEYDOWN);
                        break;
                    default: // SHIFT
                        broadcastKeyToGameWindows(VK_SHIFT, WM_KEYDOWN);
                        break;
                }

                broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
                // set the targetkey state to DOWN
                setTargetKeyFlagState(keyToBroadcast); // find out what the flag is that goes with this key, then set it to "DOWN" (pressed)
                broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
            }
        }

        /*
         * This will be used to process single key entries, such as the "w" key to move the toon forwards
         */
        public void sendModifierKeyAndTargetKey(int keyCode, string modifierKeyName)
        {
            // get the keyCode value, e.g. 65 = "a"
            string hexValue = keyCode.ToString("X");
            keyToBroadcast = listOfKeyCodes.getKeyValue(hexValue);

            // check if the key combination is in the Keys to be Broadcast list

            if (isBroadcastableKey(modifierKeyName, keyToBroadcast)) // the key is one of the ones we wish to process/broadcast
            {
                // make sure the timer is not already running
                if (keyBroadcastTimer.Enabled)
                    keyBroadcastTimer.Stop();
                keyBroadcastTimer.Start();
                // set the CONTROL key state to DOWN
                listOfModifierKeyStates.setModifierKeyState(modifierKeyName, "DOWN");
                switch (modifierKeyName)
                {
                    case "CONTROL":
                        broadcastKeyToGameWindows(VK_CONTROL, WM_KEYDOWN);
                        break;
                    case "ALT":
                        broadcastKeyToGameWindows(VK_MENU, WM_KEYDOWN);
                        break;
                    default:
                        broadcastKeyToGameWindows(VK_SHIFT, WM_KEYDOWN);
                        break;
                }

                // set the targetkey state to DOWN
                setTargetKeyFlagState(keyToBroadcast); // find out what the flag is that goes with this key, then set it to "DOWN" (pressed)
                broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
            }
        }

        int currentKeyCode = 0;
        string keyToBroadcast = "";
        /*
         * This will be used to process single key entries, such as the "w" key to move the toon forwards
         */
        public void sendTargetKey(int keyCode)
        {
            currentKeyCode = keyCode; // currentGameWindowKeyCode used globally

            // get the keyCode value, e.g. 65 = "a"
            string hexValue = keyCode.ToString("X");
            keyToBroadcast = listOfKeyCodes.getKeyValue(hexValue);
            // check if the key is in the Keys to be Broadcast list

            if (isBroadcastableKey(keyToBroadcast)) // the key is one of the ones we wish to process/broadcast
            {
                // make sure the timer is not already running
                if (keyBroadcastTimer.Enabled)
                    keyBroadcastTimer.Stop();
                keyBroadcastTimer.Start();
                setTargetKeyFlagState(keyToBroadcast); // find out what the flag is that goes with this key, then set it to "DOWN" (pressed)
                broadcastKeyToGameWindows(keyCode, WM_KEYDOWN);
            }

            // ELSE do nothing with that key, as it is not one of the ones we wish to broadcast - not in the Keys to be Broadcast list
        }




        #endregion New Broadcast Key 




        /*
         * This event will set the currently selected game client to be the primary game client, that being the one
         * the user is playing on.
         * This is done to stop the multiboxer broadcasting keys to the game client the player is playing on.
         */
        private void btnSetPrimaryClient_Click(object sender, EventArgs e)
        {
            // iterate through the game clients

            myScreens[currentScreen].setPrimaryGameClientState(currentSelectedGameWindow, true);
            myScreens[primaryGameWindowScreenIdx].setPrimaryGameClientState(primaryGameWindowIdx, false);
            primaryGameWindowIdx = currentSelectedGameWindow;
            primaryGameWindowScreenIdx = currentScreen;
        }

        private void btnKeyBroadcasting_Click(object sender, EventArgs e)
        {
            if (bBroadcastKey)
            {
                btnKeyBroadcasting.Text = "DISABLED";
                bBroadcastKey = false;
            }
            else
            {
                btnKeyBroadcasting.Text = "ENABLED";
                bBroadcastKey = true;
            }
        }




    }
}

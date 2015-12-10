using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; // for file handling
using System.Windows.Forms;
using System.Drawing;

namespace BoxerTasticMate
{
    class FileHandling
    {

        /*
         * This saves the current Keys to Broadcast to the key configuration file 
         */
        public static void saveToKeyConfigFile(string fileName, string [] listOfKeys, int totalNumberOfKeys)
        {
            try
            {
                // write total # of game windows on the screen
                FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using (StreamWriter configStateWriter = new StreamWriter(fs))
                {
                    // save the keys to the file
                    for (int i = 0; i < totalNumberOfKeys; i++)
                    {
                        configStateWriter.WriteLine(listOfKeys[i]);
                    }
                }
                fs.Close();
            }
            catch
            {
            }
        }

        /*
         * This loads the "Keys to Broadcast" from the spedified key configuration file 
         */
        public static string [] loadKeyConfigFile(string fileName)
        {
            string [] listOfKeys = new string[100];
            // make sure the file exists
            if (File.Exists(fileName))
            {
                // load total # of screens from file
                try
                {
                    FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using (StreamReader configStateReader = new StreamReader(fs))
                    {
                        // read till end of file
                        int count = 0;
                        while (configStateReader.Peek() >= 0)
                        {
                            listOfKeys[count] = configStateReader.ReadLine();
                            count++;
            //                MessageBox.Show("Key: " + listOfKeys[count]);
                        }
                    }
                    fs.Close();
                }
                catch
                {
                }
            }

            return listOfKeys;
        }





        public static void saveDataToFile(string fileName, List<MyScreen> accountAndClientInfo, int totalNumberOfScreens)
        {
            // save total # of screens to file
            try
            {
                FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                using (StreamWriter configStateWriter = new StreamWriter(fs))
                {
                    configStateWriter.WriteLine(totalNumberOfScreens);
                }
                fs.Close();
            }
            catch (Exception frack)
            {
                MessageBox.Show("" + frack);
            }
            // iterate through the screens
            for (int scrnIdx = 0; scrnIdx < totalNumberOfScreens; scrnIdx++)
            {
                int totalGameWindowsOnCurrentScreen = accountAndClientInfo[scrnIdx].getTotalNumberOfGameWindows();
                try
                {
                    // write total # of game windows on the screen
                    FileStream fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                    using (StreamWriter configStateWriter = new StreamWriter(fs))
                    {
                        configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].screenID); 
                        configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].screenName);
                        configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].screenHeight);
                        configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].screenWidth);
                        configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].screenXCoord);
                        configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].screenYCoord);
                        configStateWriter.WriteLine(totalGameWindowsOnCurrentScreen);
                    }
                    fs.Close();
                }
                catch
                {
                }

                // iterate through the game windows on the screen
                for (int gameWindowIdx = 0; gameWindowIdx < totalGameWindowsOnCurrentScreen; gameWindowIdx++)
                {
                    try
                    {
                        // write game window data to the config file
                        FileStream fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                        using (StreamWriter configStateWriter = new StreamWriter(fs))
                        {
                            configStateWriter.WriteLine(gameWindowIdx);
                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].getAccountName(gameWindowIdx));
                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].getAccountPassword(gameWindowIdx));
                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].getExePath(gameWindowIdx));

                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].xCoord[gameWindowIdx]);
                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].yCoord[gameWindowIdx]);
                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].height[gameWindowIdx]);
                            configStateWriter.WriteLine(accountAndClientInfo[scrnIdx].width[gameWindowIdx]);

                            string fullText = accountAndClientInfo[scrnIdx].colour[gameWindowIdx].ToString();
                            int idxOfLeftBracket = fullText.IndexOf("[");
                            int idxOfRightBracket = fullText.IndexOf("]");
                            int length = idxOfRightBracket - idxOfLeftBracket;
                            string colourName = fullText.Substring(idxOfLeftBracket + 1, length - 1);

                            configStateWriter.WriteLine(colourName);

                        }
                        fs.Close();
                    }
                    catch
                    {
                    }
                }

            }
        } // END OF saveDataToFile( ... )


        /*
         * This method loads the data held in the config file, and returns it to the calling method.
         * This data will replace the existing data - account info + screen layout
         */
        public static List<MyScreen> loadDataFromFile(string configFileName, List<MyScreen> myScreens)
        {
            int totalNumberOfScreens = 0;

            // make sure the file exists
            if (File.Exists(configFileName))
            {
                // load total # of screens from file
                try
                {
                    FileStream fs = File.Open(configFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    using (StreamReader configStateReader = new StreamReader(fs))
                    {
                        totalNumberOfScreens = Convert.ToInt32(configStateReader.ReadLine());
                        MessageBox.Show("Total # of screens: " + totalNumberOfScreens);
            
                        // iterate through the screens
                        for (int scrnIdx = 0; scrnIdx < totalNumberOfScreens; scrnIdx++)
                        {
                            myScreens[scrnIdx].screenID = Convert.ToInt32(configStateReader.ReadLine());
                            myScreens[scrnIdx].screenName = configStateReader.ReadLine();
                            myScreens[scrnIdx].screenHeight = Convert.ToInt32(configStateReader.ReadLine());
                            myScreens[scrnIdx].screenWidth = Convert.ToInt32(configStateReader.ReadLine());
                            myScreens[scrnIdx].screenXCoord = Convert.ToInt32(configStateReader.ReadLine());
                            myScreens[scrnIdx].screenYCoord = Convert.ToInt32(configStateReader.ReadLine());
                            myScreens[scrnIdx].setTotalNumberOfGameWindows(Convert.ToInt32(configStateReader.ReadLine()));

                            int totalGameWindowsOnCurrentScreen = myScreens[scrnIdx].getTotalNumberOfGameWindows();

                            // iterate through the game windows on the screen
                            for (int gameWindowIdx = 0; gameWindowIdx < totalGameWindowsOnCurrentScreen; gameWindowIdx++)
                            {
                                myScreens[scrnIdx].gameWindow[gameWindowIdx] = Convert.ToInt32(configStateReader.ReadLine());
                                myScreens[scrnIdx].setAccountName(configStateReader.ReadLine(), gameWindowIdx);
                                myScreens[scrnIdx].setAccountPassword(configStateReader.ReadLine(), gameWindowIdx);
                                myScreens[scrnIdx].setExePath(configStateReader.ReadLine(), gameWindowIdx);
                                myScreens[scrnIdx].xCoord[gameWindowIdx] = Convert.ToInt32(configStateReader.ReadLine());
                                myScreens[scrnIdx].yCoord[gameWindowIdx] = Convert.ToInt32(configStateReader.ReadLine());
                                myScreens[scrnIdx].height[gameWindowIdx] = Convert.ToInt32(configStateReader.ReadLine());
                                myScreens[scrnIdx].width[gameWindowIdx] = Convert.ToInt32(configStateReader.ReadLine());
                                myScreens[scrnIdx].colour[gameWindowIdx] = Color.FromName(configStateReader.ReadLine());

                            }


                        }


                    }
                    fs.Close();
                }
                catch (Exception frack)
                {
                    // problem whilst reading the file
                    MessageBox.Show("" + frack);
                }

            }
            
            return myScreens;
        } // END OF loadDataFromFile( ... )




        public static void addAccountInfoToFile_OLD(string fileName, List<MyScreen> accountAndClientInfo, int totalNumberOfScreens)
        {
            MessageBox.Show("Total # of screens " + totalNumberOfScreens);
            // make sure the file exists
            if (!File.Exists(fileName))
            {
                // save total # of screens to file
                try
                {
                    FileStream fs = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                    using (StreamWriter configStateWriter = new StreamWriter(fs))
                    {
                        configStateWriter.WriteLine("Total screens: " + totalNumberOfScreens);
                    }
                    fs.Close();
                }
                catch (Exception frack)
                {
                    MessageBox.Show("" + frack);
                }
                // iterate through the screens
                for (int scrnIdx=0; scrnIdx < totalNumberOfScreens; scrnIdx++)
                {
                    int totalGameWindowsOnCurrentScreen = accountAndClientInfo[scrnIdx].getTotalNumberOfGameWindows();
                    try
                    {
                        // write total # of game windows on the screen
                        FileStream fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                        using (StreamWriter configStateWriter = new StreamWriter(fs))
                        {
                            configStateWriter.WriteLine("Screen # " + scrnIdx.ToString());
                            configStateWriter.WriteLine("Screen ID: " + accountAndClientInfo[scrnIdx].screenID); ;
                            configStateWriter.WriteLine("Screen Name: " + accountAndClientInfo[scrnIdx].screenName);
                            configStateWriter.WriteLine("Screen Height: " + accountAndClientInfo[scrnIdx].screenHeight);
                            configStateWriter.WriteLine("Screen Width: " + accountAndClientInfo[scrnIdx].screenWidth);
                            configStateWriter.WriteLine("Screen X coord: " + accountAndClientInfo[scrnIdx].screenXCoord);
                            configStateWriter.WriteLine("Screen Y coord: " + accountAndClientInfo[scrnIdx].screenYCoord);
                            configStateWriter.WriteLine("Total game windows: " + totalGameWindowsOnCurrentScreen);
                        }
                        fs.Close();
                    }
                    catch
                    { 
                    }

                    // iterate through the game windows on the screen
                    for (int gameWindowIdx = 0; gameWindowIdx < totalGameWindowsOnCurrentScreen; gameWindowIdx++)
                    {
                        try
                        {
                            // write game window data to the config file
                            FileStream fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                            using (StreamWriter configStateWriter = new StreamWriter(fs))
                            {
                                configStateWriter.WriteLine(gameWindowIdx);
                                configStateWriter.WriteLine("Account name: " + accountAndClientInfo[scrnIdx].getAccountName(gameWindowIdx));
                                configStateWriter.WriteLine("Account password: " + accountAndClientInfo[scrnIdx].getAccountPassword(gameWindowIdx));
                                configStateWriter.WriteLine("Exe path: " + accountAndClientInfo[scrnIdx].getExePath(gameWindowIdx));

                                configStateWriter.WriteLine("Game window X coord: " + accountAndClientInfo[scrnIdx].xCoord[gameWindowIdx]);
                                configStateWriter.WriteLine("Game window Y coord: " + accountAndClientInfo[scrnIdx].yCoord[gameWindowIdx]);
                                configStateWriter.WriteLine("Game window Height: " + accountAndClientInfo[scrnIdx].height[gameWindowIdx]);
                                configStateWriter.WriteLine("Game window Width: " + accountAndClientInfo[scrnIdx].width[gameWindowIdx]);
                                configStateWriter.WriteLine("Game window colour: " + accountAndClientInfo[scrnIdx].colour[gameWindowIdx].ToString());
                            }
                            fs.Close();
                        }
                        catch
                        {
                        }
                    }

                }

            }
            else
            {
                MessageBox.Show("ERROR - file does not exist: " + fileName);
            }
        } // END OF 



        // adds more accounts to the config file
        public static void addToConfigFile(int currentPageNumber, string exePath, int totalNumberOfAccounts, string accountName, string password)
        {
            // FIRST PART
            // Transfer data in existing file updating the total number of accounts
            if (File.Exists("Config"))
            {
                try
                {
                    using (StreamReader configReader = new StreamReader("Config"))
                    {
                        // Make sure the Temp file does not exists
                        if (File.Exists("Temp"))
                        {
                            File.Delete("Temp");
                        }

                        // this will replace any existing file
                        using (StreamWriter configWriter = new StreamWriter("Temp"))
                        {

                            // read in the first line (path to the exe)
                            string path = configReader.ReadLine(); // make sure we skip passed this
                            configWriter.WriteLine(exePath); // add what is currently in the exe path's input box
                            // read in total # of accounts
                            totalNumberOfAccounts = Convert.ToInt32(configReader.ReadLine());
                            int accountTotal = totalNumberOfAccounts + 1;
                            configWriter.WriteLine(accountTotal.ToString());

                            string lineData;
                            // read in then write the account name and password for totalNumberOfAccount times
                            for (int i = 0; i < totalNumberOfAccounts; i++)
                            {
                                // read in the first line (total number of accounts)
                                lineData = configReader.ReadLine(); // acc name
                                configWriter.WriteLine(lineData);
                                lineData = configReader.ReadLine(); // password
                                configWriter.WriteLine(lineData);
                            }
                            configWriter.WriteLine(accountName);
                            configWriter.WriteLine(password);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("FAILED");
                }
                // Move the Temp file to the Config file
                //
                File.Delete("Config");
                File.Move("Temp", "Config");
            }
        } // END OF addToConfigFile_old( ... )


    }
}

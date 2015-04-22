#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace GradedUnit
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry p1KeyRight;
        MenuEntry p1KeyLeft;
        MenuEntry p1KeyLaunch;
        MenuEntry p1Colour;
        MenuEntry p2KeyRight;
        MenuEntry p2KeyLeft;
        MenuEntry p2KeyLaunch;
        MenuEntry p2Colour;

        enum Colour
        {   
            Red,
            Blue,
            Green,
            Pink
        }

        static Colour p1Colour = Colour.Red ;

        static string[] languages = { "C#", "French", "Deoxyribonucleic acid" };
        static int currentLanguage = 0;

        static bool frobnicate = true;

        static int elf = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            p1KeyRight = new MenuEntry(string.Empty);
            p1KeyLeft = new MenuEntry(string.Empty);
            p1KeyLaunch = new MenuEntry(string.Empty);
            p1Colour = new MenuEntry(string.Empty);
            p2KeyRight = new MenuEntry(string.Empty);
            p2KeyLeft = new MenuEntry(string.Empty);
            p2KeyLaunch = new MenuEntry(string.Empty);
            p2Colour = new MenuEntry(string.Empty);


            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            ungulateMenuEntry.Selected += UngulateMenuEntrySelected;
            p1KeyLeft.Selected += p1KeyLeftSelected;
            p1KeyLaunch.Selected += FrobnicateMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(ungulateMenuEntry);
            MenuEntries.Add(p1KeyLeft);
            MenuEntries.Add(p1KeyLaunch);
            MenuEntries.Add(elfMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            p1KeyRight.Text = "Right Key" + languages[currentLanguage];
            p1KeyLeft.Text = "Left Key" + languages[currentLanguage];
            p1Colour.Text = "Colour " + p1Colour; 
            p1KeyLaunch.Text = "Launch Key" + (frobnicate ? "on" : "off");
            elfMenuEntry.Text = "elf: " + elf;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentUngulate++;

            if (currentUngulate > Ungulate.Llama)
                currentUngulate = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            frobnicate = !frobnicate;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            elf++;

            SetMenuEntryText();
        }


        #endregion
    }
}

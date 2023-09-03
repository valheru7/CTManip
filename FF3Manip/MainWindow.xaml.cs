using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FF3Manip
{
    public partial class MainWindow : Window
    {
        public ManipController ManipController = new ManipController();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartManip(object sender, RoutedEventArgs args)
        {
            Dictionary<string, ManipList.ManipNames> inputToManipMap = new Dictionary<string, ManipList.ManipNames>
            {
                { "Altar Cave", ManipList.ManipNames.AltarCave },
                { "Land Turtle (Back up)", ManipList.ManipNames.LandTurtle },
                
            };
            
            string? buttonText = (args.Source as Button).Content.ToString(); // Text on the button
            
            if (inputToManipMap.ContainsKey(buttonText))
            { 
                ManipController.ExecuteManip(inputToManipMap[buttonText]);
            }
            else
            {
                throw new NotSupportedException(sender + " not a recognised or implemented manip");
            }
        }
    }


}


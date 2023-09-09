using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace FF3Manip
{
    public partial class MainWindow : Window
    {
        public ManipController ManipController = new ManipController();
        public static string systemDateFormat;

        public MainWindow()
        {
            InitializeComponent();
            systemDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;;
        }

        private void StartManip(object sender, RoutedEventArgs args)
        {
            Dictionary<string, ManipList.ManipNames> inputToManipMap = new Dictionary<string, ManipList.ManipNames>
            {
                { "New Game", ManipList.ManipNames.NewGame },
                { "Rainbow Pudding", ManipList.ManipNames.RainbowPudding },
                { "Octomammoth", ManipList.ManipNames.Octomammoth },
                { "Safe Travel", ManipList.ManipNames.SafeTravel },
                { "Pink Tail", ManipList.ManipNames.PinkTail },
                { "Mysidia/Ordeals/Zot", ManipList.ManipNames.MysidiaOrdealsZot },
                { "Lugae/Babil/Rubi", ManipList.ManipNames.LugaeBabilRubi }

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


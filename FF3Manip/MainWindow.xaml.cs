using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace FF3Manip
{
    public partial class MainWindow : Window
    {
        public ManipController ManipController = new ManipController();
        public static string systemDateFormat;

        public MainWindow()
        {
            InitializeComponent();
            ToggleStackVisibility(UACenabled() ? UACStack : MainStack, true);
            systemDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;;
        }

        private void StartManip(object sender, RoutedEventArgs args)
        {
            Dictionary<string, ManipList.ManipNames> inputToManipMap = new Dictionary<string, ManipList.ManipNames>
            {
                { "Altar Cave", ManipList.ManipNames.AltarCave },
                { "Land Turtle (Back up)", ManipList.ManipNames.LandTurtle },
                { "Sealed Cave", ManipList.ManipNames.SealedCave},
                { "Dragon's Peak", ManipList.ManipNames.DragonsPeak},
                { "Tower of Owen", ManipList.ManipNames.TowerOfOwen},
                { "Tower of Owen 2", ManipList.ManipNames.TowenOfOwen2},
                { "Subterranean Lake", ManipList.ManipNames.SubterraneanLake},
                { "Molten Cave", ManipList.ManipNames.MoltenCave},
                { "Hein's Castle", ManipList.ManipNames.HeinCastle},
                { "Cave of Tides", ManipList.ManipNames.CaveOfTides},
                { "Amur Sewers", ManipList.ManipNames.Sewers},
                { "Amur", ManipList.ManipNames.Amur},
                
                
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

        private static bool UACenabled()
        {

            const string uacRegistryPath = 
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";
            int uacValue = (int)Registry.GetValue(uacRegistryPath, "EnableLUA", -1);
            
            return uacValue == 1;
        }

        private void ToggleStackVisibility(StackPanel parent, bool visible)
        {
            parent.Visibility = visible ? Visibility.Visible : Visibility.Hidden;
        }
        
        private void UACOverride(object sender, RoutedEventArgs args)
        {
            ToggleStackVisibility(MainStack, true);
            ToggleStackVisibility(UACStack, false);
        }
    }
}


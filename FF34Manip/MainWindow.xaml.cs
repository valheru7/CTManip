using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FF34Manip
{
    public partial class MainWindow : Window
    {
        public static string AppVersion => $"Version 1.3 - 2025-05-06";
        public ManipController ManipController = new ManipController();
        public static string systemDateFormat;
        public static short timeOffset = 0;

        public MainWindow()
        {
            InitializeComponent();
            systemDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            InitializeTimeService();
        }

        // Verify time service is active to enable /resync
        public void InitializeTimeService()
        {
            string args = "start w32time";
            using (Process startTimeService = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "net.exe",
                    Arguments = args,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            })
            {
                startTimeService.Start();
                startTimeService.WaitForExit();
            }
        }

        private void StartManip(object sender, RoutedEventArgs args)
        {
            Dictionary<string, ManipList.ManipNames> inputToManipMap = new Dictionary<string, ManipList.ManipNames>
            {
                // FF3
                { "Altar Cave", ManipList.ManipNames.AltarCave },
                { "Sealed Cave", ManipList.ManipNames.SealedCave},
                { "Dragon's Peak", ManipList.ManipNames.DragonsPeak},
                { "Tozus Tunnel", ManipList.ManipNames.TozusTunnel},
                { "To Tower of Owen", ManipList.ManipNames.ToTowerOfOwen},
                { "Tower of Owen", ManipList.ManipNames.TowenOfOwen},
                { "Subterranean Lake", ManipList.ManipNames.SubterraneanLake},
                { "Molten Cave", ManipList.ManipNames.MoltenCave},
                { "Hein's Castle", ManipList.ManipNames.HeinCastle},
                { "Cave of Tides", ManipList.ManipNames.CaveOfTides},
                { "Amur Sewers", ManipList.ManipNames.Sewers},
                { "Chocobo's Wrath", ManipList.ManipNames.ChocoboWrath },
                { "Goldor Manor", ManipList.ManipNames.GoldorManor },
                { "Garuda", ManipList.ManipNames.Garuda },
                { "Cave of the Circle", ManipList.ManipNames.CaveOfTheCircle },
                { "Saronia Catacombs", ManipList.ManipNames.SaroniaCatacombs },
                { "Ancients' Maze", ManipList.ManipNames.AncientsMaze },
                { "Cave of Shadows", ManipList.ManipNames.CaveOfShadows },
                { "Shining Curtain", ManipList.ManipNames.ShiningCurtain },
                { "Doga's Grotto", ManipList.ManipNames.DogasGrotto },
                { "To Xande", ManipList.ManipNames.ToXande },
                { "World of Darkness", ManipList.ManipNames.WorldOfDarkness },
                { "Cloud of Darkness", ManipList.ManipNames.CloudOfDarkness },
                
                // FF4
                { "New Game", ManipList.ManipNames.NewGame },
                { "Rainbow Pudding", ManipList.ManipNames.RainbowPudding },
                { "Octomammoth", ManipList.ManipNames.Octomammoth },
                { "Safe Travel", ManipList.ManipNames.SafeTravel },
                { "Pink Tail", ManipList.ManipNames.PinkTail },
                { "Mysidia/Ordeals/Zot", ManipList.ManipNames.MysidiaOrdealsZot },
                { "Lugae", ManipList.ManipNames.Lugae },
                { "Babil/Rubi", ManipList.ManipNames.BabilRubi }

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

        // Check for valid positive and negative integers
        private static bool IsNumeric(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            // Allow a single hyphen for negative offsets
            if (text == "-")
            {
                return true;
            }
            if (text[0] == '-' && text.Length > 1)
            {
                return int.TryParse(text.Substring(1), out _);
            }
            return int.TryParse(text, out _);
        }
        
        // Check input is valid before registering or parsing it
        private void Offset_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Parse for valid input
            TextBox textBox = e.Source as TextBox;
            e.Handled = !IsNumeric(textBox.Text + e.Text);
            
            // Assign offset if input is valid
            if (short.TryParse(textBox.Text + e.Text, out short result))
            {
                timeOffset = result;
            }
        }

        private void Offset_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Invalidate spaces
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            // Prevent pasting non-numeric values
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                string clipboardText = Clipboard.GetText();
                e.Handled = !IsNumeric(clipboardText);
            }
        }
    }
}


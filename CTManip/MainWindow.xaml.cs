using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CTManip
{
    public partial class MainWindow : Window
    {
        public static string AppVersion => $"Version 1.5 - 2025-07-01";
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
            // Determine the button text and normalize it to attempt a match against the enum.
            string? buttonText = (args.Source as Button)?.Content?.ToString(); // Text on the button
            if (string.IsNullOrWhiteSpace(buttonText))
                throw new ArgumentNullException(nameof(buttonText));

            // Normalize: keep letters and digits only (e.g. "Nizbel 2" => "Nizbel2", "New Game" => "NewGame")
            string normalized = new string(buttonText.Where(char.IsLetterOrDigit).ToArray());

            // Try parse into the enum (case-insensitive). This avoids compile-time references to enum members
            // and will succeed for any ManipNames that actually exist.
            if (Enum.TryParse<ManipList.ManipNames>(normalized, ignoreCase: true, out var manip))
            {
                ManipController.ExecuteManip(manip);
                return;
            }

            // If not parseable, try a small display->enum fallback mapping for any special cases.
            var fallback = new Dictionary<string, ManipList.ManipNames>(StringComparer.OrdinalIgnoreCase)
            {
                { "Nizbel 2", ManipList.ManipNames.Nizbel2 }
                // add other manual mappings here if needed
            };

            if (fallback.TryGetValue(buttonText, out manip))
            {
                ManipController.ExecuteManip(manip);
                return;
            }

            throw new NotSupportedException(buttonText + " not a recognised or implemented manip");
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


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace CTManip
{
    public partial class MainWindow : Window
    {
        public static string AppVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var infoAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                string version = infoAttr?.InformationalVersion ?? assembly.GetName().Version?.ToString() ?? "0.0";
                return $"Version {version}";
            }
        }
        public ManipController ManipController;
        public static string systemDateFormat = "";
        private bool isBaseRngMode = false;
    // offset removed

        public MainWindow()
        {
            InitializeComponent();
            ManipController = new ManipController();
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
                { "New Game", ManipList.ManipNames.NewGame },
                { "New Game Xstrike", ManipList.ManipNames.NewGame_XstrikeGrind },
                { "Yakra", ManipList.ManipNames.Yakra },
                { "Nagas", ManipList.ManipNames.Nagas },
                { "Dragon Tank", ManipList.ManipNames.DragonTank },
                { "Guardian", ManipList.ManipNames.Guardian },
                { "Zombor", ManipList.ManipNames.Zombor },
                { "Zombor Silver", ManipList.ManipNames.ZomborSilverEarring },
                { "Masamune", ManipList.ManipNames.Masamune },
                { "Nizbel", ManipList.ManipNames.Nizbel },
                { "Flea", ManipList.ManipNames.Flea },
                { "Magus", ManipList.ManipNames.Magus },
                { "Nizbel 2", ManipList.ManipNames.Nizbel2 },
                { "Black Tyranno", ManipList.ManipNames.BlackTyranno },
                { "Mud Imp", ManipList.ManipNames.MudImp },
                { "Woe Rubble", ManipList.ManipNames.WoeRubble },
                { "Golem Twins", ManipList.ManipNames.GolemTwins },
                { "Ghosts", ManipList.ManipNames.Ghosts },
                { "Rust Rubbles", ManipList.ManipNames.RustRubbles },
                { "Rust Tyranno", ManipList.ManipNames.RustTyranno },
                { "Son of Sun", ManipList.ManipNames.SonOfSun },
                { "Yakra XIII", ManipList.ManipNames.YakraXIII },
                { "Black Omen", ManipList.ManipNames.BlackOmen },
                { "Lavos Shell", ManipList.ManipNames.LavosShell },
                { "Lavos Core", ManipList.ManipNames.LavosCore }
            };
            
            string? buttonText = (args.Source as Button)?.Content?.ToString(); // Text on the button
            
            if (buttonText != null && inputToManipMap.ContainsKey(buttonText))
            { 
                bool shouldStartGame = StartGameCheckBox.IsChecked ?? true;
                ManipController.ExecuteManip(inputToManipMap[buttonText], shouldStartGame);
            }
            else
            {
                throw new NotSupportedException(sender + " not a recognised or implemented manip");
            }
        }

        private void ToggleBaseRNG(object sender, RoutedEventArgs e)
        {
            isBaseRngMode = BaseRngMenuItem.IsChecked;          
            PopulateManipButtons();
        }

        private void PopulateManipButtons()
        {
            ManipButtonsPanel.Children.Clear();

            if (isBaseRngMode)
            {
                // Clear grid definitions for BaseRNG mode
                ManipButtonsPanel.RowDefinitions.Clear();
                ManipButtonsPanel.ColumnDefinitions.Clear();
                
                // Create BaseRNG input interface in a StackPanel
                var baseRngPanel = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var label = new Label
                {
                    Content = "Enter BaseRNG value (01-FF):",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 5)
                };

                var textBox = new TextBox
                {
                    Name = "BaseRngTextBox",
                    Width = 50,
                    Height = 25,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    MaxLength = 2,
                    CharacterCasing = CharacterCasing.Upper,
                    Margin = new Thickness(0, 5, 0, 10)
                };
                textBox.KeyDown += BaseRngTextBox_KeyDown;

                var executeButton = new Button
                {
                    Content = "Execute",
                    Width = 100,
                    Height = 35,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 10, 0, 10),
                    FontSize = 14
                };
                executeButton.Click += ExecuteBaseRngManip;

                var instructionLabel = new Label
                {
                    Content = "Valid range: 01 to FF",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 10,
                    Foreground = System.Windows.Media.Brushes.Gray,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                baseRngPanel.Children.Add(label);
                baseRngPanel.Children.Add(textBox);
                baseRngPanel.Children.Add(executeButton);
                baseRngPanel.Children.Add(instructionLabel);

                ManipButtonsPanel.Children.Add(baseRngPanel);
            }
            else
            {
                // Recreate the two-column layout for normal manips
                ManipButtonsPanel.RowDefinitions.Clear();
                ManipButtonsPanel.ColumnDefinitions.Clear();
                ManipButtonsPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                ManipButtonsPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                ManipButtonsPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                ManipButtonsPanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                // Main column header
                var mainHeader = new TextBlock
                {
                    Text = "Main",
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 6)
                };
                Grid.SetRow(mainHeader, 0);
                Grid.SetColumn(mainHeader, 0);
                ManipButtonsPanel.Children.Add(mainHeader);

                // Backup column header
                var backupHeader = new TextBlock
                {
                    Text = "Backup",
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 6)
                };
                Grid.SetRow(backupHeader, 0);
                Grid.SetColumn(backupHeader, 1);
                ManipButtonsPanel.Children.Add(backupHeader);

                // Main column buttons
                var mainManips = new[] { "New Game", "Zombor", "Flea", "Nizbel 2", "Mud Imp", "Ghosts", "Yakra XIII", "Black Omen", "Lavos Shell", "Lavos Core" };
                var mainStackPanel = new StackPanel();
                foreach (var manip in mainManips)
                {
                    var button = new Button
                    {
                        Width = 150,
                        Content = manip,
                        Margin = new Thickness(8),
                        MinWidth = 200,
                        MinHeight = 36,
                        FontSize = 14,
                        Padding = new Thickness(12, 8, 12, 8)
                    };
                    button.Click += StartManip;
                    mainStackPanel.Children.Add(button);
                }
                Grid.SetRow(mainStackPanel, 1);
                Grid.SetColumn(mainStackPanel, 0);
                ManipButtonsPanel.Children.Add(mainStackPanel);

                // Backup column buttons
                var backupManips = new[] { "New Game Xstrike", "Nagas", "Dragon Tank", "Guardian", "Masamune", "Nizbel", "Magus", "Black Tyranno", "Woe Rubble", "Golem Twins", "Rust Rubbles", "Rust Tyranno", "Son of Sun" };
                var backupStackPanel = new StackPanel();
                foreach (var manip in backupManips)
                {
                    var button = new Button
                    {
                        Width = 150,
                        Content = manip,
                        Margin = new Thickness(8),
                        MinWidth = 200,
                        MinHeight = 36,
                        FontSize = 14,
                        Padding = new Thickness(12, 8, 12, 8)
                    };
                    button.Click += StartManip;
                    backupStackPanel.Children.Add(button);
                }
                Grid.SetRow(backupStackPanel, 1);
                Grid.SetColumn(backupStackPanel, 1);
                ManipButtonsPanel.Children.Add(backupStackPanel);
            }
        }

        private void BaseRngTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteBaseRngManip(sender, new RoutedEventArgs());
            }
        }

        private void ExecuteBaseRngManip(object sender, RoutedEventArgs e)
        {
            // Find the text box in the current panel
            TextBox? textBox = null;
            foreach (var child in ManipButtonsPanel.Children)
            {
                if (child is TextBox tb)
                {
                    textBox = tb;
                    break;
                }
            }

            if (textBox == null)
            {
                MessageBox.Show("Error: Could not find input field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string input = textBox.Text.Trim().ToUpper();
            
            // Validate input
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Please enter a value.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.Focus();
                return;
            }

            // Ensure input is exactly 2 characters for hex values 01-FF
            if (input.Length == 1)
            {
                input = "0" + input;
                textBox.Text = input;
            }

            // Validate hex format and range
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, "^[0-9A-F]{2}$"))
            {
                MessageBox.Show("Please enter a valid hexadecimal value (01-FF).", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.SelectAll();
                textBox.Focus();
                return;
            }

            // Convert hex to decimal to check range
            if (int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out int value))
            {
                if (value < 1 || value > 255)
                {
                    MessageBox.Show("Value must be between 01 and FF (1-255).", "Invalid Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                    textBox.SelectAll();
                    textBox.Focus();
                    return;
                }

                // Execute the BaseRNG manip
                string enumName = $"BaseRNG_{input}";
                if (Enum.TryParse<ManipList.ManipNames>(enumName, out var baseRngManip))
                {
                    bool shouldStartGame = StartGameCheckBox.IsChecked ?? true;
                    ManipController.ExecuteManip(baseRngManip, shouldStartGame);
                }
                else
                {
                    MessageBox.Show($"BaseRNG manip {input} not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid hexadecimal value.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                textBox.SelectAll();
                textBox.Focus();
            }
        }

        // offset UI and handlers removed
    }
}


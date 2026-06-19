using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input.Platform;
using System.Linq;

namespace SimplePassGen;

public partial class MainWindow : Window
{
    // --- Character sets ---
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers   = "0123456789";
    private const string Symbols   = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    // --- Settings ---
    private int    _length      = 16;
    private bool   _useUppercase = true;
    private bool   _useLowercase = true;
    private bool   _useNumbers   = true;
    private bool   _useSymbols   = false;

    // --- Output ---
    private string _generatedPassword = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
    }

    // --- Generate button click ---
    private void GenerateButton_Click(object? sender, RoutedEventArgs e)
    {
        // Read values from controls
        _length       = (int)(LengthInput.Value ?? 16);
        _useUppercase = UppercaseCheck.IsChecked ?? false;
        _useLowercase = LowercaseCheck.IsChecked ?? false;
        _useNumbers   = NumbersCheck.IsChecked   ?? false;
        _useSymbols   = SymbolsCheck.IsChecked   ?? false;

        // Build character pool
        var pool = new StringBuilder();
        if (_useUppercase)
            pool.Append(Uppercase);

        if (_useLowercase)
            pool.Append(Lowercase);

        if (_useNumbers)
            pool.Append(Numbers);

        if (_useSymbols)
            pool.Append(Symbols);

        // Guard: nothing selected
        if (pool.Length == 0)
        {
            PasswordOutput.Text = "Select at least one option!";
            return;
        }

        // Generate password
        var rng      = new Random();
        var password = new StringBuilder();
        for (int i = 0; i < _length; i++)
            password.Append(pool[rng.Next(pool.Length)]);

        // Password validation
        if(_useUppercase && !password.ToString().Any(c => Uppercase.Contains(c)))
        {
            password[rng.Next(password.Length)] = Uppercase[rng.Next(Uppercase.Length)];
        }
        if(_useLowercase && !password.ToString().Any(c => Lowercase.Contains(c)))
        {
            password[rng.Next(password.Length)] = Lowercase[rng.Next(Lowercase.Length)];
        }
        if(_useNumbers && !password.ToString().Any(c => Numbers.Contains(c)))
        {
            password[rng.Next(password.Length)] = Numbers[rng.Next(Numbers.Length)];
        }
        if(_useSymbols && !password.ToString().Any(c => Symbols.Contains(c)))
        {
            password[rng.Next(password.Length)] = Symbols[rng.Next(Symbols.Length)];
        }

        _generatedPassword  = password.ToString();
        PasswordOutput.Text = _generatedPassword;
    }

    // --- Copy button click ---
    private async void CopyButton_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_generatedPassword)) return;

        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard is not null)
            await clipboard.SetTextAsync(_generatedPassword);

    }    
}
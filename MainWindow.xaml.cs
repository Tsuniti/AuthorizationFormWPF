using AuthorizationFormWPF.Database;
using AuthorizationFormWPF.Entities;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace AuthorizationFormWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    UsersDB db = new UsersDB();
    public bool IsDarkTheme { get; set; }
    private readonly PaletteHelper _paletteHelper;

    public MainWindow()
    {
        _paletteHelper = new PaletteHelper();
        InitializeComponent();

        SubmitButton.Click += LogIn;
        ChangeFormButton.Click += SignUpPaint;
    }

    private void ToggleTheme(object sender, RoutedEventArgs e)
    {
        ITheme theme = _paletteHelper.GetTheme();

        if (theme.GetBaseTheme() == BaseTheme.Dark)
        {
            IsDarkTheme = false;
            theme.SetBaseTheme(Theme.Light);
        }
        else
        {
            IsDarkTheme = true;
            theme.SetBaseTheme(Theme.Dark);
        }
        _paletteHelper.SetTheme(theme);
    }

    private void CloseApplication(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        DragMove();
    }

    private void LogIn(object sender, RoutedEventArgs e)
    {
        ErrorLabel.SetResourceReference(Control.ForegroundProperty, "MaterialDesignValidationErrorBrush");
        bool IsFieldsEmpty = false;
        if (UsernameTextBox.Text == string.Empty)
        {
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");

            IsFieldsEmpty = true;
        }
        if (PasswordTextBox.Password == string.Empty)
        {
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (IsFieldsEmpty)
        {
            ErrorLabel.Content = "Fields can't be empty";
            return;
        }




        if (db.GetUserByUsername(UsernameTextBox.Text) == null ||
            db.GetUserByUsername(UsernameTextBox.Text).Password != PasswordTextBox.Password)
        {
            ErrorLabel.Content = "Username or password does not match";
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
        }
        else
        {
            ErrorLabel.Content = "Success login";
            ErrorLabel.Foreground = Brushes.Lime;
            UsernameTextBox.BorderBrush = Brushes.Lime;
            PasswordTextBox.BorderBrush = Brushes.Lime;
        }

    }


    private void SignUp(object sender, RoutedEventArgs e)
    {
        ErrorLabel.SetResourceReference(Control.ForegroundProperty, "MaterialDesignValidationErrorBrush");

        bool IsFieldsEmpty = false;
        if (UsernameTextBox.Text == string.Empty)
        {
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;
        }
        if (PasswordTextBox.Password == string.Empty)
        {
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (PasswordRepeatTextBox.Password == string.Empty)
        {
            PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (IsFieldsEmpty)
        {
            ErrorLabel.Content = "Fields can't be empty";
            return;
        }
        if (UsernameTextBox.Text.Length <= 3)
        {
            ErrorLabel.Content = "Username must be longer than 2 characters";
            return;
        }

        if (db.GetUserByUsername(UsernameTextBox.Text) != null)
        {

            ErrorLabel.Content = "This username already taken";
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            return;

        }
        if (PasswordTextBox.Password.Length <= 4)
        {
            ErrorLabel.Content = "Password must be longer than 3 characters";
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");

            return;
        }
        if (PasswordTextBox.Password != PasswordRepeatTextBox.Password)
        {
            ErrorLabel.Content = "Password mismatch";
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            return;
        }

        db.AddUser(new User(UsernameTextBox.Text, PasswordTextBox.Password));
        ErrorLabel.Content = "Success signup";
        ErrorLabel.Foreground = Brushes.Lime;
        UsernameTextBox.BorderBrush = Brushes.Lime;
        PasswordTextBox.BorderBrush = Brushes.Lime;
        PasswordRepeatTextBox.BorderBrush = Brushes.Lime;



    }

    private void LogInPaint(object sender, RoutedEventArgs e)
    {
        HidePasswordRepeatTextBox();

        UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        ErrorLabel.Content = String.Empty;

        WelcomeLabel.Content = "Welcome back!";
        SuggestionLabel.Content = "Log in to your existing account";

        SubmitButton.Click -= SignUp;
        SubmitButton.Click += LogIn;
        SubmitButton.Content = "LOG IN";

        ChangeFormButton.Click -= LogInPaint;
        ChangeFormButton.Click += SignUpPaint;
        ChangeFormButton.Content = "Create account";

    }


    private void SignUpPaint(object sender, RoutedEventArgs e)
    {
        UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        ErrorLabel.Content = String.Empty;
        WelcomeLabel.Content = "Welcome!";
        SuggestionLabel.Content = "Create a new account";

        ShowPasswordRepeatTextBox();
        SubmitButton.Click -= LogIn;
        SubmitButton.Click += SignUp;
        SubmitButton.Content = "SIGN UP";


        ChangeFormButton.Click -= SignUpPaint;
        ChangeFormButton.Click += LogInPaint;
        ChangeFormButton.Content = "Log in";


    }

    private async void ShowPasswordRepeatTextBox()
    {
        var opacityAnimation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginAnimation = new ThicknessAnimation
        {
            From = new Thickness(0, -58, 0, 0),
            To = new Thickness(0, 20, 0, 0),
            Duration = TimeSpan.FromSeconds(0.3)
        };

        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginAnimation);

        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);


        await Task.Delay(TimeSpan.FromSeconds(0.3));
        PasswordRepeatTextBox.IsEnabled = true;

    }
    private async void HidePasswordRepeatTextBox()
    {
        PasswordRepeatTextBox.IsEnabled = false;

        DoubleAnimation opacityAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginAnimation = new ThicknessAnimation
        {
            From = new Thickness(0, 20, 0, 0),
            To = new Thickness(0, -58, 0, 0),
            Duration = TimeSpan.FromSeconds(0.3)
        };


        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginAnimation);


        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        await Task.Delay(TimeSpan.FromSeconds(0.3));

        PasswordRepeatTextBox.Password = String.Empty;


    }

    private void ChangeTextBoxesBorderColorToDefault()
    {
        if (UsernameTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider") &&
            PasswordTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider") &&
            PasswordRepeatTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider"))
        {
            ErrorLabel.Content = String.Empty;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
            TextBox textBox = sender as TextBox;
            if (textBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignValidationErrorBrush"))
            {
                textBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
            }


        ChangeTextBoxesBorderColorToDefault();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = sender as PasswordBox;
        if (passwordBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignValidationErrorBrush"))
        {
            passwordBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        }
        ChangeTextBoxesBorderColorToDefault();
    }
}
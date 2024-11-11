using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DopravniPodnik.Utils;

public static class PasswordBoxHelper
{
    public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached(
            "BoundPassword", 
            typeof(SecureString), 
            typeof(PasswordBoxHelper), 
            new PropertyMetadata(new SecureString(), OnBoundPasswordChanged));

    public static readonly DependencyProperty PropertyNameProperty =
        DependencyProperty.RegisterAttached(
            "PropertyName", 
            typeof(string), 
            typeof(PasswordBoxHelper), 
            new PropertyMetadata(string.Empty));

    private static bool _isPasswordBeingSetProgrammatically = false;

    public static SecureString GetBoundPassword(DependencyObject obj) =>
        (SecureString)obj.GetValue(BoundPasswordProperty);

    public static void SetBoundPassword(DependencyObject obj, SecureString value) =>
        obj.SetValue(BoundPasswordProperty, value);

    public static string GetPropertyName(DependencyObject obj) =>
        (string)obj.GetValue(PropertyNameProperty);

    public static void SetPropertyName(DependencyObject obj, string value) =>
        obj.SetValue(PropertyNameProperty, value);

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            // Only set the password if it's not already being set programmatically
            if (!_isPasswordBeingSetProgrammatically)
            {
                // Convert SecureString to string for PasswordBox
                var securePassword = e.NewValue as SecureString;
                if (securePassword != null)
                {
                    passwordBox.Password = ConvertToUnsecureString(securePassword);
                }
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
    }

    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            // Set the flag to prevent the property from being reset programmatically
            _isPasswordBeingSetProgrammatically = true;

            // Convert PasswordBox's SecurePassword to SecureString
            SetBoundPassword(passwordBox, passwordBox.SecurePassword);

            // Get the PropertyName for the property to bind to
            string propertyName = GetPropertyName(passwordBox);
            if (passwordBox.DataContext != null && !string.IsNullOrEmpty(propertyName))
            {
                var viewModel = passwordBox.DataContext;

                // Use reflection to set the corresponding SecureString property in the ViewModel
                var propertyInfo = viewModel.GetType().GetProperty(propertyName);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(viewModel, passwordBox.SecurePassword);
                }
            }

            // Reset the flag after the password is set
            _isPasswordBeingSetProgrammatically = false;
        }
    }

    public static string ConvertToUnsecureString(SecureString securePassword)
    {
        // Convert SecureString to plain string (not recommended for sensitive data)
        IntPtr ptr = IntPtr.Zero;
        try
        {
            ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(securePassword);
            return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
        }
        finally
        {
            if (ptr != IntPtr.Zero)
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}
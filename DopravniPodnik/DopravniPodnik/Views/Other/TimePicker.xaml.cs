using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DopravniPodnik.Views.Other;

public partial class TimePicker : UserControl
{
    public TimePicker()
    {
        InitializeComponent();
        // DataContext = this;

        // Initialize Hours and Minutes
        Hours = Enumerable.Range(0, 24).Select(h => h.ToString("D2")).ToList();
        Minutes = Enumerable.Range(0, 60).Select(m => m.ToString("D2")).ToList();
    }

    // Hours (0-23) and Minutes (0-59)
    public List<string> Hours { get; }
    public List<string> Minutes { get; }

    // SelectedTime Dependency Property
    public DateTime SelectedTime
    {
        get => (DateTime)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }
    public static readonly DependencyProperty SelectedTimeProperty =
        DependencyProperty.Register(
            "SelectedTime",
            typeof(DateTime),
            typeof(TimePicker),
            new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                OnSelectedTimeChanged, null, true, UpdateSourceTrigger.PropertyChanged));

    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;
        var newTime = (DateTime)e.NewValue;

        // Update SelectedHour and SelectedMinute based on SelectedTime
        control.SelectedHour = newTime.Hour.ToString("D2");
        control.SelectedMinute = newTime.Minute.ToString("D2");
    }

    // SelectedHour Dependency Property
    public string SelectedHour
    {
        get => (string)GetValue(SelectedHourProperty);
        set => SetValue(SelectedHourProperty, value);
    }

    public static readonly DependencyProperty SelectedHourProperty =
        DependencyProperty.Register("SelectedHour", typeof(string), typeof(TimePicker),
            new FrameworkPropertyMetadata(DateTime.Now.Hour.ToString("D2"),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHourOrMinuteChanged));

    // SelectedMinute Dependency Property
    public string SelectedMinute
    {
        get => (string)GetValue(SelectedMinuteProperty);
        set => SetValue(SelectedMinuteProperty, value);
    }

    public static readonly DependencyProperty SelectedMinuteProperty =
        DependencyProperty.Register("SelectedMinute", typeof(string), typeof(TimePicker),
            new FrameworkPropertyMetadata(DateTime.Now.Minute.ToString("D2"),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHourOrMinuteChanged));

    private static void OnHourOrMinuteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (TimePicker)d;

        if (int.TryParse(control.SelectedHour, out var hour) && int.TryParse(control.SelectedMinute, out var minute))
        {
            // Update SelectedTime based on SelectedHour and SelectedMinute
            control.SelectedTime = new DateTime(control.SelectedTime.Year, control.SelectedTime.Month, control.SelectedTime.Day, hour, minute, 0);
        }
    }
}
using System.ComponentModel;

namespace Licznik_Jan_Skwarek.Models;

public class Counter : INotifyPropertyChanged
{
    public string Name { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    private int _value;

    public int Value
    {
        get => _value;
        set
        {
            if (_value == value)
            {
                return;
            }

            _value = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

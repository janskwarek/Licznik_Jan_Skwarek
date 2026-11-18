using Licznik_Jan_Skwarek.Models;
using System.Collections.ObjectModel;

namespace Licznik_Jan_Skwarek;

public partial class MainPage : ContentPage
{
    private bool _isInitialized;
    private readonly string _storagePath = Path.Combine(FileSystem.AppDataDirectory, "moje_dane_licznikow.txt");

    public ObservableCollection<Counter> Counters { get; } = [];

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_isInitialized) return;

        LoadAllData();
        _isInitialized = true;
    }

    private void LoadAllData()
    {
        Counters.Clear();

        if (!File.Exists(_storagePath)) return;

        var lines = File.ReadAllLines(_storagePath);
        for (int i = 0; i < lines.Length; i += 2)
        {
            if (i + 1 < lines.Length)
            {
                Counters.Add(new Counter
                {
                    Name = lines[i],
                    Value = int.Parse(lines[i + 1])
                });
            }
        }
    }

    private void SaveAllData()
    {
        var linesToSave = new List<string>();
        foreach (var item in Counters)
        {
            linesToSave.Add(item.Name);
            linesToSave.Add(item.Value.ToString());
        }
        File.WriteAllLines(_storagePath, linesToSave);
    }

    private void OnAddCounterClicked(object? sender, EventArgs e)
    {
        string inputName = CounterNameEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(inputName)) return;

        var newCounter = new Counter { Name = inputName, Value = 0 };
        Counters.Add(newCounter);
        
        CounterNameEntry.Text = string.Empty;
        SaveAllData();
    }

    private void OnDecrementClicked(object? sender, EventArgs e)
    {
        ModifyTargetCounter(sender, -1);
    }

    private void OnIncrementClicked(object? sender, EventArgs e)
    {
        ModifyTargetCounter(sender, 1);
    }

    private static Counter? ExtractCounterFromSender(object? sender)
    {
        if (sender is Button btn && btn.BindingContext is Counter match)
        {
            return match;
        }
        return null;
    }

    private void ModifyTargetCounter(object? sender, int step)
    {
        var target = ExtractCounterFromSender(sender);
        if (target == null) return;

        target.Value += step;
        SaveAllData();
    }
}

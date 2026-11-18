using Licznik_Jan_Skwarek.Models;
using System.Collections.ObjectModel;

namespace Licznik_Jan_Skwarek;

public partial class MainPage : ContentPage
{
    private bool _hasLoadedCounters;

    public ObservableCollection<Counter> Counters { get; } = [];

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_hasLoadedCounters)
        {
            return;
        }

        LoadCounters();
        _hasLoadedCounters = true;
    }

    private void LoadCounters()
    {
        Counters.Clear();

        var filePaths = Directory.GetFiles(FileSystem.AppDataDirectory, "licznik-*.txt");
        foreach (var filePath in filePaths)
        {
            Counters.Add(ReadCounterFromFile(filePath));
        }
    }

    private static Counter ReadCounterFromFile(string filePath)
    {
        var fileLines = File.ReadAllLines(filePath);
        return new Counter
        {
            Name = fileLines[0],
            Value = int.Parse(fileLines[1]),
            FileName = Path.GetFileName(filePath)
        };
    }

    private void OnAddCounterClicked(object? sender, EventArgs e)
    {
        var counterName = CounterNameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(counterName))
        {
            return;
        }

        var counter = new Counter { Name = counterName };
        Counters.Add(counter);
        CounterNameEntry.Text = string.Empty;
        SaveCounter(counter);
    }

    private void OnDecrementClicked(object? sender, EventArgs e)
    {
        ChangeCounterValue(sender, -1);
    }

    private void OnIncrementClicked(object? sender, EventArgs e)
    {
        ChangeCounterValue(sender, 1);
    }

    private static Counter? GetCounter(object? sender)
    {
        return (sender as Button)?.BindingContext as Counter;
    }

    private void ChangeCounterValue(object? sender, int change)
    {
        if (GetCounter(sender) is not { } counter)
        {
            return;
        }

        counter.Value += change;
        SaveCounter(counter);
    }

    private static void SaveCounter(Counter counter)
    {
        if (string.IsNullOrEmpty(counter.FileName))
        {
            counter.FileName = $"licznik-{Path.GetRandomFileName()}.txt";
        }

        var filePath = Path.Combine(FileSystem.AppDataDirectory, counter.FileName);
        File.WriteAllLines(filePath, [counter.Name, counter.Value.ToString()]);
    }
}

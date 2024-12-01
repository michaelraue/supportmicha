namespace SupportMicha.WpfUi.Shared;

using System.Collections;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

/// <summary>
/// An abstract base class that provides validation functionality for view models.
/// Implements the <see cref="INotifyDataErrorInfo"/> interface to support
/// data validation and error reporting.
/// </summary>
public abstract class ValidationViewModel : ObservableObject, INotifyDataErrorInfo
{
    private readonly Dictionary<string, List<string>> errors = new();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => this.errors.Any();

    public IEnumerable GetErrors(string? propertyName)
    {
        return propertyName != null && this.errors.TryGetValue(propertyName, out var error)
            ? error
            : Enumerable.Empty<string>();
    }

    protected void AddError(string propertyName, string error)
    {
        if (!this.errors.ContainsKey(propertyName))
        {
            this.errors[propertyName] = [];
        }

        this.errors[propertyName].Add(error);
        this.OnErrorsChanged(propertyName);
    }

    protected void ClearErrors(string? propertyName = null)
    {
        if (propertyName == null)
        {
            foreach (var error in this.errors.ToDictionary())
            {
                this.ClearErrors(error.Key);
            }

            return;
        }

        this.errors.Remove(propertyName);
        this.OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string? propertyName)
    {
        this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}
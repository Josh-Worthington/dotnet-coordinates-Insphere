using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Viewer.ViewModels;

/// <summary>
/// 	The view model base, implements INotifyPropertyChanged to notify the view of updates to properties.
/// </summary>
/// <seealso cref="INotifyPropertyChanged"/>
public abstract class ViewModelBase(
	ILogger<ViewModelBase> logger) : INotifyPropertyChanged
{
	private readonly Dictionary<string, object> _propertyPicker = new();

	/// <inheritdoc />
	public event PropertyChangedEventHandler? PropertyChanged;

	/// <summary>
	/// 	Sets a value in the property picker dictionary.
	/// </summary>
	/// <typeparam name="T">	Generic type parameter. </typeparam>
	/// <param name="value">	   	Desired value for the property. </param>
	/// <param name="propertyName">	(Optional) Name of the property used to notify listeners. This value is optional and can be provided automatically when invoked from compilers that support CallerMemberName. </param>
	/// <returns>
	/// 	True if it updates the value, false if not.
	/// </returns>
	protected bool SetValue<T>(
		T value,
		[CallerMemberName] string? propertyName = null)
	{
		ArgumentNullException.ThrowIfNull(propertyName);

		var hasChanged = false;
		// Property is not yet set
		if (!_propertyPicker.TryGetValue(propertyName, out var storedValue))
		{
			_propertyPicker[propertyName] = value;
			hasChanged = true;
		}

		// value is different
		else if (value is not null && !value.Equals(storedValue))
		{
			_propertyPicker[propertyName] = value;
			hasChanged = true;
		}

		if (hasChanged) RaisePropertyChanged(propertyName);

		return hasChanged;
	}

	/// <summary>
	/// 	Gets a value from the property picker dictionary.
	/// </summary>
	/// <typeparam name="T">	Generic type parameter. </typeparam>
	/// <param name="propertyName">	(Optional) Name of the property used to notify listeners. This value is optional and can be provided automatically when invoked from compilers that support CallerMemberName. </param>
	/// <returns>
	/// 	The value of the property from the dictionary.
	/// </returns>
	protected T GetValue<T>(
		[CallerMemberName] string? propertyName = null)
	{
		ArgumentNullException.ThrowIfNull(propertyName);

		if (!_propertyPicker.TryGetValue(propertyName, out var value))
		{
			logger.LogWarning("Property {PropertyName} does not exist in the storage", propertyName);
		}

		if ((Nullable.GetUnderlyingType(typeof(T)) is not null || default(T) is null) && value is null)
		{
			return default;
		}
		if (value is not T castedValue)
		{
			throw new InvalidCastException("Cannot cast to expected type");
		}

		return castedValue;
	}

	/// <summary>
	/// 	Raises the property changed event, to notify the view about changes to properties.
	/// </summary>
	/// <param name="propertyName">	(Optional) Name of the property used to notify listeners. This value is optional and can be provided automatically when invoked from compilers that support CallerMemberName. </param>
	protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
	{
		ArgumentNullException.ThrowIfNull(propertyName);

		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	/// <summary>
	/// 	Raises the property changed event, to notify the view about changes to properties.
	/// </summary>
	/// <param name="propertyNames">	Name of the properties used to notify listeners. </param>
	protected void RaisePropertiesChanged(params string[] propertyNames)
	{
		foreach (var propertyName in propertyNames)
		{
			RaisePropertyChanged(propertyName);
		}
	}
}
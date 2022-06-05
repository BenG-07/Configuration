# Configuration of own DataUnits
To configure a DataUnit, you must set the DataUnitInfoAttribute for the class that handles the communication (emitting and received data) with the ConfigureManager.

For data emission, the NewValueAvailableEventArgs<T> must be used and the event must be flagged with the DataSourceAttribute.

For data receiving, the DataDestinationAttribute must be used and the corresponding type (from the DataUnitInfoAttribute.OutputType) must be the only parameter.

DataSourceUnits must have a public void Start() and Stop() method without parameters. This functionality should enable/disable data-emission.


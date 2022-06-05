# Configuration of own DataUnits
To configure a DataUnit, you must set the DataUnitInfoAttribute for the class that handles the communication (emitting and received data) with the ConfigureManager.

For data emission, the NewValueAvailableEventArgs<T> must be used and the event must be flagged with the DataSourceAttribute. These EventArgs must be the same as the ones from the ConfigurationManager, thus the following code ist needed to create the EventArgs:
var assembly = Assembly.LoadFrom(@"DUEvents.dll");
var eventType = assembly.GetType("DUEvents.NewValueAvailableEventArgs`1").MakeGenericType(typeof(int));
var constructor = eventType.GetConstructor(new Type[] { typeof(int) });
var eventArgs = constructor.Invoke(new object[] { number });

For data receiving, the DataDestinationAttribute must be used and the corresponding type (from the DataUnitInfoAttribute.OutputType) must be the only parameter.

DataSourceUnits must have a public void Start() and Stop() method without parameters. This functionality should enable/disable data-emission.


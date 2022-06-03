namespace DUAttributes.DataUnitType
{
    public interface IDataUnitVisitable
    {
        void Accept(IDataUnitVisitor visitor);

        T Accept<T>(IDataUnitVisitor<T> visitor);
    }
}
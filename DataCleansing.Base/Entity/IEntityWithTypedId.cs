namespace DataCleansing.Base.Entity
{
    interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}

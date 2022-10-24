using System;
using System.Xml.Serialization;

namespace DataCleansing.Base.Entity
{
    [Serializable]
    public abstract class EntityWithTypedId<TId> : IEntityWithTypedId<TId>
    {
        [XmlIgnore]
        public virtual TId Id { get; protected set; }
    }
}

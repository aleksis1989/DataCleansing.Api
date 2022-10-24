using System;

namespace DataCleansing.Base.Entity
{
    /// <summary>
    /// Основен ентитет (модел) кој што го наследуваат сите доменски модели. Ги содржи потребните информации за корисникот кој што го креирал/изменувал даден запис во база
    /// </summary>
    /// <typeparam name="T">Тип на идентификатор класата која што го наследува овој модел</typeparam>
    /// <seealso cref="IBaseEntity" />
    public class BaseEntity<T> : EntityWithTypedId<T>, IBaseEntity
    {
        /// <summary>
        /// Датум и време на креирање на записот во база
        /// </summary>
        public virtual DateTime CreatedOn { get; set; }

        /// <summary>
        /// Датум и време на кога записот бил изменет во база
        /// </summary>
        public virtual DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Корисникот кој што го креирал записот во база
        /// </summary>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// Корисникот кој што го изменил записот во база
        /// </summary>
        public virtual string ModifiedBy { get; set; }
    }
}

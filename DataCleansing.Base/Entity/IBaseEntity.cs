using System;

namespace DataCleansing.Base.Entity
{
    /// <summary>
    /// Интерфес за основен ентитет кој што го имплементираат сите доменски модели
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Корисникот кој што го креирал записот во база
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// Корисникот кој што го изменил записот во база
        /// </summary>
        string ModifiedBy { get; set; }

        /// <summary>
        /// Датум и време на креирање на записот во база
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Датум и време на кога записот бил изменет во база
        /// </summary>
        DateTime ModifiedOn { get; set; }
    }
}

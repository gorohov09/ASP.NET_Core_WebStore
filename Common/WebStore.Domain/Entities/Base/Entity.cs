using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key] //Свойство - первичный ключ
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //База данных сама генерирует уникальные значения
        public int Id { get; set; }
    }
}

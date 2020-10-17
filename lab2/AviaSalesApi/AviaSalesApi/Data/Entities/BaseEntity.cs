using System;

namespace AviaSalesApi.Data.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
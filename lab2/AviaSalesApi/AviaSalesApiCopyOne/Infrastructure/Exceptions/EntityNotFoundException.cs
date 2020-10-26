using System;

namespace AviaSalesApiCopyOne.Infrastructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) {}
        public EntityNotFoundException(Type entityType, Guid id) : 
            base($"Entity of type { entityType.Name } with id: { id } not found") {}
        public EntityNotFoundException(Type entityType) : base($"Entity of type { entityType.Name } not found") {}
        public static EntityNotFoundException OfType<T>(Guid id) => new EntityNotFoundException(typeof(T), id);
        public static EntityNotFoundException OfType<T>() => new EntityNotFoundException(typeof(T));
    }
}
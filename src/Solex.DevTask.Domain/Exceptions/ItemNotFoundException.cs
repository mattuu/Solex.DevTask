using System;

namespace Solex.DevTask.Domain.Exceptions
{
    public class ItemNotFoundException : ApplicationException
    {
        public ItemNotFoundException()
        {
        }

        public ItemNotFoundException(string type, int id)
            : base($"Entity of type {type} with ID {id} does not exist")
        {
        }
    }
}
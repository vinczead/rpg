using System;
using System.Collections.Generic;
using System.Text;
using WorldEditor.DataAccess;

namespace WorldEditor.Utility
{
    public class EntityEventArgs<T> : EventArgs
    {
        public EntityEventArgs(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; set; }
    }
}

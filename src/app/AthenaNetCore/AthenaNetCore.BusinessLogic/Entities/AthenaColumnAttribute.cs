using System;
using System.Collections.Generic;
using System.Text;

namespace AthenaNetCore.BusinessLogic.Entities
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class AthenaColumnAttribute : Attribute
    {

        /// <summary>
        /// Map Amazon Athena column name to a proprierty
        /// </summary>
        /// <param name="columnName">Column name on Amazon Athena</param>
        public AthenaColumnAttribute(string columnName)
        {
            this.ColumnName = columnName?.ToLower();
        }

        public string ColumnName { get; }

    }
}

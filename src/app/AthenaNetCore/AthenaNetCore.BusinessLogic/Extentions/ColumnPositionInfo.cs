using Amazon.Athena.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AthenaNetCore.BusinessLogic.Extentions
{
    internal class ColumnPositionInfo
    {
        public int IndexPosition { get; set; }
        public ColumnInfo ColumnInfo { get; set; }
    }
}

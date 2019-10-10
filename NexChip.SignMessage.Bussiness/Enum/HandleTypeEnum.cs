using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    enum HandleTypeEnum
    {
        Add = 1,
        Completed = 2,
        Del = 3,
        CompletedWithOID = 4 //加上signboxid的更新
    }
}

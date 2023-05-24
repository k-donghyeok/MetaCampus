using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MultiUseKey : DoorKey
{
    private void Awake()
    {
        lockTypeID = IHaveLockID.TypeID.MultiUse;
    }

}
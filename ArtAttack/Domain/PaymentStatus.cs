using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    public enum PaymentStatus
    {
        SUCCESS=0,
        FAILED_INSUFFICIENT_FUNDS=1,
        FAILED_INVALID_CARD=2,
        FAILED_GATEWAY_ERROR=3
    }
}

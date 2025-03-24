﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    public class CardPaymentDetails
    {
        required public string cardholderName { get; set; }
        required public string cardNumber { get; set; }
        required public string month { get; set; }
        required public string year { get; set; }
        required public string cvc { get; set; }
        required public string country { get; set; }

    }
}

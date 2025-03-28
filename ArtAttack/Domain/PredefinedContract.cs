﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.Domain
{
    public class PredefinedContract
    {
        public int ID { get; set; }
        public required string Content { get; set; }
    }

    public enum PredefinedContractType
    {
        Buying = 1,
        Selling = 2,
        Borrowing = 3
    }
}

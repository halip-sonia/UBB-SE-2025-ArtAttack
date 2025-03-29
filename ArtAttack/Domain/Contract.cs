using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ArtAttack.Domain
{
    public class Contract
    {
        // Unique identifier for the contract (Identity field in the database)
        public long ID { get; set; }

        // Foreign key linking to the Order table (assumed to be of type integer)
        public int OrderID { get; set; }

        // The status of the contract.
        // Valid values: "ACTIVE", "RENEWED", "EXPIRED"
        public string ContractStatus { get; set; }

        // The full content/text of the contract
        public string ContractContent { get; set; }

        // The count of how many times this contract has been renewed
        public int RenewalCount { get; set; }

        // Optional foreign key to the PredefinedContract table
        public int? PredefinedContractID { get; set; }

        // Foreign key to the PDF table (holds the contract's PDF reference)
        public int PDFID { get; set; }

        // Holds the ID of the original contract if this is a renewal; null otherwise
        public long? RenewedFromContractID { get; set; }

        // Returns a user-friendly name for display purposes in the UI
        public string DisplayName => $"Contract {ID}";

    }
}
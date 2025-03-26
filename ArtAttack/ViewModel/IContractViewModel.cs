using ArtAttack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAttack.ViewModel
{
    internal interface IContractViewModel
    {
        Contract GetContractById(long contractId);
        List<Contract> GetAllContracts();
        List<Contract> GetContractHistory(long contractId);
        void AddContract(Contract contract, byte[] pdfFile);
        (int SellerID, string SellerName) GetContractSeller(long contractId);
        (int BuyerID, string BuyerName) GetContractBuyer(long contractId);
        Dictionary<string, object> GetOrderSummaryInformation(long contractId);
        (DateTime StartDate, DateTime EndDate)? GetProductDatesByContractId(long contractId);
        List<Contract> GetContractsByBuyer(int buyerId);
        byte[] GenerateContractPdf(Contract contract, PredefinedContract predefinedContract, Dictionary<string, string> fieldReplacements);

        public void GenerateAndSaveContract(Contract contract);
    }
}

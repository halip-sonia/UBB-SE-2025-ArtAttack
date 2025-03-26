using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using ArtAttack.Domain;
using ArtAttack.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ArtAttack.ViewModel
{
    public class ContractViewModel : IContractViewModel
    {
        private readonly ContractModel _model;

        public ContractViewModel(string connectionString)
        {
            _model = new ContractModel(connectionString);
        }

        public Contract GetContractById(long contractId)
        {
            return _model.GetContractById(contractId);
        }

        public List<Contract> GetAllContracts()
        {
            return _model.GetAllContracts();
        }

        public List<Contract> GetContractHistory(long contractId)
        {
            return _model.GetContractHistory(contractId);
        }

        public void AddContract(Contract contract, byte[] pdfFile)
        {
            _model.AddContract(contract, pdfFile);
        }

        public (int SellerID, string SellerName) GetContractSeller(long contractId)
        {
            return _model.GetContractSeller(contractId);
        }

        public (int BuyerID, string BuyerName) GetContractBuyer(long contractId)
        {
            return _model.GetContractBuyer(contractId);
        }

        public Dictionary<string, object> GetOrderSummaryInformation(long contractId)
        {
            return _model.GetOrderSummaryInformation(contractId);
        }

        public (DateTime StartDate, DateTime EndDate)? GetProductDatesByContractId(long contractId)
        {
            return _model.GetProductDatesByContractId(contractId);
        }

        public List<Contract> GetContractsByBuyer(int buyerId)
        {
            return _model.GetContractsByBuyer(buyerId);
        }

        public byte[] GenerateContractPdf(Contract contract, PredefinedContract predefinedContract, Dictionary<string, string> fieldReplacements)
        {
            string content = predefinedContract.Content;

            foreach (var pair in fieldReplacements)
            {
                content = content.Replace("{" + pair.Key + "}", pair.Value);
            }
            
            var productDates = GetProductDatesByContractId(contract.ID);
            if (productDates.HasValue)
            {
                content = content.Replace("{StartDate}", productDates.Value.StartDate.ToShortDateString());
                content = content.Replace("{EndDate}", productDates.Value.EndDate.ToShortDateString());
            }
            else
            {
                content = content.Replace("{StartDate}", "N/A");
                content = content.Replace("{EndDate}", "N/A");
            }

            content = content.Replace("{ContractID}", contract.ID.ToString());
            content = content.Replace("{OrderID}", contract.OrderID.ToString());
            content = content.Replace("{ContractStatus}", contract.ContractStatus);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Content().Text(content);
                });
            });

            return document.GeneratePdf();
        }

        public void GenerateAndSaveContract(Contract contract)
        {
            // For this example, assume a predefined contract of type Buying.
            var predefinedContract = new PredefinedContract
            {
                ID = (int)PredefinedContractType.Buying,
                Content = "Contract for {ContractID} with Order {OrderID}.\nStart: {StartDate}, End: {EndDate}.\nStatus: {ContractStatus}"
            };

            // You can add extra field replacements if needed.
            var fieldReplacements = new Dictionary<string, string>();

            // Generate the PDF.
            var pdfBytes = GenerateContractPdf(contract, predefinedContract, fieldReplacements);

            // Determine the Downloads folder path.
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = $"Contract_{contract.ID}.pdf";
            string filePath = Path.Combine(downloadsPath, fileName);

            // Save the PDF to the file.
            File.WriteAllBytes(filePath, pdfBytes);
        }

     
    }
}


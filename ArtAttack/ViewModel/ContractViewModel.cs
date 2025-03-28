using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        public async Task<Contract> GetContractByIdAsync(long contractId)
        {
            return await _model.GetContractByIdAsync(contractId);
        }

        public async Task<List<Contract>> GetAllContractsAsync()
        {
            return await _model.GetAllContractsAsync();
        }

        public async Task<List<Contract>> GetContractHistoryAsync(long contractId)
        {
            return await _model.GetContractHistoryAsync(contractId);
        }

        public async Task AddContractAsync(Contract contract, byte[] pdfFile)
        {
            await _model.AddContractAsync(contract, pdfFile);
        }

        public async Task<(int SellerID, string SellerName)> GetContractSellerAsync(long contractId)
        {
            return await _model.GetContractSellerAsync(contractId);
        }

        public async Task<(int BuyerID, string BuyerName)> GetContractBuyerAsync(long contractId)
        {
            return await _model.GetContractBuyerAsync(contractId);
        }

        public async Task<Dictionary<string, object>> GetOrderSummaryInformationAsync(long contractId)
        {
            return await _model.GetOrderSummaryInformationAsync(contractId);
        }

        public async Task<(DateTime StartDate, DateTime EndDate)?> GetProductDatesByContractIdAsync(long contractId)
        {
            return await _model.GetProductDatesByContractIdAsync(contractId);
        }

        public async Task<List<Contract>> GetContractsByBuyerAsync(int buyerId)
        {
            return await _model.GetContractsByBuyerAsync(buyerId);
        }

        public byte[] GenerateContractPdf(Contract contract, PredefinedContract predefinedContract, Dictionary<string, string> fieldReplacements)
        {
            // This part remains synchronous as QuestPDF's GeneratePdf() method is synchronous.
            string content = predefinedContract.Content;
            foreach (var pair in fieldReplacements)
            {
                content = content.Replace("{" + pair.Key + "}", pair.Value);
            }

            // Note: We use the asynchronous GetProductDatesByContractIdAsync in our calling method.
            // The PDF generation itself is synchronous.
            // Placeholders for product dates are assumed to be replaced externally.
            content = content.Replace("{ContractID}", contract.ID.ToString());
            content = content.Replace("{OrderID}", contract.OrderID.ToString());
            content = content.Replace("{ContractStatus}", contract.ContractStatus);
            QuestPDF.Settings.License = LicenseType.Community;
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

        public async Task GenerateAndSaveContractAsync(Contract contract)
        {
            // For this example, assume a predefined contract of type Buying.
            var predefinedContract = new PredefinedContract
            {
                ID = (int)PredefinedContractType.Buying,
                Content = "Contract for {ContractID} with Order {OrderID}.\nStart: {StartDate}, End: {EndDate}.\nStatus: {ContractStatus}"
            };

            var fieldReplacements = new Dictionary<string, string>();

            // Retrieve the product dates asynchronously.
            var productDates = await GetProductDatesByContractIdAsync(contract.ID);
            if (productDates.HasValue)
            {
                fieldReplacements["StartDate"] = productDates.Value.StartDate.ToShortDateString();
                fieldReplacements["EndDate"] = productDates.Value.EndDate.ToShortDateString();
            }
            else
            {
                fieldReplacements["StartDate"] = "N/A";
                fieldReplacements["EndDate"] = "N/A";
            }

            // Generate the PDF (synchronously) using the generated replacements.
            var pdfBytes = GenerateContractPdf(contract, predefinedContract, fieldReplacements);

            // Determine the Downloads folder path.
            string downloadsPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = $"Contract_{contract.ID}.pdf";
            string filePath = System.IO.Path.Combine(downloadsPath, fileName);

            // Save the PDF file asynchronously.
            await File.WriteAllBytesAsync(filePath, pdfBytes);
        }
    }
}

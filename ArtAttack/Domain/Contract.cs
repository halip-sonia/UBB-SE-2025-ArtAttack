using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Windows.Data.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Numerics;
using Windows.ApplicationModel.Activation;

namespace ArtAttack.Domain
{
	class Contract
	{
		private int _orderID { get; set; }
		private DateTime _startDate { get; set; }
        private DateTime _endDate { get; set; }
        private ContractStatus _status { get; set; }
        private string _content { get; set; }
        private int _renewalCount { get; set; }
        private int _predefinedContractID { get; set; }
        private int _pdfID { get; set; }
        //public Contract(){ }
	}

	enum ContractStatus
	{
		Active,
		Renewed,
		Expired
	}
}
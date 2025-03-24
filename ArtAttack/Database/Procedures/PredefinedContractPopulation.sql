INSERT INTO PredefinedContract (content)
VALUES (
'Product Purchase Agreement
This Agreement is made effective as of {AgreementDate} by and between {SellerName} (Seller) and {BuyerName} (Buyer).
WHEREAS, the Seller agrees to sell, and the Buyer agrees to purchase, the product described as {ProductDescription}.
The purchase price is {Price} to be paid via {PaymentMethod}. The product shall be delivered on or before {DeliveryDate}.
Additional terms: {AdditionalTerms}.
IN WITNESS WHEREOF, the parties have executed this Agreement on the date first written above.'
);

-- Predefined Contract for Selling a Product
INSERT INTO PredefinedContract (content)
VALUES (
'Sales Agreement
This Agreement is entered into on {AgreementDate} by and between {SellerName} (Seller) and {BuyerName} (Buyer).
The Seller agrees to sell, and the Buyer agrees to purchase, the product identified as {ProductDescription} for the sum of {Price}.
Payment shall be made by {PaymentMethod} with delivery scheduled for {DeliveryDate}.
Additional provisions: {AdditionalTerms}.
The parties have executed this Agreement as of the date stated above.'
);

-- Predefined Contract for Borrowing a Product
INSERT INTO PredefinedContract (content)
VALUES (
'Product Loan Agreement
This Agreement is made on {AgreementDate} between {LenderName} (Lender) and {BorrowerName} (Borrower).
The Borrower shall borrow the product described as {ProductDescription} for a period of {LoanPeriod} days, with a due date of {DueDate}.
The Borrower agrees to maintain the product in good condition and will incur a late fee of {LateFee} per day for overdue returns.
Additional terms: {AdditionalTerms}.
Both parties agree to the terms set forth in this Agreement as of the date first above written.'
);
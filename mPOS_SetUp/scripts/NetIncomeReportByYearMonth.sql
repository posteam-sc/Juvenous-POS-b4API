create proc NetIncomeReportByYearMonth
@Year int, 
@Month int
as
begin
select
Product.Id,
PurchaseDetail.UnitPrice  as Price,
StockTransaction.Purchase as Purchase,
StockTransaction.Refund as Refund,
StockTransaction.Sale as Sale,
StockTransaction.AdjustmentStockIn as AdjustmentStockIn,
StockTransaction.AdjustmentStockOut,
StockTransaction.Consignment,
StockTransaction.ConversionStockIn,
StockTransaction.ConversionStockOut,
StockTransaction.StockIn,
StockTransaction.StockOut,
StockTransaction.Opening,
(StockTransaction.Opening+StockTransaction.Purchase+StockTransaction.Refund +StockTransaction.AdjustmentStockIn+StockTransaction.Consignment+StockTransaction.ConversionStockIn+StockTransaction.StockIn)-(StockTransaction.Sale+StockTransaction.AdjustmentStockOut+StockTransaction.ConversionStockOut+StockTransaction.StockOut) as Closing
 from Product 
left join StockTransaction on StockTransaction.ProductId=Product.Id
left join PurchaseDetail on PurchaseDetail.ProductId=Product.Id
where StockTransaction.Month=@Month and StockTransaction.Year=@Year and Month(PurchaseDetail.Date)=@Month and YEAR(PurchaseDetail.Date)=@Year and PurchaseDetail.IsDeleted=0
union  all
select 
Product.Id,
0 as Price,
isnull(0,0) as Purchase,
isnull(0,0) as Refund,
isnull(0,0) as Sale,
isnull(0,0) as AdjustmentStockIn,
isnull(0,0)as AdjustmentStockOut,
isnull(0,0)as Consignment,
isnull(0,0)as ConversionStockIn,
isnull(0,0)as ConversionStockOut,
isnull(0,0) as StockIn,
isnull(0,0)as StockOut,
isnull(product.Qty,0) as Opening,
0 as Closing
 from Product 
where Product.Id not in (select ProductId from StockTransaction where StockTransaction.Month=@Month and StockTransaction.Year=@Year)
end
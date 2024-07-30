alter proc StockTransactionReport
@Year int, 
@Month int
as
begin
select Product.Id as ProductID,
Product.Name as ProductName,
Product.ProductCode as ProductCode,
StockTransaction.Purchase as Purchase,
StockTransaction.Refund as Refund,
StockTransaction.Sale as Sale,
StockTransaction.AdjustmentStockIn as AdjustmentStockIn,
StockTransaction.AdjustmentStockOut,
StockTransaction.Consignment,
StockTransaction.ConversionStockIn,
StockTransaction.ConversionStockOut,
StockTransaction.StockIn,
StockTransaction.StockOut,StockTransaction.Opening
 from Product 
left join StockTransaction on StockTransaction.ProductId=Product.Id
where StockTransaction.Month=@Month and StockTransaction.Year=@Year
union  all
select Product.Id as ProductID,
Product.Name as ProductName,
Product.ProductCode as ProductCode,
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
isnull(product.Qty,0) as Opening
 from Product 
where Product.Id not in (select ProductId from StockTransaction where StockTransaction.Month=@Month and StockTransaction.Year=@Year)
order by Product.ProductCode
end
using Dapper;
using practiceAss3_4Tables.Model;
using practiceAss3_4Tables.Repository.Interface;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using static practiceAss3_4Tables.Model.BaseModel;

namespace practiceAss3_4Tables.Repository
{
    public class BigBazarRepository:BaseAsyncRepository,IBigBazarRepository
    {
        IConfiguration configuration;
        public BigBazarRepository(IConfiguration configuration):base(configuration)
        {
            this.configuration = configuration;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            List< ProductModel> products = new List<ProductModel>();
            var query = "select * from Ass3_Product";
            using(DbConnection dbConnection = sqlwriterConnection)
            {
                var result = await dbConnection.QueryAsync<ProductModel>(query);
                products = result.ToList();
            }
            return products;
        }
        public async Task<List<DiscountModel>> GetAllDiscount()
        {
            List<DiscountModel> discounts = new List<DiscountModel>();
            var query = "select * from Ass3_Discount";
            using (DbConnection dbConnection = sqlwriterConnection)
            {
                var result = await dbConnection.QueryAsync<DiscountModel>(query);
                discounts = result.ToList();
            }
            return discounts;
        }

        public async Task<List<OrderModel>> GetAllOrders()
        {
            List<OrderModel> orders = new List<OrderModel>();
            var query = "select * from Ass3_TrnOrder";
            using (DbConnection dbConnection = sqlwriterConnection)
            {
                var result = await dbConnection.QueryAsync<OrderModel>(query);
                orders = result.ToList();
                foreach(var ord in orders)
                {
                    var result1 = await dbConnection.QueryAsync<OrderDetailsModel>("select * from Ass3_TrnOrdeDetails where Id=@Id", new {Id=ord.Id});
                    ord.orderDetails = result1.ToList();
                }               
            }
            return orders;
        }

        public async Task<List<OrderModel>> GetorderbyId(int Id)
        {
            var query = @"select * from Ass3_TrnOrder where Id=@Id and isDeleted=0";
            using (var con = sqlwriterConnection)
            {
                var result = await con.QueryAsync<OrderModel>(query, new { Id });

                return result.ToList();

            }

        }

        public async Task<int> Add(OrderModel orderModel)
        {
            var query = @"insert into Ass3_TrnOrder
                        (OrderCode,OrderDate,SubTotal,TotalDiscount,GrandTotal,Remark,BillingAddress,ShippingAddress,CreatedBy,CreatedDate,IsDeleted,ModifiedBy,ModifiedDate)
                        values
                        (@OrderCode,@OrderDate,@SubTotal,@TotalDiscount,@GrandTotal,@Remark,@BillingAddress,@ShippingAddress,@CreatedBy,GETDATE(),0,@ModifiedBy,GETDATE())
                        SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";
            List<OrderDetailsModel> ord = new List<OrderDetailsModel>();
            ord = orderModel.orderDetails.ToList();
            using (DbConnection dbConnection = sqlwriterConnection)
            {                
                    int result = await dbConnection.QuerySingleAsync<int>(query, orderModel);
                    foreach (var orderss in orderModel.orderDetails)
                    {
                        orderss.Id= result;
                        int result1 = await dbConnection.ExecuteAsync(@"insert into Ass3_TrnOrdeDetails
                        (OrderId,ProductId,DiscountId,Quantity,Rate,Amount,Remark,DiscountAmount,NetAmount,CreatedBy,CreatedDate,IsDeleted,ModifiedBy,ModifiedDate)
                        values
                        (@OrderId,@ProductId,@DiscountId,@Quantity,@Rate,@Amount,@Remark,@DiscountAmount,@NetAmount,@CreatedBy,GetDate(),0,@ModifiedBy,GetDate());", orderss);
                    }
                    return Convert.ToInt32(result);
                
            }
        }

        public async Task<int> Update(OrderModel orderModel)
        {
            var query = @"update Ass3_TrnOrder set
                        OrderCode=@OrderCode,OrderDate=@OrderDate,SubTotal=@SubTotal,TotalDiscount=@TotalDiscount,
                        GrandTotal=@GrandTotal,Remark=@Remark,BillingAddress=@BillingAddress,ShippingAddress=@ShippingAddress,
                        CreatedBy=@CreatedBy,CreatedDate=GETDATE(),IsDeleted=0,ModifiedBy=@ModifiedBy,ModifiedDate=GETDATE()
                        where Id=@Id";



            using (DbConnection connection=sqlwriterConnection)
            {
                {
                    int result = await connection.ExecuteAsync(query, orderModel);
                    foreach (var ords in orderModel.orderDetails)
                    {
                        int result1 = await connection.ExecuteAsync(@"update Ass3_TrnOrdeDetails set OrderId=@OrderId,ProductId=@ProductId,DiscountId=@DiscountId,Quantity=@Quantity,Rate=@Rate,
                        Amount=@Amount,Remark=@Remark,DiscountAmount=@DiscountAmount,NetAmount=@NetAmount,CreatedBy=@CreatedBy,
                        CreatedDate=GetDate(),IsDeleted=0,ModifiedBy=@ModifiedBy,ModifiedDate=GetDate() where Id=@Id", ords);
                    }
                    return result;
                }
            }
        }

        public async Task<long> Delete(DeleteObj dobj)
        {
            /*int result = 0;
            var query = @"UPDATE Ass3_TrnOrder 
                        SET IsDeleted = 1, ModifiedBy = @ModifiedBy, ModifiedDate = GETDATE() 
                        WHERE Id = @Id";
            using (DbConnection dbConnection = sqlreaderConnection)
            {
                result = await dbConnection.ExecuteAsync(query, dobj);
                if (result != 0)
                {
                    var result1 = await dbConnection.ExecuteAsync(@"UPDATE Ass3_TrnOrdeDetails 
                        SET IsDeleted = 1, ModifiedBy = @ModifiedBy, ModifiedDate = GETDATE()
                        WHERE Id = @Id",dobj);
                }
                return result;
            } */
            int result = 0;
            var query = @"select top(1)* from Ass3_TrnOrder where Id=@Id and isdeleted=0";
            using (DbConnection dbConnection = sqlreaderConnection)
            {
                result = await dbConnection.ExecuteAsync(query, dobj);
                if (result != 0)
                {
                    var result1 = await dbConnection.ExecuteAsync(@"UPDATE Ass3_TrnOrdeDetails 
                        SET IsDeleted = 1, ModifiedBy = @ModifiedBy, ModifiedDate = GETDATE()
                        WHERE Id = @Id", dobj);
                }       
        
                return result;
            }
        }
    }
}

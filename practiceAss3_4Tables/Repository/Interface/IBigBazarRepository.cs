using practiceAss3_4Tables.Model;
using static practiceAss3_4Tables.Model.BaseModel;

namespace practiceAss3_4Tables.Repository.Interface
{
    public interface IBigBazarRepository
    {
        public Task<List<ProductModel>> GetAllProducts();
        public Task<List<DiscountModel>> GetAllDiscount();
        public Task<List<OrderModel>> GetAllOrders();
        public Task<List<OrderModel>> GetorderbyId(int Id);
        public Task<int> Add(OrderModel orderModel);
        public Task<int> Update(OrderModel orderModel);
        public Task<long> Delete(DeleteObj dobj);
    }
}

using DAL.Data;

public interface IOrderRepository{
    List<Order> getAllorders();
    List<Order>? getAllordersBySearch(string searchedOrder);
    List<Order> getAllOrdersFromStatus(int statusid);
    List<Orderstatus> getAllStatus();
}
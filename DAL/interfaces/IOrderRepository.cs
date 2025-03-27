using DAL.Data;

public interface IOrderRepository{
    List<Order> getAllOrderByDateFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
    List<Order> getAllOrderByOptionFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
    List<Order> getAllorders();
    List<Order> getAllordersBySearch(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
    List<Order> getAllOrdersFromStatus(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate);
    List<Orderstatus> getAllStatus();
    List<Order>? GetAllOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate);
}
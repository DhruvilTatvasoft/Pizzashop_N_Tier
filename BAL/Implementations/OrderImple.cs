using BAL.Interfaces;
using DAL.Data;

namespace BAL.Implementations
{
    public class OrderImple : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderImple(IOrderRepository orderRepository){
            _orderRepository = orderRepository;
        }

        public List<Order> getAllOrderByOptionFilter(string filterBy)
        {
          
            return _orderRepository.getAllOrderByOptionFilter(filterBy);
          
        }

        public List<Order> getAllOrders()
        {
            return _orderRepository.getAllorders();
        }

        public List<Order> getAllOrdersBySearch(string searchedOrder)
        {
            return _orderRepository.getAllordersBySearch(searchedOrder);
        }

        public List<Order> getAllOrdersByStatus(int statusid)
        {
            return _orderRepository.getAllOrdersFromStatus(statusid);
        }

        public List<Orderstatus> getAllStatus()
        {
            return _orderRepository.getAllStatus();
        }
        
    }
}
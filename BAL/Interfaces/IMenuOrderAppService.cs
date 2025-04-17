using DAL.Data;

public interface IMenuOrderAppService
{
    List<Item> getItemsForcategory(int categoryid);
}
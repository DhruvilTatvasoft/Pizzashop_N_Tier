using DAL.Data;

public interface IMenuOrderAppRepository
{
    List<Item> getItemsForcategory(int categoryid);
}
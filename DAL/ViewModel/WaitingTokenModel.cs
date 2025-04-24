using System.ComponentModel.DataAnnotations;
using DAL.Data;

public class WaitingTokenModel
{
    public int tokenId{set;get;}
    public DateTime createdAt{set;get;}
    public List<Section> sections{get;set;}

    public int personCount{get;set;}

    public int sectionId{get;set;}
    public Dictionary<Section,int> Sections{get;set;}
    public List<Waitingtoken> waitingTokens{get;set;}

    public List<Table> tables{get;set;}
    public int pageNumber{get;set;}
    public int pageSize{get;set;}
    public int TotalWaitingTokens{get;set;}

    public List<CustomerModel> customerList{get;set;}

    public CustomerModel customer{get;set;}
}
public class CustomerModel{
    public int customerId{get;set;}
    
    [Required(ErrorMessage = "Customer Name is required")]
    [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "please Use only letters")]
    public string name{get;set;}

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string phone{get;set;}

    [Required(ErrorMessage ="Email is required")]
    [EmailAddress(ErrorMessage ="Invalid email address")]
    public string email{get;set;}

    public int PersonCount{get;set;}
    public Section section{get;set;}

    public Table table{get;set;}

    public List<Table> tables{get;set;}

    public int tokenid{get;set;}

    public List<int> tableids{get;set;}

    public List<Section> sections{get;set;}

   

} 
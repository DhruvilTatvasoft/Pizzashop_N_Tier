
public interface ICookieService{
    public CookieOptions setCookieOption();
    public void setInCookie(string token,HttpResponse response,string name);
    bool IsSetCookie(HttpRequest request,string name);
    string getValueFromCookie(string name,HttpRequest request);
}
public class CookieService : ICookieService
{
    public CookieOptions setCookieOption()
    {
        CookieOptions options = new CookieOptions
        {
            HttpOnly = true,
            MaxAge = TimeSpan.FromHours(1),
            IsEssential = true
        };
        return options;
    }
    public bool IsSetCookie(HttpRequest request,string name){
        if(request.Cookies[name] != null){
            return true;
        }
        else{
            return false;
        }
    }

    public void setInCookie(string token,HttpResponse response,string name)
    {
       response.Cookies.Append(name, token, setCookieOption());
    }

    public string getValueFromCookie(string name,HttpRequest request)
    {
       return request.Cookies[name];
    }
}
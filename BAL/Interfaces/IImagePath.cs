using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;

public interface IImagePath{
    string getImagePath(IFormFile file);

    string getImagePath(int userid);
}
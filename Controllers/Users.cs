using Microsoft.AspNetCore.Mvc;
using APIKvihaugenEngine.Classes;

namespace APIKvihaugenEngine.Controllers;

[ApiController]
[Route("[controller]/{path}")]
public class UsersController : ControllerBase{
    private readonly ILogger<UsersController> _logger;

    public UsersController(ILogger<UsersController> logger){
        _logger = logger;
    }

    [HttpGet]
    public Person Hehehe(){
        return new Person{
            Path = "GET",
            Name = "Lars Kvihaugen",
            Age = 18,
            Email = "lars@kvihaugen.no"
        };
    }

    [HttpPost]
    public Person Post(){
        return new Person{
            Path = "POST",
            Name = "Lars Kvihaugen",
            Age = 18,
            Email = "lars@kvihaugen.no"
        };
    }
}
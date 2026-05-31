using HttpClientExample;
using System.Net.Http.Json;
using System.Text.Json;

await GelAllUsers();
await UpdateUser();
await CreateUser();
await GelAllUsers();
await DeleteUser();

Console.WriteLine();

await GelAllUsers();

Console.ReadLine();

async Task DeleteUser()
{
    var httpClient = new HttpClient();
    var responce = await httpClient.DeleteAsync($"https://localhost:7167/users/{1}");
}

async Task UpdateUser()
{
    var firstName = Console.ReadLine();
    var lastName = Console.ReadLine();

    var user = new User
    {
        Id = 1,
        FirstName = firstName,
        LastName = lastName
    };

    var httpClient = new HttpClient();
    var responce = await httpClient.PutAsJsonAsync($"https://localhost:7167/users/{user.Id}", user);
}

async Task CreateUser()
{
    var firstName = Console.ReadLine();
    var lastName = Console.ReadLine();

    var user = new User
    {
        FirstName = firstName,
        LastName = lastName
    };

    var httpClient = new HttpClient();
    var responce = await httpClient.PostAsJsonAsync("https://localhost:7167/users", user);
}

async Task GelAllUsers()
{
    var httpClient = new HttpClient();
    var responce = await httpClient.GetAsync("https://localhost:7167/users");
    var content = await responce.Content.ReadAsStringAsync();
    var users = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

    foreach (var user in users)
    {
        Console.WriteLine($"Id: {user.Id}, FirstName: {user.FirstName}, LastName: {user.LastName}");
    }
}

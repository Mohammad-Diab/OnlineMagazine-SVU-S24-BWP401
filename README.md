# OnlineMagazine

An online magazine website in Arabic (right to left), made for the Web Programming I course (BWP401) at the Syrian Virtual University (SVU), semester S24. An ASP.NET Core 6 Web API in C# with SQL Server serves the site, a static HTML, CSS and JavaScript front end in `API\wwwroot`.

## Features

- Home page with the newest articles and the top 5 articles.
- Articles list and an article page with its paragraphs and images.
- About page with a contact form; the messages are stored in the database.
- Admin login, a manager page to add and delete articles (with paragraphs and images), and a messages page to read and delete the contact messages.

## Requirements

- The [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0), and IIS with the ASP.NET Core 6 Hosting Bundle to host it.
- SQL Server with a database named `BWP401S24` (tables `TBL_ARTICLES`, `TBL_PARAGRAPHS`, `TBL_CONTACT_US`). The database scripts are not in this repository.

## Building

```
git clone https://github.com/Mohammad-Diab/OnlineMagazine-SVU-S24-BWP401.git
```

Open `OnlineMagazine.sln` in Visual Studio, or build the API project:

```
dotnet build API\Api.csproj -c Release
```

## Configuration

- `API\config.json`: the database server, name, user and password (the password is stored encrypted, see `Authentication.Encrypt` in `API\BasicAuth\Authentication.cs`), or a full `connectionString`; the admin user name and password hash (see `SecurePassword.Hash` in `API\Logic\SecurePassword.cs`); and the folder for uploaded images.
- The front end calls the API at `http://localhost/API/`, so the site expects to run as the IIS application `API` on localhost (the **IIS** launch profile in Visual Studio). To host it elsewhere, change `ServerUrl` in `API\wwwroot\script\common.js` and `serverUrl` in `API\config.json`.

## Credits

- [Tajawal](https://fonts.google.com/specimen/Tajawal) font by Boutros International (`API\wwwroot\assets\fonts`), SIL Open Font License 1.1; see `OFL.txt` in that folder.
- [Bootstrap](https://getbootstrap.com) 5.2.3, [Popper](https://popper.js.org) 2.11.6, [jQuery](https://jquery.com) 3.7.1 and [Animate.css](https://animate.style) 4.1.1, loaded from CDNs.

## Author

Mohammad Diab

## License

[Apache 2.0](LICENSE), except the Tajawal font files, which keep the SIL Open Font License.

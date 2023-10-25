using lab3;
using lab3.Services;
using lab3.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;

        string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
        services.AddDbContext<TouristAgency1Context>(options => options.UseSqlServer(connection));



        services.AddMemoryCache();

        services.AddDistributedMemoryCache();
        services.AddScoped<CachedAgencyDb>();
        services.AddSession();

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();

        var app = builder.Build();
        app.UseSession();


        app.Map("/info", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {

                string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Информация:</H1>";
                strResponse += "<BR> Сервер: " + context.Request.Host;
                strResponse += "<BR> Путь: " + context.Request.PathBase;
                strResponse += "<BR> Протокол: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/client", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedAgencyDb cachedAgencyDb = context.RequestServices.GetService<CachedAgencyDb>();

                IEnumerable<Client> clients = cachedAgencyDb.GetClient("client");

                string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>ФИО</TH>";
                HtmlString += "<TH>Дата рождения</TH>";
                HtmlString += "<TH>Пол</TH>";
                HtmlString += "<TH>Адрес</TH>";
                HtmlString += "<TH>Серия паспорта</TH>";
                HtmlString += "<TH>Номер паспорта</TH>";
                HtmlString += "<TH>Скидка</TH>";
                HtmlString += "</TR>";
                foreach (var client in clients)
                {
                    HtmlString += "<TR>";
                    HtmlString += "<TD>" + client.Id + "</TD>";
                    HtmlString += "<TD>" + client.Fio + "</TD>";
                    HtmlString += "<TD>" + client.DateOfBirth + "</TD>";
                    HtmlString += "<TD>" + client.Sex + "</TD>";
                    HtmlString += "<TD>" + client.Address + "</TD>";
                    HtmlString += "<TD>" + client.Series + "</TD>";
                    HtmlString += "<TD>" + client.Number + "</TD>";
                    HtmlString += "<TD>" + client.Discount + "</TD>";
                    HtmlString += "</TR>";
                }
                HtmlString += "</TABLE>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "</BODY></HTML>";

                await context.Response.WriteAsync(HtmlString);
            });
        });

        app.Map("/searchClientDiscount", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedAgencyDb cachedAgencyDb = context.RequestServices.GetService<CachedAgencyDb>();
                IEnumerable<Client> clients = cachedAgencyDb.GetClient("client");

                string formHtml = "<form method='post' action='/searchClientDiscount'>" +
                                  "<label>Скидка:</label>";



                if (context.Request.Cookies.TryGetValue("discount", out var input_value))
                {
                    formHtml += $"<input type='number' name='discount' value='{input_value}'><br><br>" +
                               "<input type='submit' value='Поиск'>" +
                               "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='discount'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                "</form>";
                }


                if (context.Request.Method == "POST")
                {
                    var discount = long.Parse(context.Request.Form["discount"]);

                    context.Response.Cookies.Append("discount", discount.ToString());

                    IEnumerable<Client> byClientDiscount = clients.Where(s => s.Discount > discount);

                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>ФИО</TH>";
                    HtmlString += "<TH>Дата рождения</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Серия паспорта</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "<TH>Скидка</TH>";
                    HtmlString += "</TR>";
                    foreach (var client in byClientDiscount)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + client.Id + "</TD>";
                        HtmlString += "<TD>" + client.Fio + "</TD>";
                        HtmlString += "<TD>" + client.DateOfBirth + "</TD>";
                        HtmlString += "<TD>" + client.Sex + "</TD>";
                        HtmlString += "<TD>" + client.Address + "</TD>";
                        HtmlString += "<TD>" + client.Series + "</TD>";
                        HtmlString += "<TD>" + client.Number + "</TD>";
                        HtmlString += "<TD>" + client.Discount + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {
                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>ФИО</TH>";
                    HtmlString += "<TH>Дата рождения</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Серия паспорта</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "<TH>Скидка</TH>";
                    HtmlString += "</TR>";
                    foreach (var client in clients)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + client.Id + "</TD>";
                        HtmlString += "<TD>" + client.Fio + "</TD>";
                        HtmlString += "<TD>" + client.DateOfBirth + "</TD>";
                        HtmlString += "<TD>" + client.Sex + "</TD>";
                        HtmlString += "<TD>" + client.Address + "</TD>";
                        HtmlString += "<TD>" + client.Series + "</TD>";
                        HtmlString += "<TD>" + client.Number + "</TD>";
                        HtmlString += "<TD>" + client.Discount + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });

        
        app.Map("/searchClient", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedAgencyDb cachedAgencyDb = context.RequestServices.GetService<CachedAgencyDb>();
                IEnumerable<Client> clients = cachedAgencyDb.GetClient("clients");

                string formHtml = "<form method='post' action='/searchClient'>" +
                                    "<label>ФИО:</label>";


                if (context.Session.Keys.Contains("fio"))
                {
                    string fio = context.Session.GetString("fio");

                    formHtml += $"<input type='text' name='fio' value='{fio}'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                 "</form>";
                }
                else
                {
                    formHtml += "<input type='text' name='fio'><br><br>" +
                                "<input type='submit' value='Поиск'>" +
                                 "</form>";
                }

                if (context.Request.Method == "POST")
                {
                    string fio = context.Request.Form["fio"];

                    context.Session.SetString("fio", fio);

                    IEnumerable<Client> clientsByFIO = clients.Where(s => s.Fio == fio);

                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>ФИО</TH>";
                    HtmlString += "<TH>Дата рождения</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Серия паспорта</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "<TH>Скидка</TH>";
                    HtmlString += "</TR>";
                    foreach (var client in clientsByFIO)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + client.Id + "</TD>";
                        HtmlString += "<TD>" + client.Fio + "</TD>";
                        HtmlString += "<TD>" + client.DateOfBirth + "</TD>";
                        HtmlString += "<TD>" + client.Sex + "</TD>";
                        HtmlString += "<TD>" + client.Address + "</TD>";
                        HtmlString += "<TD>" + client.Series + "</TD>";
                        HtmlString += "<TD>" + client.Number + "</TD>";
                        HtmlString += "<TD>" + client.Discount + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {

                    string HtmlString = "<HTML><HEAD><TITLE>Подписки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>Список клиентов</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>ФИО</TH>";
                    HtmlString += "<TH>Дата рождения</TH>";
                    HtmlString += "<TH>Пол</TH>";
                    HtmlString += "<TH>Адрес</TH>";
                    HtmlString += "<TH>Серия паспорта</TH>";
                    HtmlString += "<TH>Номер паспорта</TH>";
                    HtmlString += "<TH>Скидка</TH>";
                    HtmlString += "</TR>";
                    foreach (var client in clients)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + client.Id + "</TD>";
                        HtmlString += "<TD>" + client.Fio + "</TD>";
                        HtmlString += "<TD>" + client.DateOfBirth + "</TD>";
                        HtmlString += "<TD>" + client.Sex + "</TD>";
                        HtmlString += "<TD>" + client.Address + "</TD>";
                        HtmlString += "<TD>" + client.Series + "</TD>";
                        HtmlString += "<TD>" + client.Number + "</TD>";
                        HtmlString += "<TD>" + client.Discount + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });
        
        app.Run((context) =>
        {

            CachedAgencyDb cachedAgencyDb = context.RequestServices.GetService<CachedAgencyDb>();

            cachedAgencyDb.GetClient("client");


            string HtmlString = "<HTML><HEAD><TITLE>Емкости</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>Главная</H1>";
            HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
            HtmlString += "<BR><A href='/'>Главная</A></BR>";
            HtmlString += "<BR><A href='/info'>Информация</A></BR>";
            HtmlString += "<BR><A href='/client'>Список клиентов</A></BR>";
            HtmlString += "<BR><A href='/searchClientDiscount'>Список отсортированный</A></BR>";
            HtmlString += "<BR><A href='/searchClient'>Поиск по ФИО</A></BR>";
            HtmlString += "</BODY></HTML>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}
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

                string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>����������:</H1>";
                strResponse += "<BR> ������: " + context.Request.Host;
                strResponse += "<BR> ����: " + context.Request.PathBase;
                strResponse += "<BR> ��������: " + context.Request.Protocol;
                strResponse += "<BR><A href='/'>�������</A></BODY></HTML>";

                await context.Response.WriteAsync(strResponse);
            });
        });

        app.Map("/client", (appBuilder) =>
        {
            appBuilder.Run(async (context) =>
            {
                CachedAgencyDb cachedAgencyDb = context.RequestServices.GetService<CachedAgencyDb>();

                IEnumerable<Client> clients = cachedAgencyDb.GetClient("client");

                string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                HtmlString += "<TR>";
                HtmlString += "<TH>ID</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "<TH>���� ��������</TH>";
                HtmlString += "<TH>���</TH>";
                HtmlString += "<TH>�����</TH>";
                HtmlString += "<TH>����� ��������</TH>";
                HtmlString += "<TH>����� ��������</TH>";
                HtmlString += "<TH>������</TH>";
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
                HtmlString += "<BR><A href='/'>�������</A></BR>";
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
                                  "<label>������:</label>";



                if (context.Request.Cookies.TryGetValue("discount", out var input_value))
                {
                    formHtml += $"<input type='number' name='discount' value='{input_value}'><br><br>" +
                               "<input type='submit' value='�����'>" +
                               "</form>";
                }
                else
                {
                    formHtml += "<input type='number' name='discount'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                "</form>";
                }


                if (context.Request.Method == "POST")
                {
                    var discount = long.Parse(context.Request.Form["discount"]);

                    context.Response.Cookies.Append("discount", discount.ToString());

                    IEnumerable<Client> byClientDiscount = clients.Where(s => s.Discount > discount);

                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ��������</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {
                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ��������</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
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
                                    "<label>���:</label>";


                if (context.Session.Keys.Contains("fio"))
                {
                    string fio = context.Session.GetString("fio");

                    formHtml += $"<input type='text' name='fio' value='{fio}'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                 "</form>";
                }
                else
                {
                    formHtml += "<input type='text' name='fio'><br><br>" +
                                "<input type='submit' value='�����'>" +
                                 "</form>";
                }

                if (context.Request.Method == "POST")
                {
                    string fio = context.Request.Form["fio"];

                    context.Session.SetString("fio", fio);

                    IEnumerable<Client> clientsByFIO = clients.Where(s => s.Fio == fio);

                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ��������</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
                else
                {

                    string HtmlString = "<HTML><HEAD><TITLE>��������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>";
                    HtmlString += "<BODY><H1>������ ��������</H1><TABLE BORDER=1>";
                    HtmlString += formHtml;
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ID</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ��������</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>�����</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>����� ��������</TH>";
                    HtmlString += "<TH>������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                }
            });
        });
        
        app.Run((context) =>
        {

            CachedAgencyDb cachedAgencyDb = context.RequestServices.GetService<CachedAgencyDb>();

            cachedAgencyDb.GetClient("client");


            string HtmlString = "<HTML><HEAD><TITLE>�������</TITLE></HEAD>" +
            "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            "<BODY><H1>�������</H1>";
            HtmlString += "<H2>������ �������� � ��� �������</H2>";
            HtmlString += "<BR><A href='/'>�������</A></BR>";
            HtmlString += "<BR><A href='/info'>����������</A></BR>";
            HtmlString += "<BR><A href='/client'>������ ��������</A></BR>";
            HtmlString += "<BR><A href='/searchClientDiscount'>������ ���������������</A></BR>";
            HtmlString += "<BR><A href='/searchClient'>����� �� ���</A></BR>";
            HtmlString += "</BODY></HTML>";

            return context.Response.WriteAsync(HtmlString);

        });

        app.Run();
    }
}
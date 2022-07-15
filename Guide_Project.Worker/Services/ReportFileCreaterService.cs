using ClosedXML.Excel;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Guide_Project.Core.Services;
using Guide_Project.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guide_Project.Worker.Services;
public class ReportFileCreaterService
{
    public IModel _channel;
    public IServiceProvider _serviceProvider;
    public DataTable GetTable(string tableName)
    {
        List<CommercialActivity> commercialActivities;
        try
        {
            using(var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                commercialActivities = context.CommercialActivities.ToList();
            }

            DataTable table = new DataTable{ TableName = "AllDataTable" };
            table.Columns.Add("Name", typeof(int));
            table.Columns.Add("SurName", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("Price", typeof(decimal));

            int totalCommercialActivity = 0;
            decimal totalPrice = 0; 
            commercialActivities.ForEach(ctx => 
            {
                table.Rows.Add(
                    ctx.Customer.Name, 
                    ctx.Customer.SurName,
                    ctx.Customer.Phone,
                    ctx.Price);
                totalPrice += ctx.Price;
                totalCommercialActivity++;
            });

            DataView sortedTable = new DataView(table);
            sortedTable.Sort = "Name";
            commercialActivities.ForEach(ctx => 
            {
                sortedTable.RowFilter = $"Name = {ctx.Customer.Name}";
            });
            DataTable topFive = sortedTable.ToTable(tableName, true, "Name", "Surname", "Phone"); 
            topFive.Rows.Add($"TotalPrice = {totalPrice}");
            topFive.Rows.Add($"TotalCommercialActivity = {totalCommercialActivity}");
            return topFive;
        }   
        catch(Exception err)
        {
            throw new Exception(err.Message);
        }
    }
    public Task Consumer_Recieved(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var reportExcelMessage = JsonSerializer.Deserialize<ReportMessageDto>
                (Encoding.UTF8.GetString(@event.Body.ToArray()));
            
            using var memeoryStream = new MemoryStream();

            var workBook = new XLWorkbook();
            var dataSet = new DataSet();

            dataSet.Tables.Add(this.GetTable("topFive"));

            workBook.Worksheets.Add(dataSet);
            workBook.SaveAs(memeoryStream);

        }
        catch(Exception er)
        {
            throw new Exception(er.Message);
        }
        return Task.CompletedTask;
    }
}


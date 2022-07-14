using Guide_Project.Core.DTOs;
using Guide_Project.Core.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guide_Project.Worker.Services;
public class ImageWatermarkService
{
    public IModel _channel;
    public Task Consumer_Recieved(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var imageCreatedEvent = JsonSerializer.Deserialize<CustomerDto>
                (Encoding.UTF8.GetString(@event.Body.ToArray()));

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", imageCreatedEvent.Image);
            var drawString = "www.mysite.com";
            using var image = Image.FromFile(path);
            using var graphic = Graphics.FromImage(image);

            var font = new Font(FontFamily.GenericMonospace, 32, FontStyle.Italic, GraphicsUnit.Pixel);

            var textSize = graphic.MeasureString(drawString, font);
            var color = Color.FromArgb(128, 255, 255, 255);
            var brush = new SolidBrush(color);

            var position = new Point(image.Width - ((int)textSize.Width + 30), image.Height + ((int)textSize.Height + 30));

            graphic.DrawString(drawString, font, brush, position);
            image.Save(path + "/Watermarks/" + imageCreatedEvent.Image);
            image.Dispose();
            graphic.Dispose();
            _channel.BasicAck(@event.DeliveryTag, false);
        }
        catch(Exception err)
        {
            throw new Exception(err.Message);
        }
        return Task.CompletedTask;
    }
}


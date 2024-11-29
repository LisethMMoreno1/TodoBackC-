using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    public class TodoController
    {
        private readonly TodoService _service;

        public TodoController(TodoService service)
        {
            _service = service;
        }

        public void HandleRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/todos")
            {
                HandleGet(response);
            }
            else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/todos")
            {
                HandlePost(request, response);
            }
            else if (request.HttpMethod == "DELETE")
            {
                HandleDelete(request, response);
            }
            else
            {
                response.StatusCode = 404;
                WriteResponse(response, "Not Found");
            }

            response.Close();
        }

        private void HandleGet(HttpListenerResponse response)
        {
            var todos = _service.GetAll();
            string json = JsonSerializer.Serialize(todos);
            WriteResponse(response, json, "application/json");
        }

        private void HandlePost(HttpListenerRequest request, HttpListenerResponse response)
        {
            using var reader = new StreamReader(request.InputStream, Encoding.UTF8);
            string description = reader.ReadToEnd();
            var todo = _service.Add(description);
            WriteResponse(response, JsonSerializer.Serialize(todo), "application/json");
        }

        private void HandleDelete(HttpListenerRequest request, HttpListenerResponse response)
        {
            string idStr = request.QueryString["id"];
            if (int.TryParse(idStr, out int id) && _service.Delete(id))
            {
                WriteResponse(response, "Deleted");
            }
            else
            {
                response.StatusCode = 404;
                WriteResponse(response, "Todo Not Found");
            }
        }

        private void WriteResponse(HttpListenerResponse response, string content, string contentType = "text/plain")
        {
            response.ContentType = contentType;
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}

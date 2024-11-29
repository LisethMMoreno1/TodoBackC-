using System;
using System.Net;
using TodoApi.Controllers;
using TodoApi.Services;

class Program
{
    static void Main(string[] args)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();
        Console.WriteLine("Listening on http://localhost:5000/");

        var todoService = new TodoService();
        var todoController = new TodoController(todoService);

        while (true)
        {
            HttpListenerContext context = listener.GetContext();
            todoController.HandleRequest(context);
        }
    }
}

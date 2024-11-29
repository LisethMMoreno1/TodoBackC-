using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService
    {
        private List<TodoItem> _todos = new List<TodoItem>();
        private int _nextId = 1;

        public List<TodoItem> GetAll()
        {
            return _todos;
        }

        public TodoItem Add(string description)
        {
            var todo = new TodoItem { Id = _nextId++, Description = description };
            _todos.Add(todo);
            return todo;
        }

        public bool Delete(int id)
        {
            var todo = _todos.Find(t => t.Id == id);
            if (todo != null)
            {
                _todos.Remove(todo);
                return true;
            }
            return false;
        }
    }
}

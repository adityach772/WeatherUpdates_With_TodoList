namespace ToDoList.Models
{
    public class List
    {
        public int Id { get; set; }
        public string? Date { get; set; }
        public string? Task { get; set; }

        public string? Priority { get; set; }

        public List()
        {

        }
    }
}



namespace TestTask
{
    public class APIData
    {
        public int Id { set; get; }
        public string Name { set; get; }

        public override string ToString()
        {
            return Name;
        }
    }
}

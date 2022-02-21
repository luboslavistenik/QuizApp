namespace Quiz_Vlajky.Models
{
    public class Question
    {
        public string Name { get; set; }
        public string CorrectAnswer { get; set; }
        public string Answer1  { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public Category Category { get; set; }
    }
}

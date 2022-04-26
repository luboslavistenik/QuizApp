using SQLite;
using Xamarin.Forms;

namespace Quiz_Vlajky.Models
{
    public sealed class Country
    {
        public string Name { get; set; }
        public string ImageFile { get; set; }
        public CountryCategory Category { get; set; }
        
        [Ignore]
        public ImageSource Image => ImageSource.FromFile(ImageFile);
    }
}

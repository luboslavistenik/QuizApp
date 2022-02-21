using Xamarin.Forms;

namespace Quiz_Vlajky.Models
{
    public sealed class Country
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }

        public static implicit operator Country(string internalName)
        {
            var name = internalName.Replace('_', ' ');
            var image = ImageSource.FromFile($"{internalName}.png");

            return new Country
            {
                Name = name,
                Image = image
            };
        }
    }
}

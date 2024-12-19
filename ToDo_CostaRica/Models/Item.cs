using System;

namespace ToDo_CostaRica.Models
{
    public class BaseModel
    {
        public bool IsBusy { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }

    public class OnboardingItem
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLastItem { get; set; }
    }

    public class Banner : BaseModel
    {
        public string Img { get; set; }
    }

    public class FavouriteItem
    {
        public string Icon { get; set; }
        public string Title { get; set; }
    }
}
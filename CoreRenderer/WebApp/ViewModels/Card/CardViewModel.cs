using Progress.Sitefinity.AspNetCore.Widgets.Models.Common;
using Progress.Sitefinity.Renderer.Entities.Content;
using Progress.Sitefinity.RestSdk.Dto;
using ViewComponents.Card;

namespace WebApp.ViewModels.Card
{
    public class CardViewModel
    {
        public ImageDto Image { get; set; }
        public string CardTitle { get; set; }
        public string ButtonStyle { get; set; }
        public string Margins { get; set; }
        public string Link { get; set; }   


    }
}

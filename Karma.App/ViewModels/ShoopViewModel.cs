using Karma.Core.DTOS;
namespace Karma.App.ViewModels
{
    public class ShoopViewModel
    {

      public  IEnumerable<CategoryGetDto> categories { get; set; }
        public IEnumerable<BrandGetDto> brands { get; set; }
        public IEnumerable<ColorGetDto> colorGetDtos { get; set; }
    }
}


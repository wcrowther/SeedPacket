using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SeedPacket.Examples.TagHelpers
{
    // FROM: https: //docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-5.0#condition-tag-helper

    [HtmlTargetElement(Attributes = "asp-if")]
    public class AspIfTagHelper : TagHelper
    {
        public bool AspIf { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!AspIf)
            {
                output.SuppressOutput();
            }
        }
    }
}

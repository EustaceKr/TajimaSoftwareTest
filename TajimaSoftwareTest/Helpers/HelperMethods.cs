using Data.Enumerations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Helpers
{
    public static class HelperMethods
    {
        public static SelectList GetDecorationMethodSelectList()
        {
            var decorationMethods = Enum.GetValues(typeof(DecorationMethod))
                                        .Cast<DecorationMethod>()
                                        .Select(e => new { Value = (int)e, Text = e.ToString() })
                                        .ToList();

            decorationMethods.Insert(0, new { Value = 0, Text = "-select-" });

            return new SelectList(decorationMethods, "Value", "Text");
        }
        public static SelectList GetDecorationMethodSelectList(DecorationMethod selectedValue)
        {
            var values = Enum.GetValues(typeof(DecorationMethod))
                             .Cast<DecorationMethod>()
                             .Select(e => new { Value = (int)e, Text = e.ToString() })
                             .ToList();

            return new SelectList(values, "Value", "Text", selectedValue);
        }
    }
}

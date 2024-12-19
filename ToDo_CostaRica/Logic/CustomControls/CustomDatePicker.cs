

namespace ToDo_CostaRica.CustomControls
{
    public class CustomDatePicker : DatePicker
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(string),
        defaultValue: string.Empty);
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }


        public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor),
        typeof(Color), typeof(CustomDatePicker), Colors.Gray);
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor),
        typeof(Color), typeof(CustomDatePicker), Colors.Gray);
        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }

        public static readonly BindableProperty UnfocusColorProperty =
        BindableProperty.Create(nameof(UnfocusColor),
        typeof(Color), typeof(CustomDatePicker), Colors.Gray);
        // Gets or sets UnfocusColor value  
        public Color UnfocusColor
        {
            get => (Color)GetValue(UnfocusColorProperty);
            set => SetValue(UnfocusColorProperty, value);
        }
    }
}

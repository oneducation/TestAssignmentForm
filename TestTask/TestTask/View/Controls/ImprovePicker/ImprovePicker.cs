using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestTask
{
    /// <summary>
    /// Стандартний <see cref="Picker"/>, доповнений двома додатковими властивостями: PlaceHolder і HorizontalTextAlignment
    /// </summary>
    public class ImprovePicker : Picker
    {
        public static readonly BindableProperty PlaceHolderProperty = 
            BindableProperty.Create(
                propertyName: nameof(PlaceHolder),
                returnType: typeof(string),
                declaringType: typeof(ImprovePicker),
                defaultValue: default(string));

        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(
                propertyName: nameof(HorizontalTextAlignment),
                returnType: typeof(TextAlignment),
                declaringType: typeof(ImprovePicker),
                defaultValue: default(TextAlignment));

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public TextAlignment HorizontalTextAlignment
        {
            get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }
    }
}

using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using Windows.UI.Xaml;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(TestTask.ImprovePicker), typeof(TestTask.UWP.ImprovePickerRenderer))]

namespace TestTask.UWP
{
    public class ImprovePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> args)
        {
            base.OnElementChanged(args);
            var element = Element as ImprovePicker;

            if (Control != null)
            {
                if (element.PlaceHolder != null)
                    Control.PlaceholderText = element.PlaceHolder;

                UpdateAlignment(element);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = this.Element as ImprovePicker;

            if (e.PropertyName == ImprovePicker.PlaceHolderProperty.PropertyName)
                Control.PlaceholderText = element.PlaceHolder;
            else if (e.PropertyName == ImprovePicker.HorizontalTextAlignmentProperty.PropertyName)
                UpdateAlignment(element);
        }

        void UpdateAlignment(ImprovePicker element)
        {
            Control.HorizontalContentAlignment = HorizontalTextAlignmentToHorizontalContentAlignment(element.HorizontalTextAlignment);
        }

        HorizontalAlignment HorizontalTextAlignmentToHorizontalContentAlignment(Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    return HorizontalAlignment.Center;
                case Xamarin.Forms.TextAlignment.End:
                    return HorizontalAlignment.Right; 
                default:
                    return HorizontalAlignment.Left;
            }
        }
    }
}

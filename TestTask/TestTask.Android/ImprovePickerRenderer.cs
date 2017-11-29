using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using System.ComponentModel;
using Android.Views;

[assembly: ExportRenderer(typeof(TestTask.ImprovePicker), typeof(TestTask.Droid.ImprovePickerRenderer))]

namespace TestTask.Droid
{
    public class ImprovePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> args)
        {
            base.OnElementChanged(args);
            ImprovePicker element = Element as ImprovePicker;

            if (element.PlaceHolder != null)
                    element.Title = element.PlaceHolder;

            if (Control != null)
                UpdateGravity(element);            
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var element = this.Element as ImprovePicker;

            if (e.PropertyName == ImprovePicker.PlaceHolderProperty.PropertyName)
                element.Title = element.PlaceHolder;
            else if (e.PropertyName == ImprovePicker.HorizontalTextAlignmentProperty.PropertyName)
                UpdateGravity(element);
        }

        void UpdateGravity(ImprovePicker element)
        {
            Control.Gravity = HorizontalTextAlignmentToHorizontalGravityFlags(element.HorizontalTextAlignment);
        }

        GravityFlags HorizontalTextAlignmentToHorizontalGravityFlags(Xamarin.Forms.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    return GravityFlags.CenterHorizontal;
                case Xamarin.Forms.TextAlignment.End:
                    return GravityFlags.Right;
                default:
                    return GravityFlags.Left;
            }
        }
    }
}
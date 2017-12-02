using Xamarin.Forms;

namespace TestTask
{
    // Джерело: https://habrahabr.ru/company/microsoft/blog/307890/

    [ContentProperty("Content")]
    public class StateConditionRegion : View
    {
        public object State { get; set; }
        public View Content { get; set; }
    }
}

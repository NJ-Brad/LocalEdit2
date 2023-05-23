namespace LocalEdit2.Shared
{
    public partial class BlazoriseLayout
    {
        string TestValue = "Hello Brad";

        bool isErrorActive;
        string title;
        string message;

        public void ShowError(string title, string message)
        {
            this.isErrorActive = true;
            this.title = title;
            this.message = message;
            StateHasChanged();
        }

        private void HideError()
        {
            isErrorActive = false;
        }
    }
}

using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class MessageBox : LE_ModalBase
    {
        public string? MessageBoxID { get; set; }
        public string? Title { get; set; } = "Message Box";
        public string? QuestionText { get; set; } = "I have a question for you";
        public List<string> Buttons { get; set; } = new List<string>();
        public MarkupString MarkedUpQuestionText { get {
                return new MarkupString(QuestionText);
            } }

        public void ShowModal(string MessageBoxID, string Title, string QuestionText, List<string> Buttons)
        {
            this.MessageBoxID = MessageBoxID;
            this.Title = Title;
            this.QuestionText = QuestionText;
            this.Buttons = Buttons;
            ShowModal();
        }

        // for potential "autoclose"
        // await Task.Delay(...)

        // MarkupString

        protected Task ButtonClicked(ModalResult result)
        {
            Result = result;

            if (modalRef != null)
                modalRef.Hide();

            return Closed.InvokeAsync();
        }

        private bool ButtonVisible(int buttonNumber)
        {
            return ((buttonNumber > 0) && (buttonNumber <= Buttons.Count));
        }

        private bool ButtonPrimary(int buttonNumber)
        {
            return (buttonNumber == Buttons.Count);
        }

    }
}

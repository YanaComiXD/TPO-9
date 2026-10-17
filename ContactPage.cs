using OpenQA.Selenium;
using System.IO;

namespace ContactFormTests.Pages
{
    public class ContactPage : BasePage
    {
        private string GetUrl()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(baseDir, "contact.html");
            return "file:///" + filePath.Replace("\\", "/");
        }

        private static readonly By NameInput = By.Id("name");
        private static readonly By EmailInput = By.Id("email");
        private static readonly By SubjectInput = By.Id("subject");
        private static readonly By MessageInput = By.Id("message");
        private static readonly By SubmitButton = By.Id("submitBtn");

        private static readonly By SuccessAlert = By.Id("successAlert");
        private static readonly By NameError = By.Id("errorName");
        private static readonly By EmailError = By.Id("errorEmail");
        private static readonly By MessageError = By.Id("errorMessage");

        public ContactPage(IWebDriver driver) : base(driver) { }

        public ContactPage OpenPage()
        {
            Open(GetUrl());
            return this;
        }

        public ContactPage FillForm(string name = null, string email = null,
                                    string subject = null, string message = null)
        {
            if (name != null) Type(NameInput, name);
            if (email != null) Type(EmailInput, email);
            if (subject != null) Type(SubjectInput, subject);
            if (message != null) Type(MessageInput, message);
            return this;
        }

        public ContactPage Submit()
        {
            Click(SubmitButton);
            return this;
        }

        public bool IsSuccessVisible() => IsVisible(SuccessAlert);
        public bool IsNameErrorVisible() => IsVisible(NameError);
        public bool IsEmailErrorVisible() => IsVisible(EmailError);
        public bool IsMessageErrorVisible() => IsVisible(MessageError);

        public string GetSuccessText() => GetText(SuccessAlert);
        public string GetNameErrorText() => GetText(NameError);
        public string GetEmailErrorText() => GetText(EmailError);
        public string GetMessageErrorText() => GetText(MessageError);
    }
}

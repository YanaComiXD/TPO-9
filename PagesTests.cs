using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ContactFormTests.Pages;
using System;

namespace ContactFormTests.Tests
{
    [TestClass]
    public class ContactFormTests
    {
        private IWebDriver _driver;
        private ContactPage _page;

        [TestInitialize]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1400,900");
    
            _driver = new ChromeDriver(options); 
            _page = new ContactPage(_driver);
        }
        [TestCleanup]
        public void Teardown()
        {
            _driver?.Quit();
        }

        [TestMethod]
        [TestCategory("Positive")]
        public void Positive_ValidData_ShowsSuccessMessage()
        {
            _page.OpenPage()
                 .FillForm(
                     name: "Зубцова Яна Романовна",
                     email: "yanacomix@gmail.com",
                     subject: "Вопрос по заказу",
                     message: "Здравствуйте! Где моя аниме фигурка!?????")
                 .Submit();

            Assert.IsTrue(_page.IsSuccessVisible(),
                "Сообщение об успешной отправке не отображается");
            StringAssert.Contains(_page.GetSuccessText().ToLower(), "успешно");
        }

        [TestMethod]
        [TestCategory("Negative")]
        public void Negative_EmptyName_ShowsValidationError()
        {
            _page.OpenPage()
                 .FillForm(
                     name: "",
                     email: "yanacomix@gmail.com",
                     subject: "Вопрос",
                     message: "Хочу питсу.")
                 .Submit();

            Assert.IsTrue(_page.IsNameErrorVisible(),
                "Ошибка поля 'Имя' не отображается");
            StringAssert.Contains(_page.GetNameErrorText().ToLower(), "обязательно");
            Assert.IsFalse(_page.IsSuccessVisible(),
                "Форма не должна отправляться при пустом обязательном поле");
        }

        [TestMethod]
        [TestCategory("Negative")]
        public void Negative_InvalidEmail_ShowsEmailError()
        {
            _page.OpenPage()
                 .FillForm(
                     name: "Зубцова Яна",
                     email: "yanacomix.com",
                     subject: "Вопрос",
                     message: "Поставите автомат?")
                 .Submit();

            Assert.IsTrue(_page.IsEmailErrorVisible(),
                "Ошибка поля 'Email' не отображается");
            StringAssert.Contains(_page.GetEmailErrorText().ToLower(), "корректн");
            Assert.IsFalse(_page.IsSuccessVisible());
        }

        [TestMethod]
        [TestCategory("Negative")]
        public void Negative_EmptyMessage_ShowsMessageError()
        {
            _page.OpenPage()
                 .FillForm(
                     name: "Зубцова Яна",
                     email: "yanacomix@gmail.com",
                     subject: "Вопрос",
                     message: "")
                 .Submit();

            Assert.IsTrue(_page.IsMessageErrorVisible(),
                "Ошибка поля 'Сообщение' не отображается");
            Assert.IsFalse(_page.IsSuccessVisible());
        }

        [TestMethod]
        [TestCategory("Boundary")]
        public void Negative_AllFieldsEmpty_ShowsAllErrors()
        {
            _page.OpenPage().Submit();

            Assert.IsTrue(_page.IsNameErrorVisible());
            Assert.IsTrue(_page.IsEmailErrorVisible());
            Assert.IsTrue(_page.IsMessageErrorVisible());
            Assert.IsFalse(_page.IsSuccessVisible());
        }
    }
}

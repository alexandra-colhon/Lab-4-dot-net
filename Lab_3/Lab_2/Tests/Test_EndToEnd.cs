using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    class Test_EndToEnd
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetUpDriver()
        {
            _driver = new ChromeDriver("D:/Personal/Sem IV/Modul II/dot net");
        }


        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }


        [Test]
        public void ExpensesListMenuButtonGoesToList()
        {
            _driver.Url = "http://localhost:8100/login";

            try
            {
                var menuButton = _driver.FindElement(By.XPath("//*[@id='content1']/app-login/app-navbar/ion-header/ion-toolbar/ion-buttons"));
                menuButton.Click();
                Thread.Sleep(2000);

                try
                {
                    var menuListButton = _driver.FindElement(
                        By.XPath("/html/body/app-root/ion-app/app-side-menu/ion-menu/ion-content/ion-list/ion-menu-toggle[1]/ion-item"));
                    menuListButton.Click();
                    Thread.Sleep(5000);

                    try
                    {
                        var goToButton = _driver.FindElement(By.XPath("//*[@id='content1']/app-expenses/ion-footer/ion-button"));
                        goToButton.Click();
                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        Assert.Fail("Go to extenses button not found");
                    }

                }
                catch (NoSuchElementException)
                {
                    Assert.Fail("Expenses List button not found");
                }

            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Menu button not found");
            }

        }


        [Test]
        public void MenuButtonClicked()
        {
            _driver.Url = "http://localhost:8100/login";
            //Thread.Sleep(5000);
            try
            {
                var menuButton = _driver.FindElement(By.XPath("//*[@id='content1']/app-login/app-navbar/ion-header/ion-toolbar/ion-buttons"));
                menuButton.Click();
                Thread.Sleep(2000);
            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Menu button not found");
            }

        }

    }
}

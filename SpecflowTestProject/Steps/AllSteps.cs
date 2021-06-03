using NewBookModelsApiTests.ApiRequests.Auth;
using NewBookModelsApiTests.Models.Auth;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumTests.PageObject;
using SeleniumTests.POM;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace SpecflowTestProject.Steps.API
{
    [Binding]
    public class AuthSingInSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly UpdateProfilePage _updateProfilePage;
        private readonly IWebDriver _webDriver;
        private readonly SignUpPage _signUpPage;
        private readonly SignInPage _signInPage;

        public AuthSingInSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _webDriver = _scenarioContext.Get<IWebDriver>(Constants.WebDriver);
            _signUpPage = new SignUpPage(_webDriver);
            _signInPage = new SignInPage(_webDriver);
            _updateProfilePage = new UpdateProfilePage(_webDriver);
        }

        [Given(@"Client is created")]
        public void GivenClientIsCreated()
        {
            var createdUser = AuthRequests.SendRequestClientSignUpPost(new Dictionary<string, string>
            {
                {"email", $"asda2sd2asd{DateTime.Now:ddyyyymmHHmmssffff}@asdasd.ert"},
                {"first_name", "asdasdasd"},
                {"last_name", "asdasdasd"},
                {"password", Constants.Password},
                {"phone_number", "3453453454"}
            });

            _scenarioContext.Add(Constants.User, createdUser);
        }

        [When(@"I change phone with data on Update Profile page")]
        public void WhenIChangePhoneWithDataOnUpdateProfilePage(Table table)
        {
            _updateProfilePage.ClickPhoneChange()
                .SetCurrentPassword(Constants.Password)
                .SetNewPhone(table.Rows[0]["new_phone"])
                .ClickSavePhone();
        }

        [Then(@"Successfully changed phone to (.*) on update profile page")]
        public void ThenSuccessfullyChangedPhoneToOnUpdateProfilePage(string phone)
        {
            Assert.AreEqual(phone, _updateProfilePage.GetNewPhone());
        }

        [Then(@"Successfully registrated in NewBookModels as new client")]
        public void ThenSuccessfullyLoggedInNewBookModelAsCreatedClient()
        {
            Assert.IsTrue(_signUpPage.IsPageTitleVisible());
        }

        [Then(@"exception message (.*) is displayed on sign in page")]
        public void ThenExceptionMessageIsDisplayedOnSignInPage(string message)
        {
            Assert.AreEqual(message, _signInPage.GetExceptionInvalidData());
        }

        [Given(@"Sign in page is opened")]
        public void GivenSignInPageIsOpened()
        {
            _signInPage.OpenPage();
        }

        [When(@"I login with email (.*) and password (.*)")]
        public void ILoginWithData(string email, string password)
        {
            email = _scenarioContext.Get<AuthRequests.ResponseModel<ClientAuthModel>>(Constants.User).Model.User.Email;
            _signInPage.SetEmail(email);
            _signInPage.SetPassword(Constants.Password);
            _signInPage.ClickLoginButton();
        }

        [When(@"I login with invalid email (.*) and password (.*)")]
        public void WhenILoginWithInvalidEmailAndPassword(string email, string password)
        {
            _signInPage.SetEmail(email);
            _signInPage.SetPassword(password);
            _signInPage.ClickLoginButton();
        }

        [Then(@"Successfully logged in NewBookModels as created client")]
        public void ThenSuccessfullyLoggedInNewBookModelsAsCreatedClient()
        {
            Assert.IsTrue(_signUpPage.IsPageTitleVisible());
        }

        [Given(@"Sign up page is opened")]
        public void GivenSignUpPageIsOpened()
        {
            _signUpPage.GoToRegistrationPage();
        }

        [When(@"I registrate with data")]
        public void WhenIRegistrateWithData(Table table)
        {
            _signUpPage.SetFirstName(table.Rows[0]["name"]);
           _signUpPage.SetLastName(table.Rows[0]["last_name"]);
           _signUpPage.SetEmail($"asda2sd2asd{DateTime.Now:ddyyyymmHHmmssffff}@asdasd.ert");
           _signUpPage.SetPassword(table.Rows[0]["password"]);
            _signUpPage.SetPhoneNumber(table.Rows[0]["phone_number"]);

            _signUpPage.ClickNextButton();
        }

        [Given(@"Update Profile page is opened")]
        public void GivenUpdateProfilePageIsOpened()
        {
            var user = _scenarioContext.Get<AuthRequests.ResponseModel<ClientAuthModel>>(Constants.User).Model.TokenData.Token;
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            var webDriver = _scenarioContext.Get<IWebDriver>(Constants.WebDriver);
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
            webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
            IJavaScriptExecutor js = (IJavaScriptExecutor)webDriver;

            webDriver.Navigate().GoToUrl("https://newbookmodels.com/auth/signin");

            js.ExecuteScript($"localStorage.setItem('access_token','{user}');");

            webDriver.Navigate().GoToUrl("https://newbookmodels.com/account-settings/account-info/edit");
        }

        [When(@"I update profile with data on UpdateProfile page")]
        public void WhenIUpdateProfileWithDataOnUpdateProfilePage(Table table)
        {
            _updateProfilePage.ClickChangeGeneralInfo();
            _updateProfilePage.SetNewName(table.Rows[0]["name"]);
            _updateProfilePage.SetNewLastName(table.Rows[0]["last_name"]);
            _updateProfilePage.SetIndustry(table.Rows[0]["industry"]);
            _updateProfilePage.SetLocation(table.Rows[0]["company_location"]);
            _updateProfilePage.ClickSaveGeneralChanges();
        }

        [Then(@"Successfully changed name to (.*), industry to (.*), company location to (.*) on update profile page")]
        public void ThenSuccessfullyChangedGeneralInformationOnUpdateProfilePage(string name, string industry, string location)
        {
            Assert.AreEqual(name, _updateProfilePage.GetNewName());
            Assert.AreEqual(industry, _updateProfilePage.GetNewIndustry());
            Assert.AreEqual(location, _updateProfilePage.GetNewLocation());
        }
    }
}
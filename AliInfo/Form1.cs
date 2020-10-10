using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AliInfo
{
    public partial class Form1 : Form
    {
        IWebDriver Browser;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in richTextBox1.Lines)
            {
                string priceRUB, priceUAH, priceUSD, mark, review, orders, rating;
                Browser = new ChromeDriver();
                Browser.Navigate().GoToUrl(item);
                priceRUB = Browser.FindElement(By.ClassName("product-price-value")).Text;
                mark = Browser.FindElement(By.ClassName("product-price-mark")).Text;
                review = Browser.FindElement(By.ClassName("product-reviewer-reviews")).Text;
                review = review.Replace("Отзывы", "");
                orders = Browser.FindElement(By.ClassName("product-reviewer-sold")).Text;
                orders = orders.Replace("заказа(ов)", "");
                rating = Browser.FindElement(By.ClassName("overview-rating-average")).Text;
                Cookie cookie = Browser.Manage().Cookies.GetCookieNamed("aep_usuc_f");
                string val = cookie.Value;
                val = val.Replace("RUB", "USD");
                Browser.Manage().Cookies.DeleteCookieNamed("aep_usuc_f");
                Browser.Manage().Cookies.AddCookie(new Cookie("aep_usuc_f", val));
                Browser.Navigate().Refresh();
                Thread.Sleep(1000);
                priceUSD = Browser.FindElement(By.ClassName("product-price-value")).Text;
                Browser.Manage().Cookies.DeleteCookieNamed("aep_usuc_f");
                val = val.Replace("USD", "UAH");
                Browser.Manage().Cookies.AddCookie(new Cookie("aep_usuc_f", val));
                Browser.Navigate().Refresh();
                Thread.Sleep(1000);
                priceUAH = Browser.FindElement(By.ClassName("product-price-value")).Text;
                richTextBox1.Text = $"Цена: {priceRUB.Replace("руб.", "")} | {priceUAH.Replace("грн.", "")} | {priceUSD.Replace("US $", "")} \n" +
                                $"Скидка: {mark} \n" +
                                $"Заказов: {orders}\n" +
                                $"Рейтинг продавца: {rating}\n" +
                                $"Отзывов: {review}\n";
                Browser.Close();
                Browser.Dispose();

            }
        }
    }
}

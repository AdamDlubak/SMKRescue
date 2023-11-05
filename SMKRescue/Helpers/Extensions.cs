using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace SMKRescue.Extensions;

public static class SmkExtensions
{
    public const string TrXPath = "./div[2]/div[2]/div[1]/div[2]/div[1]/div[3]/div[1]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr[1]";

    public static void InsertInput(this IWebElement parentNode, int index, string value)
    {
        var node = parentNode.FindElement(By.XPath(TrXPath));

        var input = node.FindElement((By.XPath($"./td[{index}]/div/input")));
        input.SendKeys(value);
    }

    public static void InsertSelectByIndex(this IWebElement parentNode, int selectFieldIndex, int selectValueIndex)
    {
        var node = parentNode.FindElement(By.XPath(TrXPath));

        var select = node.FindElement((By.XPath($"./td[{selectFieldIndex}]/div/select")));
        var selectElement = new SelectElement(select);
        selectElement.SelectByIndex(selectValueIndex);
    }

    public static void InsertSelectByText(this IWebElement parentNode, int selectFieldIndex, string text)
    {
        var node = parentNode.FindElement(By.XPath(TrXPath));

        var select = node.FindElement((By.XPath($"./td[{selectFieldIndex}]/div/select")));
        var selectElement = new SelectElement(select);
        selectElement.SelectByText(text);
    }

    public static IWebElement GetParent(this IWebElement node)
    {
        return node.FindElement(By.XPath(".."));
    }
}
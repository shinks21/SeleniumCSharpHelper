<h1>How do I...</h1>

<h2>Setup the project</h2>
<p>Add references to SeleniumCSharpHelper and OpenQA.Selenium</p>
<p>Add to the top of the page</p>

<code>
    using OpenQA.Selenium;
    <br />
    using SeleniumCSharpHelper;
</code>

<h2>Open a Browser</h2>
<code>
    WebDriver driver;
    <br />
    <br />WebDriverExtensions.Browser_NavigateChrome(ref driver, appURL, title: "expected title", maximize: true);
</code>

<h2>Select an element by Id</h2>
<code>
    var element = By.Id("ElementId");
</code>

<h2>Select an element by an attribute</h2>
<code>
    var element = By.CssSelector($"[data-name='VehicleCard']");
</code>

<h2>Select an element by an a CSS class</h2>
<code>
    var element = By.ClassName(("ClassName");
</code>

<h2>Select an element by an an XPath to the element</h2>
<code>
    var element = By.XPath("//*[@id=\"ofertas_servicos\"]/div/p");
</code>

<h2>Set the value of a text element</h2>
<code>
    _driver.Element_SendKeys(By.Id("Name"), "First Last");
</code>

<h2>Set a the value of a select element (dropdown)</h2>
<code>
    SelectElement selectElement = _driver.SelectElement_SelectByTextRetry(By.Id("ElementId"), textToSelect);
</code>

<h2>Set an html attribute value</h2>
<code>
    _driver.Element_SetAttribute(By.Id("ElementId"), "attributeName", "valueToSet");
</code>

<h2>Set a radio button value</h2>
<code>
	_driver.RadioButtonElement_SetValue(By.Id("ElementId"), 1);
</code>

<h2>Move the mouse over an element</h2>
<code>
    _driver.Element_MouseOver(By.Id("ElementId");
</code>

<h2>Move the page to an element</h2>
<code>
    _driver.Element_MoveToElement(By.Id("ElementId");
</code>

<h2>Verify an element is not visible</h2>
<code>
    _driver.Element_WaitUntilNotVisible(By.CssSelector("[data-name='City']"));
</code>

<h2>Verify an elements text is correct</h2>
<code>
    string text = _driver.Element_GetText(element);
</code>

<h2>Verify a hidden elements text is correct</h2>
<code>
    string text = _driver.HiddenElement_GetText(elementIdAsString).ToString();
</code>

<h2>Wait until an elements text is correct</h2>
<code>
    bool = _driver.Element_WaitUntilTextIsVisible(elementThatHoldsText, textToSelect);
</code>

<h2>Verify an element is visible</h2>
<code>
    bool visible = _driver.Element_WaitUntilVisible(element);
</code>

<h2>Verify select element's text is correct</h2>
<code>
    string text = _driver.SelectElement_GetText(By.CssSelector("[data-name='State']"));
</code>

<h2>Verify the selected radio button value</h2>
<code>
	string value _driver.RadioButtonElement_GetValue(By.Id("ElementId"));
</code>

<h2>View the current state of the dom</h2>
<code>
    string pageSource = _driver.PageSource;

    (then plug the result into https://codebeautify.org/xmlviewer)
</code>




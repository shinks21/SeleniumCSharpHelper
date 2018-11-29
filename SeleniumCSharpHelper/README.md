<h1>How do I...</h1>

<h2>Setup he project</h2>
<code>
	using OpenQA.Selenium;
	<br/>
	using SeleniumCSharpHelper;
</code>


<h2>Open a Browser</h2>
<code>
	WebDriver driver;
	<br/>
	<br/>WebDriverExtensions.Browser_NavigateChrome(ref driver, appURL, title: "expected title", maximize: true);
</code>
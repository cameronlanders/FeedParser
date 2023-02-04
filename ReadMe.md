
# Feed Parser   
## Extracts Articles From RSS or Atom Feeds 
--------------------------------------------------------------
- Version 1.0.19 - Released July 16, 2022

- Developed using C#

- Author: **Cameron Landers**

    - __LinkedIN profile:__ https://www.linkedin.com/in/cameronlandersexperience/
 
    - __Web Site:__ https://conversiondynamics.com

    - __Email:__ support@conversiondynamics.com.

- __LICENSE:__  Free and Open Source. No restrictions. 

- Please credit the author by including the above information in your distributions or documentation.

--------------------------------------------------------------
  
## What is Feed Parser?
Feed Parser is a useful class you can use to add the ability to consume Atom or RSS feeds in your ASP.NET web applications. Feed Parser will parse the feed specified and produce a list of Article objects, the contents of which you can then format into your web pages any way you like.  

## Where can I use Feed Parser?

Feed Parser was created for classic ASP.NET (Web Forms) applications. I developed it initially for use in a classic ASP.NET MVC (Web API 2) application. 

If you are using classic ASP.NET 4.6 or later, it should be plug-and-play for you. 

If you are using Microsoft-supported versions of ASP.NET Core (or .NET 6 or later) or some other technology, I recommend you look elsewhere for a solution more specific to the framework or technology you are using. 

It would require significant changes to port it, since ASP.NET Core (and later) differs from the classic ASP.NET Framework in a multitude of ways - it's so vastly different in fact, that it would require pretty much a complete rewrite. 

- You may recall that Microsoft recently dropped the "Core" verbiage from thier product naming scheme. They now use a simplified naming scheme for the latest frameworks: ".NET 6" and ".NET 7" respectively. I may release an ASP.NET 6 or 7 (or later) version of this at some point. Check my GitHub repos periodically.

## Files in the distribution 
- `Article.cs` - The custom "Article" class used by Feed Parser
- `FeedParser.cs` - The main class that parses RSS and Atom feeds and produces lists of Article objects (`List<Article>`) that you can use in your ASP.NET web pages or in C# Windows Desktop programs. 
- `Readme.md` - This file
 
## How To Use Feed Parser

After adding the FeedParser.cs and Article.cs classes to your application where you want to use them, you can simply reference FeedParser's public method as you would with any other class.

You will notice that both of these classes are contained within a namespace called "`util`". You can change this to match your namespace and call the public method directly, or you can leave it alone and preface your method calls with "`util.`" - see the method call example below for clarity.

FeedParser's ParseFeed method parses article content from the standardized XML feed returned from the feed URL, and captures the elements of each article into Article objects, then puts all the Article objects into a List<T> and returns the list to the calling procedure. 

The following example uses FeedParser to generate some simple HTML-formatted output that includes the article title, description (summary content) and an image if one exists, for each article extracted from the feed:

--------------------------------------------------------------

    string strtURL = "https://www.nytimes.com/services/xml/rss/nyt/HomePage.xml" 
    string strContent = string.Empty;
    List<util.Article> lstArticles = util.FeedParser.ParseFeed(strURL);
    if (lstArticles == null || lstArticles.Count == 0)
    {
        SetStatus("No articles were returned.");
        return;
    }
    foreach (Article item in lstArticles)
    {
        strContent += "<hr />";
        strContent += "<div class='pad8'><strong>" + item.Title + "</strong></div>";
        strContent += "<div class='pad8'>Published:&nbsp;" + item.PublishDate.ToString() + "</div>";
        strContent += (item.Image == "No Image Available") ? "<div class='pad8'>" + item.Image + "</div>" : "<div class='pad8'><img src='" + item.Image + "' alt='Article Image' style='width:250px' /></div>";
        strContent += "<div class='pad8'>" + item.Description + "</div>";
        strContent += (item.Link == "No Link Available") ? "<div class='pad8'>" + item.Link + "</div>" : "<div class='pad8'><a href='" + item.Link + "'>Read the full story...</a></div>";
        // Create Log info for each item
        strLogTitle = item.Link.Contains("/") ? item.Link.Substring(item.Link.LastIndexOf("/") + 1) : item.Link;
        Logger.AppendLog("[info]: Title Reference: " + strLogTitle + " - URI: " + item.Link);
    }

--------------------------------------------------------------

Notice that I am using my Logger class to generate log entries for each article. See my Logger repository for details on that. If you don't want to use that, just comment it out.

Of course, this is a simple and specific example of the format I used to display articles within an application I was testing at the time. You can modify this to your liking, presenting the articles any way you choose, using your own CSS classes to match the results to your theme, etc. 

Enjoy!

Cheers,

-=Cameron

--------------------------------------------------------------

[eof]  

  
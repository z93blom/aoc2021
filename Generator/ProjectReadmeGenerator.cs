namespace AdventOfCode.Generator
{
    public class ProjectReadmeGenerator {
        public static string Generate(int firstYear, int lastYear, IUsageProvider usageProvider) {
           
            return $@"
               > # Advent of Code ({firstYear}-{lastYear})
               > My C# solutions to the advent of code problems. (My solutions are in the ""solutions"" branch. The master branch contains the scaffolding).
               > Check out http://adventofcode.com.

               > ## Dependencies

               > - `Newtonsoft.Json` for JSON parsing
               > - `HtmlAgilityPack` is used for problem download.

               > ## Setting the session variable
               > The application depends on an environment variable. To get your identifier, you need to
               > look at the request headers for your daily data, and take the session identifier from
               > the cookie in the Request Headers (for example https://adventofcode.com/2018/day/1/input)

               > Then you can set it:

               > ```
               > Powershell: $Env:{Updater.SessionEnvironmentName} = {'"'}...{'"'}
               > 
               > Command Prompt: set {Updater.SessionEnvironmentName} = ...
               > ```

               > ## Running

               > To run the project:

               > 1. Install .NET (6.0 or greater)
               > 2. Clone the repo
               > 3. Get help with `dotnet run`
               > ```
               > {usageProvider.Usage()}
               > ```
               > ".StripMargin("> ");
        }
    }
}
using System.Net;
using System.Text.RegularExpressions;

namespace ParserWeb
{
    /// Взаимодействие с веб-страницей. Вся функциональность тут
    class WebAgent
    {
        private WordsCounter wordsCounter { get; set; }
        private string site { get; }
        private string content { get; }

        /// Сразу избавляемся от всего не предназначенного для пользователя и остаток передаем на подсчет статистики слов
        public WebAgent(string path)
        {
            site = path;
            using var client = new WebClient();
            client.Headers.Add("User-Agent", "C# parsing program");
            content = client.DownloadString(site);
            content = DropInternalsOf(content, "script", "style");
            content = StripHTML(content);
        }

        /// Избавляется от всех тэгов
        private string StripHTML(string HTMLText)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
        }

        /// Избавляется от указанных тэгов и их содержимых
        private string DropInternalsOf(string HTMLText, params string[] TagNames)
        {
            foreach (var TagName in TagNames)
            {
                Regex reg = new Regex($@"(?<=^|\s)<{TagName}.*>[\S\s]+?</{TagName}>(?=\s|$)", RegexOptions.IgnoreCase);
                HTMLText = reg.Replace(HTMLText, "");
            }
            return HTMLText;
        }

        /// Подсчет статистики
        public Dictionary<string, uint> PrintStatistics()
        {
            wordsCounter = new WordsCounter();
            return wordsCounter.PrintWordsCounts(site, content);
        }
    }
}

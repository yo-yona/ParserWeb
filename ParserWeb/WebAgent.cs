using System.Net;
using System.Text.RegularExpressions;

namespace ParserWeb
{
    /// Взаимодействие с веб-страницей. Вся функциональность тут
    class WebAgent
    {
        private WordsCounter? wordsCounter { get; set; }
        private string site { get; }
        private string content { get; set; }
        private WebClient client { get; }
        private object? locker { get; }

        /// Сразу избавляемся от всего не предназначенного для пользователя и остаток передаем на подсчет статистики слов
        public WebAgent(string path)
        {
            locker = new object();
            site = path;
            client = new WebClient();
            client.Headers.Add("User-Agent", "C# parsing program");
        }

        private async Task GetContent()
        {
            content = client.DownloadStringTaskAsync(site).Result;
            content = await DropInternalsOf(content, true, "script", "style");
            content = StripHTML(content);
        }

        /// Избавляется от всех тэгов
        private string StripHTML(string HTMLText)
        {
            Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(HTMLText, "");
        }

        /// Избавляется от указанных тэгов и их содержимых
        private async Task<string> DropInternalsOf(string HTMLText, bool removeAllTags, params string[] TagNames)
        {
            List<Task> localPool = new List<Task>();
            foreach (var TagName in TagNames)
            {
                localPool.Add(Task.Run(() =>
                {
                    Regex reg = new Regex($@"(?<=^|\s)<{TagName}.*>[\S\s]+?</{TagName}>(?=\s|$)", RegexOptions.IgnoreCase);
                    lock (locker)
                    {
                        HTMLText = reg.Replace(HTMLText, "");
                    }
                }));
            }
            await Task.WhenAll(localPool);
            return HTMLText;
        }

        /// Подсчет статистики
        public async Task<Dictionary<string, uint>> PrintStatistics()
        {
            await GetContent();
            wordsCounter = new WordsCounter();
            Task<Dictionary<string, uint>> task = Task.Run(() => wordsCounter.PrintWordsCounts(site, content));
            return task.Result;
        }
    }
}

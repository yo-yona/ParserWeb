using System;
using System.Collections.Generic;
using System.Linq;

namespace ParserWeb
{
    class WordsCounter
    {
        private Dictionary<string, uint> wordStatistics;

        public WordsCounter()
        {
            wordStatistics = new Dictionary<string, uint>();
        }

        /// Если слово есть, увеличиваем счетчик, иначе добавляем в статистику
        private void SafeCountIncrement(string word)
        {
            if (word.All(c => Char.IsLetter(c)))
            {
                if (wordStatistics.ContainsKey(word))
                {
                    wordStatistics[word]++;
                }
                else
                {
                    wordStatistics.Add(word, 1);
                }
            }
        }

        /// Делим текст на слова, разделяемые специальными знаками metaChar
        private void ExtractWords(string incoming)
        {
            //Console.WriteLine("WordsExtracter начал работу...");
            char[] metaChar = { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t', '/', '<', '>', '\'', '«', '»', '?' };

            string[] textDividedIntoWords = incoming?.ToUpper().Split(metaChar, StringSplitOptions.RemoveEmptyEntries);
            if (textDividedIntoWords != null)
            {
                foreach (var word in textDividedIntoWords)
                {
                    SafeCountIncrement(word);
                }
            }
        }

        /// Вывод статистики для страницы (Если есть запись в БД, читаем оттуда. Если нет - подсчитываем заново)
        public Dictionary<string, uint> PrintWordsCounts(string site, string content)
        {
            this.ExtractWords(content);
            return wordStatistics;

        }
    }
}

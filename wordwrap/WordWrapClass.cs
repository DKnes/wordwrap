﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WordWrap
{
    class Tool
    {
        static char[] splitChars = new char[] { ' ', '-', '\t', '\n' };
        public static string WordWrap(string str, int width)
        {
            string[] words = Explode(str, splitChars);
            int curLineLength = 0;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < words.Length; i += 1)
            {
                string word = words[i];
                if (word == "\n")
                {
                    strBuilder.Append("\n");
                    curLineLength = 0;
                    continue;
                }
                if (curLineLength + word.Length > width)
                {
                    if (curLineLength > 0)
                    {
                        strBuilder.Append("\n");
                        curLineLength = 0;
                    }
                    while (word.Length > width)
                    {
                        strBuilder.Append(word.Substring(0, width - 1) + "-");
                        word = word.Substring(width - 1);
                        strBuilder.Append("\n");
                    }
                }
                if (curLineLength == 0 && word != "    ")
                {
                    word = word.TrimStart(' ');
                }
                strBuilder.Append(word);
                curLineLength += word.Length;
            }
            return strBuilder.ToString().Replace("\n\n", "\n");
        }
        private static string[] Explode(string str, char[] splitChars)
        {
            string whitespace = "    ";
            List<string> parts = new List<string>();
            parts.Add(whitespace);
            int startIndex = 0;
            while (true)
            {
                int index = str.IndexOfAny(splitChars, startIndex);
                if (index == -1)
                {
                    parts.Add(str.Substring(startIndex));
                    return parts.ToArray();
                }
                string word = str.Substring(startIndex, index - startIndex);
                char nextChar = str[index];
                if (char.IsWhiteSpace(nextChar))
                {
                    parts.Add(word);
                    parts.Add(nextChar.ToString());
                    if (word == Environment.NewLine || nextChar == '\n')
                    {
                        parts.Add(whitespace);
                    }
                }
                else
                {
                    parts.Add(word + nextChar);
                }
                startIndex = index + 1;
            }
        }
    }
    class HandyTextHandler
    {
        /// <summary>
        /// 计算的书出的总页数
        /// </summary>
        public int numOfPages = 0;
        /// <summary>
        /// 当前页，用于获取相对页数，就public吧
        /// </summary>
        public int curPage = 0;
        /// <summary>
        /// 返回第n页，下标从0开始
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string page(int n)
        {
            return Pages[curPage = n];
        }
        /// <summary>
        /// 返回相对于curPage的第offset页，offset可正可负
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public string rPage(int offset)
        {
            return Pages[curPage += offset];
        }
        /// <summary>
        /// content是你的书的全部内容，length是一行最多多少字符，lines是一页有几行
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="lines"></param>
        public HandyTextHandler(string content, int length, int lines)
        {
            content = Tool.WordWrap(content, length);
            int numOfLine = 0;
            foreach (var i in content)
            {
                if (i == '\n') numOfLine++;
            }
            numOfPages = (numOfLine + lines - 1) / lines;
            for (int i = 0, cur = 0, end = 0; i < numOfPages; i++)
            {
                for (int j = 0; j < lines; j++)
                {
                    end = content.IndexOf('\n', end) + 1;
                }
                int tmp = end - cur;
                if (tmp < 0)
                {
                    tmp = content.Length - cur;
                }
                Pages.Add(content.Substring(cur, tmp));
                cur = end;
            }
        }
        private List<string> Pages = new List<string>();
    }
}

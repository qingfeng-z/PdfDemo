#region license

// <copyright company="ZAN LLC" file="Program.cs">
//   Copyright (c)2019 ZAN ALL RIGHTS RESERVED.
// </copyright>

#endregion

namespace PdfDemo
{
    #region region

    using System;
    using System.IO;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    #endregion

    /// <summary>
    ///     The program.
    /// </summary>
    internal class Program
    {
        /**
         * 将字体文件放入bin目录下  或者 更改获取字体的url
         * 生成pdf文件（文本直接根据xy坐标放在可以放的位置）
         * 1.引nuget包 itextsharp
         * 2.创建 Document  / 文件
         * 3.创建流 MemoryStream
         */

        /// <summary>
        ///     The create pdf.
        /// </summary>
        /// <returns>
        ///     The <see cref="MemoryStream" />.
        /// </returns>
        public static MemoryStream CreatePdf()
        {
            Document document;
            var memory = new MemoryStream();
            document = new Document(PageSize.B5, 20, 20, 20, 20);
            var root = AppDomain.CurrentDomain.BaseDirectory;
            try
            {
                var writer = PdfWriter.GetInstance(document, memory);
                document.Open();

                // 设置字体
                var font = BaseFont.CreateFont($@"{root}simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                // 写入文字
                var directContent = writer.DirectContent;
                directContent.BeginText();
                directContent.SetFontAndSize(font, 25);
                directContent.SetCharacterSpacing(1); // 设定字间距 
                directContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "标题", 100, 100, 1);
                directContent.EndText();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                document.Close();
            }

            return memory;
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            var pdf = CreatePdf().ToArray();
            var path = @"../../test.pdf";
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(pdf, 0, pdf.Length);
            fs.Close();
        }
    }
}
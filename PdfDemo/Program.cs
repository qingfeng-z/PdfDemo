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
    using System.Text;

    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.tool.xml;

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
                var baseFont = BaseFont.CreateFont($@"{root}simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                // 写入文字
                var directContent = writer.DirectContent;
                directContent.BeginText();
                directContent.SetFontAndSize(baseFont, 25);
                directContent.SetCharacterSpacing(1); // 设定字间距 
                directContent.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "标题", 200, 200, 1);
                directContent.EndText();

                var font = new Font(baseFont, 25);

                // 如果是单独一行 比如标题可以使用，用作模板的话不行
                // 写段落
                var content = "合同";
                AddParagraph(document, content, font, 1);
               
                // 写段落
                content = "一二三四五六七八九十一二三四五六七八九十一二三四五六七八九十一二三四五六七八九十";
                AddParagraph(document, content, font, 0);


                /**
                 *将html 富文本转pdf文件。
                 * 1.引入nuget包  itextsharp.xmlworker
                 * 2.创建类实现FontFactoryImp接口
                 */
                var html = "xxxxx";
                byte[] data = Encoding.UTF8.GetBytes(html);
                var msInput = new MemoryStream(data);
                var pdfDest = new PdfDestination(PdfDestination.XYZ, 50, document.PageSize.Height, 1f);

                // UnicodeFontFactory 实现FontFactoryImp接口的实现类
                XMLWorkerHelper.GetInstance().ParseXHtml(
                    writer,
                    document,
                    msInput,
                    null,
                    Encoding.UTF8,
                    new UnicodeFontFactory());
                var action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                writer.SetOpenAction(action);
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
        #region 添加段落

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="document"></param>
        /// <param name="content">内容</param>
        /// <param name="fontsize">字体大小</param>
        public static void AddParagraph(Document document, string content, Font font)
        {
            Paragraph pra = new Paragraph(content, font);
            document.Add(pra);
        }

        /// <summary>
        /// 添加段落
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="fontsize">字体大小</param>
        /// <param name="Alignment">对齐方式（1为居中，0为居左，2为居右）</param>
        /// <param name="SpacingAfter">段后空行数（0为默认值）</param>
        /// <param name="SpacingBefore">段前空行数（0为默认值）</param>
        /// <param name="MultipliedLeading">行间距（0为默认值）</param>
        public static void AddParagraph(
            Document document,
            string content,
            Font font,
            int Alignment = 0,
            float SpacingAfter = 0,
            float SpacingBefore = 0,
            float MultipliedLeading = 0)
        {
            Paragraph pra = new Paragraph(content, font);
            pra.Alignment = Alignment;
            if (SpacingAfter != 0)
            {
                pra.SpacingAfter = SpacingAfter;
            }

            if (SpacingBefore != 0)
            {
                pra.SpacingBefore = SpacingBefore;
            }

            if (MultipliedLeading != 0)
            {
                pra.MultipliedLeading = MultipliedLeading;
            }

            document.Add(pra);
        }

        #endregion

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
        /// <inheritdoc />
        /// <summary>
        /// The unicode font factory.
        /// </summary>
        public class UnicodeFontFactory : FontFactoryImp
        {
            /// <summary>
            /// The get font.
            /// </summary>
            /// <param name="fontname">
            /// The fontname.
            /// </param>
            /// <param name="encoding">
            /// The encoding.
            /// </param>
            /// <param name="embedded">
            /// The embedded.
            /// </param>
            /// <param name="size">
            /// The size.
            /// </param>
            /// <param name="style">
            /// The style.
            /// </param>
            /// <param name="color">
            /// The color.
            /// </param>
            /// <param name="cached">
            /// The cached.
            /// </param>
            /// <returns>
            /// The <see cref="Font"/>.
            /// </returns>
            public override Font GetFont(
                string fontname,
                string encoding,
                bool embedded,
                float size,
                int style,
                BaseColor color,
                bool cached)
            {
                BaseFont baseFont = BaseFont.CreateFont(AppDomain.CurrentDomain.BaseDirectory + "Fonts\\simhei.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED); // 黑体
                return new Font(baseFont, size, style, color);
            }
        }

    }
}
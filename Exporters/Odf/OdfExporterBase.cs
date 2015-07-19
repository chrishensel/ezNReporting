// Copyright 2015 Sascha-Christian Hensel
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using AODL.Document;
using AODL.Document.Content;
using AODL.Document.Content.Text;
using ezNReporting.Export;
using ezNReporting.Template.Composition;

namespace ezNReporting.Exporter.Odf
{
    /// <summary>
    /// Provides an abstract base class for a report exporter that exports to a suitable OpenDocument-formatted document.
    /// </summary>
    public abstract class OdfExporterBase : ReportExporterBase
    {
        #region Properties

        /// <summary>
        /// Gets the file extension used for this format (without leading dot).
        /// </summary>
        protected string FileExtension { get; private set; }

        #endregion

        #region Constructors

        private OdfExporterBase()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdfExporterBase"/> class.
        /// </summary>
        /// <param name="fileExtension">The file extension used for this format (without leading dot).</param>
        protected OdfExporterBase(string fileExtension)
            : this()
        {
            this.FileExtension = fileExtension;

            if (this.FileExtension.StartsWith("."))
            {
                this.FileExtension = this.FileExtension.Remove(0, 1);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Begins iterating
        /// </summary>
        /// <param name="element"></param>
        /// <param name="doc"></param>
        protected void WriteElement(ICompositionElement element, IDocument doc)
        {
            WriteElement(element, doc, 0);
        }

        private void WriteElement(ICompositionElement element, IDocument doc, int level)
        {
            if (element.ChildrenSupported)
            {
                foreach (ICompositionElement child in element.Children)
                {
                    WriteElement(child, doc, level + 1);
                }
            }

            OnWriteElement(element, doc, level);
        }

        /// <summary>
        /// When overridden in a derived class, writes the given element to the document.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="doc"></param>
        /// <param name="level"></param>
        protected virtual void OnWriteElement(ICompositionElement element, IDocument doc, int level)
        {
        }

        /// <summary>
        /// Creates a simple text content and adds it to the provided content collection.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="content"></param>
        /// <param name="value">The value to write. May be null.</param>
        protected static void CreateAddSimpleText(IDocument doc, IContentCollection content, object value)
        {
            Paragraph p = ParagraphBuilder.CreateStandardTextParagraph(doc);
            p.TextContent.Add(new SimpleText(doc, Convert.ToString(value ?? string.Empty)));

            content.Add(p);
        }

        /// <summary>
        /// Saves the provided document to a stream and returns it.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="doc"/> was null.</exception>
        protected Stream CreateStream(IDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }

            string filePath = Path.Combine(Path.GetTempPath(), string.Format("~odf_{0}.{1}", DateTime.UtcNow.ToFileTime(), this.FileExtension));

            FileInfo tmp = new FileInfo(filePath);

            try
            {
                using (tmp.Create()) { }

                tmp.Attributes = FileAttributes.Temporary;

                doc.SaveTo(tmp.FullName);

                MemoryStream stream = new MemoryStream();
                using (FileStream fs = tmp.OpenRead())
                {
                    fs.CopyTo(stream);
                }

                return stream;
            }
            finally
            {
                tmp.Delete();
            }
        }

        #endregion
    }
}

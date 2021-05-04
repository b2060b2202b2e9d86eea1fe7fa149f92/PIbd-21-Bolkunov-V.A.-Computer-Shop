using System;
using System.Collections.Generic;
using System.Text;
using ComputerShopBusinessLogic.HelperModels;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ComputerShopBusinessLogic.BusinessLogics
{
    static class SaveToWord
    {
        public static void CreateDoc(WordInfo info)
        {
            using (var wordDocument = WordprocessingDocument
                .Create(info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());

                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<(string, WordTextProperties)> 
                    { 
                        (info.Title, new WordTextProperties { Bold = true, Size = "24" }) 
                    },
                    TextProperties = new WordTextProperties
                    {
                        Size = "24",
                        JustificationValues = JustificationValues.Center
                    }
                }));

                foreach (var computer in info.Computers)
                {
                    docBody.AppendChild(CreateParagraph(new WordParagraph
                    {
                        Texts = new List<(string, WordTextProperties)>
                        {
                            (computer.ComputerName+"    ", new WordTextProperties { Size = "24", Bold = true}),
                            (computer.Price.ToString(), new WordTextProperties { Size = "24"})
                        },
                        TextProperties = new WordTextProperties
                        {
                            Size = "24",
                            JustificationValues = JustificationValues.Both
                        }
                    }));
                }

                docBody.AppendChild(CreateSectionProperties());
                wordDocument.MainDocumentPart.Document.Save();
            }
        }

        private static SectionProperties CreateSectionProperties()
        {
            var properties = new SectionProperties();

            var pageSize = new PageSize
            {
                Orient = PageOrientationValues.Portrait
            };

            properties.AppendChild(pageSize);

            return properties;
        }

        private static Paragraph CreateParagraph(WordParagraph paragraph)
        {
            if(paragraph != null)
            {
                var docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateSectionProperties(paragraph.TextProperties));

                foreach(var run in paragraph.Texts)
                {
                    Run docRun = new Run();

                    var properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = run.Item2.Size });
                    if(run.Item2.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }
                    docRun.AppendChild(properties);

                    docRun.AppendChild(new Text 
                    { 
                        Text = run.Item1, 
                        Space = SpaceProcessingModeValues.Preserve 
                    });

                    docParagraph.AppendChild(docRun);
                }

                return docParagraph;
            }

            return null;
        }

        private static ParagraphProperties CreateSectionProperties(WordTextProperties paragraphProperties)
        {
            if(paragraphProperties != null)
            {
                var properties = new ParagraphProperties();

                properties.AppendChild(new Justification()
                {
                    Val = paragraphProperties.JustificationValues
                });

                properties.AppendChild(new SpacingBetweenLines()
                {
                    LineRule = LineSpacingRuleValues.Auto
                });

                properties.AppendChild(new Indentation());

                var paragraphMarkRunProperties = new ParagraphMarkRunProperties();
                if(!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize 
                    {
                        Val = paragraphProperties.Size 
                    });
                }
                properties.AppendChild(paragraphMarkRunProperties);
            }

            return null;
        }
    }
}

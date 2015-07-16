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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ezNReporting.Data;
using ezNReporting.Data.Provider;
using ezNReporting.Engine;
using ezNReporting.Export;
using ezNReporting.Template;
using ezNReporting.Template.Composition;
using ezNReporting.Template.Composition.Elements;
using ezNReporting.Template.Section;
using ezNReporting.Web.Data;
using ezNReporting.Web.Models.Reports;

namespace ezNReporting.Web.Controllers
{
    public class ReportController : Controller
    {
        #region Fields

        private static readonly IReportTemplateFactory _templateFactory = new ReportTemplateFactory();
        private static readonly IReportEngineFactory _engineFactory = new ReportEngineFactory();

        #endregion

        #region Methods

        private static IDataRepository GetRepository()
        {
            return new DataRepository();
        }

        #endregion

        #region API Methods

        [HttpPost()]
        public JsonResult Create(CreateReportModel model)
        {
            IReportTemplate template = _templateFactory.Create(new DescriptionMetadata() { Name = model.Name, Author = model.CreatedBy });
            template.DataSources.Set(new DataSource("", new StaticDataProvider().Set("test", 4711).Set("switch", true)));

            // Un-comment this to add a data provider which returns a DataSet with data filled by a custom script written in C#.
            //template.DataSources.Set(new DataSource("scr1", new ScriptDataProvider()
            //{
            //    ScriptTypeKey = "cs",
            //    ScriptText = @"DataSet result = new DataSet(""Created from code""); result.Tables.Add(""Hello world""); return result;"
            //}));

            IReportTemplateSection section = template.Sections.GetSection(SectionType.Detail);

            ICompositionElement rootElement = new VerticalContainerElement();
            rootElement.Set("data-source", "");
            rootElement.AddChild(new StaticLabelElement() { Value = "Hello, world!" });
            rootElement.AddChild(new TableElement() { DataSource = "ours" });
            section.RootElement = rootElement;

            string definition = null;

            using (StreamReader reader = new StreamReader(_templateFactory.Save(template)))
            {
                definition = reader.ReadToEnd();
            }

            ReportData rd = new ReportData()
            {
                Name = model.Name,
                Guid = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now,
                CreatedBy = model.CreatedBy,
                Definition = definition
            };

            using (IDataRepository repository = GetRepository())
            {
                repository.Add(rd);
            }

            return Json(new { success = true, name = rd.Name, guid = rd.Guid }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportsList(GetReportsListModel model)
        {
            ReportData[] reports = null;

            using (IDataRepository repository = GetRepository())
            {
                reports = repository.GetReports(model.LastId, model.Count, false).ToArray();
            }

            object result = new
            {
                reports = reports.Where(_ => _.State == EntityState.Ok).ToArray(),
                lastId = reports.Max(_ => _.Id),
            };

            return new JsonNetResult(result);
        }

        public JsonResult GetGeneratedReport(GetGeneratedReportModel model)
        {
            object result = null;
            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                IReportTemplate template = null;

                using (IDataRepository repository = new DataRepository())
                {
                    ReportData report = repository.Get(model.Guid);
                    if (report != null)
                    {
                        template = _templateFactory.Create(new MemoryStream(Encoding.UTF8.GetBytes(report.Definition)));
                    }
                }

                /* Attach our custom data source.
                 */
                StaticDataProvider ours = new StaticDataProvider();
                ours.AddTable(new System.Data.DataTable());
                ours.Tables.First().Columns.Add(new System.Data.DataColumn("Id", typeof(int)) { AutoIncrement = true, AutoIncrementSeed = 1, AutoIncrementStep = 1, AllowDBNull = false });
                ours.Tables.First().Columns.Add(new System.Data.DataColumn("Text", typeof(string)));

                foreach (string value in new[] { "Hello", "World!", "This", "is", "a", "test!", "This", "report", "generator", "rocks!" })
                {
                    ours.AddRow(0, null, value);
                }

                template.DataSources.Set(new DataSource("ours", ours));

                // Uncomment the following statement to replace the above data source with one that generates random data each time.
                //template.DataSources.Set(new DataSource("ours", new ezNReporting.Web.Utilities.ezNReporting.RandomDataProvider()));

                /* Generate the report.
                 */
                IReportEngine engine = _engineFactory.Create();

                using (StreamReader reader = new StreamReader(engine.Generate(template, typeof(XHtmlReportExporter))))
                {
                    result = new
                    {
                        success = true,
                        html = reader.ReadToEnd(),
                        duration = sw.ElapsedMilliseconds,
                    };
                }
            }
            catch (Exception ex)
            {
                result = new
                {
                    success = false,
                    error = ex.Message
                };
            }
            finally
            {
                sw.Stop();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public JsonResult Delete(DeleteReportModel model)
        {
            using (IDataRepository repository = GetRepository())
            {
                repository.Delete(model.Guid);
            }

            return new JsonNetResult(new { success = true });
        }

        #endregion
    }
}
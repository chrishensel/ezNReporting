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

using System.Web.Mvc;
using ezNReporting.Web.Models.Reports;
using Newtonsoft.Json;

namespace ezNReporting.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Details(ReportDetailsModel model)
        {
            this.ViewData["guid"] = model.Guid;
            this.ViewData["exporters"] = JsonConvert.SerializeObject(new
            {
                csv = "CsvReportExporter",
                xhtml = "XHtmlReportExporter",
                odt = "OdtDocumentExporter",
                ods = "OdsDocumentExporter"
            });

            return View();
        }

        #endregion
    }
}